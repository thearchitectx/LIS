using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using TheArchitect.Core;
using TheArchitect.Core.Constants;

namespace TheArchitect.Controllers.UI.PanelSaveIO
{

    [RequireComponent(typeof(MainThreadDispatcher))]
    public class PanelSaveIO : MonoBehaviour
    {
        public GameObject PanelSaveItemPrefab;
        public RectTransform TransformItemParent;
        public Text TextAllowed;
        public Button ButtonBack;

        private Game m_Game;
        private MainThreadDispatcher m_Dispatcher;

        void Start()
        {
            m_Game = Resources.Load<Game>(ResourcePaths.SO_GAME);
            m_Dispatcher = GetComponent<MainThreadDispatcher>();
            TextAllowed.gameObject.SetActive(!m_Game.AllowSaving);

            ReadData();
        }

        void Update()
        {
            CursorManager.RequestUnlocked();
            CursorManager.RequestVisible();
        }

        public void ReadData()
        {
            string rootPath = Application.persistentDataPath;

            foreach (Transform t in TransformItemParent)
                Destroy(t.gameObject);

            new System.Threading.Thread( () => {
                System.Threading.Thread.CurrentThread.IsBackground = true;

                string[] saveSlotsFolders = new string[11];
                saveSlotsFolders[0] = "autosave";
                for (var i = 1; i < 11; i++)
                    saveSlotsFolders[i] = $"{i:00}";

                foreach (var s in saveSlotsFolders)
                    FillSaveItem(rootPath, s);
            }).Start();
        }

        private void FillSaveItem(string rootPath, string slot)
        {
            string label = GameState.LoadLabel(rootPath, slot);
            byte[] image = GameState.LoadImage(rootPath, slot);

            this.m_Dispatcher.Enqueue( () => {
                Texture2D textureScreen = new Texture2D(256, 256, TextureFormat.RGB24, false);
                if (image.Length>0)
                    textureScreen.LoadImage(image);

                PanelSaveItem item = Instantiate(PanelSaveItemPrefab).GetComponent<PanelSaveItem>();
                item.transform.SetParent(this.TransformItemParent, false);
                item.ImageScreen.sprite = Sprite.Create(textureScreen, new Rect(0, 0, textureScreen.width, textureScreen.height), new Vector2(textureScreen.width/2, textureScreen.height/2));
                item.ImageScreen.color = image.Length>0 ? Color.white : Color.black;
                item.TextSlot.text = slot;
                item.TextLabel.text = label!=null ? label : "-- EMPTY --";
                item.TextDate.text = label != null ? GameState.LoadLastWrite(rootPath, slot) : "--";

                item.InputLabel.onEndEdit.AddListener( value => {
                    item.InputLabel.gameObject.SetActive(false);
                    item.TextLabel.gameObject.SetActive(true);
                    m_Game.State.Rename(rootPath, slot, value);
                    ReadData();
                });

                item.ButtonSave.gameObject.SetActive( slot!="autosave" && m_Game.AllowSaving);
                item.ButtonSave.onClick.AddListener(() => StartCoroutine(DoSave(rootPath, slot, label)));

                item.ButtonLoad.gameObject.SetActive(label != null);
                item.ButtonLoad.onClick.AddListener(() => DoLoad(rootPath, slot));

                item.ButtonRename.gameObject.SetActive(slot!="autosave" && label != null);
                item.ButtonRename.onClick.AddListener( () => {
                    item.InputLabel.gameObject.SetActive(true);
                    item.InputLabel.text = item.TextLabel.text;
                    item.TextLabel.gameObject.SetActive(false);
                });

                item.ButtonDelete.gameObject.SetActive(slot!="autosave" && label != null);
                item.ButtonDelete.onClick.AddListener( () => {
                    const string TEXT_CONFIRM = "CONFIRM?";
                    Text btText = item.ButtonDelete.GetComponentInChildren<Text>();
                    if (btText.text==TEXT_CONFIRM)
                    {
                        string path = GameState.GetSlotPath(rootPath, slot);
                        Directory.Delete(path, true);
                        ReadData();
                    }
                    else
                        btText.text = TEXT_CONFIRM;
                });
            });
        }

        private void DoLoad(string root, string slot)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Empty");
            GameState state = GameState.Load(root, slot);
            m_Game.State = state;
        }

        private IEnumerator DoSave(string root, string slot, string label)
        {
            // Save state
            m_Game.State.Save(root, slot, label != null ? label : $"SAVE SLOT {slot}", Application.version);

            // Prepare for screenshot
            yield return TakeSaveScreenshot(root, slot);

            ReadData();
        }

        public static IEnumerator TakeSaveScreenshot(string root, string slot)
        {
            List<Canvas> allCanvas = GameObject.FindObjectsOfType<Canvas>().Where( c => c.enabled ).ToList();
            allCanvas.ForEach( c => c.enabled = false );
            PostProcessLayer ppl = GameObject.FindObjectOfType<PostProcessLayer>();
            if (ppl!=null)
                ppl.enabled = false;

            yield return new WaitForEndOfFrame();
            
            Texture2D texScreen = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            try {
                texScreen.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
                texScreen.Apply();
                texScreen = TextureScaler.scaled(texScreen, 256, Mathf.CeilToInt(256f / Screen.width * Screen.height));
            } finally {
                // Recover from screenshot
                if (ppl!=null) ppl.enabled = true;
                allCanvas.ForEach( c => c.enabled = true );
            }

            // Write screenshot
            File.WriteAllBytes( $"{GameState.GetSlotPath(root, slot)}/{GameState.SCREEN_FILE_NAME}" , texScreen.EncodeToJPG(80));
        }

    }
}