using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TheArchitect.Core.Constants;
using TheArchitect.Core;
using TheArchitect.Core.Controllers;
using TheArchitect.Core.Data;

namespace TheArchitect.Controllers.UI.PanelPause
{

    public class PauseController : MonoBehaviour
    {
        public const string PLAYER_PREF_SHOW_FPS = "PauseController.SHOW_FPS";
        public const string PLAYER_PREF_HIDE_OBJECTIVES = "PauseController.HIDE_OBJECTIVES";
        public const string PLAYER_PREF_IGNORE_EXCEPTION = "PauseController.PLAYER_PREF_IGNORE_EXCEPTION";
        [SerializeField] public GameObject CanvasPausePrefab;
        [SerializeField] public GameObject CanvasSavePrefab;
        [SerializeField] public GameObject CanvasHelpPrefab;
        [SerializeField] public GameObject CanvasExceptionPrefab;
        [SerializeField] public GameObject CanvasObjectivePrefab;
        [SerializeField] public GameObject CanvasTrophiesPrefab;
        [SerializeField] public GameObject CanvasFPSPrefab;
        [SerializeField] private PostProcessVolume PostProcessPause;

        private Transform m_CanvasFPS;
        private Transform m_CanvasPause;
        private Transform m_CanvasSave;
        private Transform m_CanvasHelp;
        private Transform m_CanvasTrophies;
        private PanelException m_PanelException;
        private Transform m_CanvasObjective;
        private Canvas[] m_DisabledCanvases;

        private bool m_IgnoreException;

        void Start()
        {
            if (PlayerPrefs.GetInt(PLAYER_PREF_HIDE_OBJECTIVES, 0) == 0)
                OpenObjectives();
            if (PlayerPrefs.GetInt(PLAYER_PREF_SHOW_FPS, 0) == 1)
                ShowFPS();
            this.m_IgnoreException = PlayerPrefs.GetInt(PLAYER_PREF_IGNORE_EXCEPTION, 0) == 1;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleException;
        }

        void OnEnable()
        {
            Application.logMessageReceived += HandleException;
        }

        void Update()
        {
            if (Input.GetButtonDown("Options"))
            {
                if (this.m_CanvasPause == null)
                    Open();
                else
                    Close();
            }

            if (m_CanvasPause == null && Input.GetKeyDown(KeyCode.F1))
            {
                if (this.m_CanvasHelp == null)
                    OpenHelp();
                else
                    CloseHelp();
            }

            if (m_CanvasPause == null && Input.GetKeyDown(KeyCode.F12))
            {
                this.m_IgnoreException = !this.m_IgnoreException;
                PlayerPrefs.SetInt(PLAYER_PREF_IGNORE_EXCEPTION, this.m_IgnoreException ? 1 : 0);
                PlayerPrefs.Save();
                Resources.Load<TheArchitect.Core.Data.Variables.Console>(ResourcePaths.SO_CONSOLE).Log("EXCEPTION DIALOG "+ (this.m_IgnoreException?"DISABLED":"ENABLED"));
            }
            if (m_CanvasPause == null && Input.GetKeyDown(KeyCode.F11))
            {
                ToggleFPS();
            }

            if (m_CanvasPause == null && Input.GetKeyDown(KeyCode.Tab))
            {
                if (this.m_CanvasObjective == null)
                    OpenObjectives();
                else
                    CloseObjectives();
            }

            PostProcessPause.weight = Mathf.Clamp01(PostProcessPause.weight + Time.unscaledDeltaTime * 2 * (this.m_CanvasPause == null ? -1 : 1));
            PostProcessPause.gameObject.SetActive(PostProcessPause.weight > 0);
        }

        public void Open()
        {
            CloseHelp();
            this.m_DisabledCanvases = GameObject.FindObjectsOfType<Canvas>();
            foreach (Canvas c in this.m_DisabledCanvases)
                c.enabled = false;

            this.m_CanvasPause = Instantiate(this.CanvasPausePrefab).GetComponent<Transform>();
            this.m_CanvasPause.SetParent(this.transform, false);
            Time.timeScale = 0;
        }

        public void Close()
        {
            if (this.m_CanvasSave!=null)
            {
                Destroy(this.m_CanvasSave.gameObject);
                this.m_CanvasSave = null;
            }

            if (this.m_CanvasTrophies!=null)
            {
                Destroy(this.m_CanvasTrophies.gameObject);
                this.m_CanvasTrophies = null;
            }
            
            Destroy(this.m_CanvasPause.gameObject);
            this.m_CanvasPause = null;

            Time.timeScale = 1;

            foreach (Canvas c in this.m_DisabledCanvases)
                if (c!=null) c.enabled = true;

            this.m_DisabledCanvases = null;
        }

        public void ToggleFPS()
        {
            if (m_CanvasFPS==null)
                ShowFPS();
            else
                HideFPS();
        }

        public void ShowFPS()
        {
            m_CanvasFPS = GameObject.Instantiate(CanvasFPSPrefab).transform;
            m_CanvasFPS.SetParent(this.transform);
            PlayerPrefs.SetInt(PLAYER_PREF_SHOW_FPS, 1);
        }

        public void HideFPS()
        {
            Destroy(m_CanvasFPS.gameObject);
            PlayerPrefs.SetInt(PLAYER_PREF_SHOW_FPS, 0);
        }

        public void OpenHelp()
        {
            this.m_CanvasHelp = Instantiate(CanvasHelpPrefab).GetComponent<Transform>();
            this.m_CanvasHelp.SetParent(this.transform, false);
        }

        public void CloseHelp()
        {
            if (this.m_CanvasHelp!=null)
                Destroy(this.m_CanvasHelp.gameObject);
        }

        public void OpenObjectives()
        {
            PlayerPrefs.SetInt(PLAYER_PREF_HIDE_OBJECTIVES, 0);
            PlayerPrefs.Save();
            if (this.m_CanvasObjective == null)
            {
                this.m_CanvasObjective = Instantiate(CanvasObjectivePrefab).GetComponent<Transform>();
                this.m_CanvasObjective.SetParent(this.transform, false);

            }
        }

        public void CloseObjectives()
        {
            PlayerPrefs.SetInt(PLAYER_PREF_HIDE_OBJECTIVES, 1);
            PlayerPrefs.Save();
            if (this.m_CanvasObjective!=null)
                Destroy(this.m_CanvasObjective.gameObject);
        }


        public void ToggleLoadSave()
        {
            if (this.m_CanvasPause.gameObject.activeSelf)
            {
                this.m_CanvasSave = Instantiate(this.CanvasSavePrefab).GetComponent<Transform>();
                this.m_CanvasSave.SetParent(this.transform, false);
                this.m_CanvasSave.GetComponentInChildren<TheArchitect.Controllers.UI.PanelSaveIO.PanelSaveIO>()
                    .ButtonBack.onClick.AddListener(() => ToggleLoadSave()
                );
                this.m_CanvasPause.gameObject.SetActive(false);
            }
            else
            {
                Destroy(this.m_CanvasSave.gameObject);
                this.m_CanvasPause.gameObject.SetActive(true);
            }

        }

        public void ToggleTrophies()
        {
            if (this.m_CanvasTrophies == null)
            {
                this.m_CanvasTrophies = Instantiate(this.CanvasTrophiesPrefab).GetComponent<Transform>();
                this.m_CanvasTrophies.SetParent(this.transform, false);
                this.m_CanvasTrophies.GetComponentInChildren<PanelTrophies>()
                    .ButtonBack.onClick.AddListener(() => ToggleTrophies()
                );
                this.m_CanvasPause.gameObject.SetActive(false);
            }
            else
            {
                Destroy(this.m_CanvasTrophies.gameObject);
                this.m_CanvasPause.gameObject.SetActive(true);
            }

        }

        private void HandleException(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Exception && this.m_PanelException == null && !this.m_IgnoreException)
            {
                Time.timeScale = 0;
                this.m_PanelException = Instantiate(CanvasExceptionPrefab).GetComponentInChildren<PanelException>();
                this.m_PanelException.transform.SetParent(this.transform, false);
                this.m_PanelException.TextException.text = condition + "\n"+ stackTrace;
                this.m_PanelException.ButtonWhatever.onClick.AddListener(() => {
                    Destroy(this.m_PanelException.gameObject);
                    Time.timeScale = 1;
                });
                this.m_PanelException.ButtonStopShowing.onClick.AddListener(() => {
                    PlayerPrefs.SetInt(PLAYER_PREF_IGNORE_EXCEPTION, 1);
                    Destroy(this.m_PanelException.gameObject);
                    Time.timeScale = 1;
                });
                this.m_PanelException.ButtonCopy.onClick.AddListener( () => {
                    GUIUtility.systemCopyBuffer = this.m_PanelException.TextException.text;
                    this.m_PanelException.ButtonCopy.GetComponentInChildren<Text>().text = "COPIED!";
                });

            }
        }
    }

}
