using UnityEngine;
using UnityEngine.UI;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Controllers.UI.PanelSaveIO;
using TheArchitect.Core.Controllers;

public class Title : MonoBehaviour
{
    [SerializeField] private Text m_TextVersion;
    [SerializeField] private Text m_TextVersionAlert;
    [SerializeField] private GameObject m_CanvasSavePrefab;
    [SerializeField] private GameObject m_CanvasTrophiesPrefab;
    [SerializeField] private Canvas m_CanvasMenu;

    private GameObject m_CanvasSave;
    private GameObject m_CanvasTrophies;

    void Start()
    {
        this.m_TextVersion.text = Application.version;
        if (!Application.version.ToUpper().Contains("PREVIEW"))
        {
            Destroy(m_TextVersionAlert.transform.parent.gameObject);
        }
    }

    public void MenuQuit()
    {
        Application.Quit();
    }

    public void MenuLoad()
    {
        this.m_CanvasMenu.gameObject.SetActive(false);
        this.m_CanvasSave = Instantiate(m_CanvasSavePrefab);
        this.m_CanvasSave.GetComponent<Canvas>().sortingOrder = 1000;
        this.m_CanvasSave.GetComponentInChildren<PanelSaveIO>().ButtonBack.onClick.AddListener(
            () => {
                Destroy(this.m_CanvasSave);
                this.m_CanvasMenu.gameObject.SetActive(true);
            }
        );
    }

    public void MenuTrophies()
    {
        this.m_CanvasMenu.gameObject.SetActive(false);
        this.m_CanvasTrophies = Instantiate(m_CanvasTrophiesPrefab);
        this.m_CanvasTrophies.GetComponent<Canvas>().sortingOrder = 1000;
        this.m_CanvasTrophies.GetComponentInChildren<PanelTrophies>().ButtonBack.onClick.AddListener(
            () => { 
                Destroy(this.m_CanvasTrophies);
                this.m_CanvasMenu.gameObject.SetActive(true);
            }
        );
    }

    public void MenuStart()
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(ResourcePaths.PREFAB_FADE_TO_BLACK));
        go.transform.SetParent(this.transform);
        Invoke("MenuStartSync", 1);
    }

    public void MenuStartSync()
    {
        Game game = Resources.Load<Game>(ResourcePaths.SO_GAME);
        game.NewGame();
    }
}