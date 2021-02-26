using UnityEngine;
using UnityEngine.UI;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Controllers.UI.PanelSaveIO;

public class Title : MonoBehaviour
{
    [SerializeField] private Text m_TextVersion;
    [SerializeField] private Text m_TextVersionAlert;
    [SerializeField] private GameObject m_CanvasSavePrefab;

    private GameObject m_CanvasSave;

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
        this.m_CanvasSave = Instantiate(m_CanvasSavePrefab);
        this.m_CanvasSave.GetComponent<Canvas>().sortingOrder = 1000;
        this.m_CanvasSave.GetComponentInChildren<PanelSaveIO>().ButtonBack.onClick.AddListener(() => Destroy(this.m_CanvasSave));
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