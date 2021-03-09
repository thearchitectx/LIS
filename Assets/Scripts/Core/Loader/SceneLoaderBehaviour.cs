using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TheArchitect.Core.Data;

namespace TheArchitect.Core.Loader
{
    
    public class SceneLoaderBehaviour : MonoBehaviour
    {
        [SerializeField] private Image m_ImageProgress;
        [SerializeField] private Text m_TextProgress;
        
        private AsyncOperation m_LoadSceneOperation;
        private ResourceRequest m_LoadMechanicOperation;
        private Stage m_Stage;

        public void Load(Stage stage)
        {
            this.m_Stage = stage;
        }

        void Start()
        {
            m_TextProgress.color = Color.black;
        }

        // Update is called once per frame
        void Update()
        {
            if (this.m_Stage != null && this.m_LoadSceneOperation == null)
            {
                this.m_LoadSceneOperation = SceneManager.LoadSceneAsync(this.m_Stage.Scene.ToString());
                this.m_LoadSceneOperation.completed += op => {
                    if (!string.IsNullOrEmpty(this.m_Stage.MechanicPrefab))
                    {
                        m_LoadMechanicOperation = Resources.LoadAsync<GameObject>(this.m_Stage.MechanicPrefab);
                        m_LoadMechanicOperation.completed += op2 => {
                            GameObject prefab = m_LoadMechanicOperation.asset as GameObject;
                            GameObject mechanic = GameObject.Instantiate(prefab);
                            mechanic.name = prefab.name;
                        };
                    }
                };
            }

            if (this.m_LoadSceneOperation != null)
            {
                this.m_ImageProgress.fillAmount = m_LoadSceneOperation.progress;
            }
            if (this.m_LoadMechanicOperation != null)
            {
                this.m_ImageProgress.fillAmount = m_LoadMechanicOperation.progress;
            }

            float col = m_TextProgress.color.r+Time.deltaTime;
            this.m_TextProgress.color = new Color(col, col, col);
            this.m_ImageProgress.color = new Color(0, 0, col);

        }
    }

}
