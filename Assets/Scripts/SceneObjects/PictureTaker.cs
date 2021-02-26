using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TheArchitect.Core;

namespace TheArchitect.SceneObjects
{
    public class PictureTaker : SceneObject
    {   
        public const string OUTCOME_CLICK = "click";

        [SerializeField] public Image ImageFlash;
        private string m_PicName;
        Texture2D tex;
        float waitTimer = 1;

        public void SetPictureName(string name)
        {
            this.m_PicName = name;
        }

        // Update is called once per frame
        void Update()
        {
            waitTimer -= Time.deltaTime;

            if (Time.deltaTime==0 || waitTimer > 0)
                return;
                
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(TakePic());
            }

            ImageFlash.color = new Color(1,1,1, Mathf.Clamp01(ImageFlash.color.a - Time.deltaTime) );
            ImageFlash.gameObject.SetActive(ImageFlash.color.a > 0);
        }

        IEnumerator TakePic()
        {
            Canvas[] allCanvas = GameObject.FindObjectsOfType<Canvas>();
            foreach (var c in allCanvas)
                c.enabled = false;

            yield return new WaitForEndOfFrame();

            try
            {
                tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
                tex.Apply();

                string name = this.m_PicName == null ? $"pic-{UnityEngine.Random.value*10000:F0}" : this.m_PicName;
                string path = $"{Application.persistentDataPath}/{GameState.PICTURES_SUB_DIRECTORY}";
                Directory.CreateDirectory(path);
                path = $"{path}/{name}.jpg";

                File.WriteAllBytes(path, tex.EncodeToJPG(80));
                Debug.Log($"Picture saved to {path}");
                
                ImageFlash.color = Color.white;
            } finally {
                Debug.Log("OUTCOME 111111");
                this.Outcome = OUTCOME_CLICK;
                foreach (var c in allCanvas)
                    c.enabled = true;

            }

        }
    }

}