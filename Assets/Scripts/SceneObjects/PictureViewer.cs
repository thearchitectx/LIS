using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace TheArchitect.SceneObjects
{

    public class PictureViewer : SceneObject
    {
        [SerializeField] private RectTransform m_Container;
        [SerializeField] private Image m_ImagePicture;
        [SerializeField] private Image m_ImageBorder;
        [SerializeField] private Image m_ImageForward;

        public string PicturePath = null;
        private Sprite m_ImageSprite = null;
        private byte[] m_Buf = null;
        private float m_Timer = 0.5f;


        public void LoadPicture(string name)
        {
            if (this.PicturePath==null || this.PicturePath=="")
                this.PicturePath = $"{TheArchitect.Core.GameState.GetPicturesPath(Application.persistentDataPath)}";

            this.m_Buf = null;
            this.m_ImageSprite = null;
                
            new System.Threading.Thread( () => {
                System.Threading.Thread.CurrentThread.IsBackground = true;
                this.m_Buf = File.ReadAllBytes($"{PicturePath}/{name}.jpg");
            }).Start();
        }

        public void LoadSprite(string spriteResource)
        {
            this.m_ImageSprite = Resources.Load<Sprite>(spriteResource);
            if (this.m_ImageSprite!=null)
            {
                this.m_ImagePicture.sprite = this.m_ImageSprite;
                this.m_ImagePicture.color = Color.white;
                this.m_Container.sizeDelta = new Vector2(this.m_ImageSprite.texture.width, this.m_ImageSprite.texture.height);
            }
        }

        public void HideImageForward()
        {
            this.m_ImageForward.gameObject.SetActive(false);
        }

        
        public void SetBorderColor(string htmlColor)
        {
            Color c = Color.white;
            if (ColorUtility.TryParseHtmlString(htmlColor, out c))
                this.m_ImageBorder.color = c;
            
        }

        void Update()
        {
            if (this.m_Buf != null && this.m_ImageSprite==null)
            {
                Texture2D tex = new Texture2D(16, 16, TextureFormat.RGB24, false);
                tex.LoadImage(this.m_Buf);
                this.m_Buf = null;

                this.m_ImageSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width/2, tex.height/2) );
                this.m_ImagePicture.sprite = m_ImageSprite;
                this.m_ImagePicture.color = Color.white;
                
                m_Container.sizeDelta = new Vector2(tex.width / 1.6f, tex.height / 1.6f);
            }

            if (this.m_Timer <= 0 && (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")))
            {
                this.Outcome = "DONE";
            }

            this.m_Timer -= Time.deltaTime;
        }
    }

}

