using System.Collections;
using System.Collections.Generic;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Data;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Core;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TheArchitect.Core.Controllers;

public class GalleryController : MonoBehaviour
{
    public const string CHARACTER_PATH = "Character";
    public const string CATEGORY_BASIC = "BASIC";
    public const string PREF_CHARACTER_UNLOCK_TEMPLATE = "GALLERY_{0}_{1}";
    
    [SerializeField] private RectTransform m_TransformPanelLeft;
    [SerializeField] private Dropdown m_DropDownCharacter;
    [SerializeField] private Dropdown m_DropDownIdle;
    [SerializeField] private Dropdown m_DropDownEyes;
    [SerializeField] private Slider m_SliderShape;
    [SerializeField] private Text m_TextLockedCharacter;
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    [SerializeField] private CinemachineTargetGroup targetGroup;

    [SerializeField] private CharacterListVariable m_Characters;
    [SerializeField] private Sprite m_SpriteLock;
    [SerializeField] private Sprite m_SpriteTransparent;

    private Transform m_TransformCharacter;
    private IdleAnim[] m_Loops;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var c in m_Characters.Value)
        {
            m_DropDownCharacter.options.Add(
                new Dropdown.OptionData(c.DisplayName, IsUnlocked(c.name, CATEGORY_BASIC) ? m_SpriteTransparent : m_SpriteLock)
            );
        }

        m_DropDownCharacter.onValueChanged.AddListener( i => {
            SetCharacter();
        });

        m_Loops = new IdleAnim[] {
            new IdleAnim() { name="UNEASY", intValue = 2 },
            new IdleAnim() { name="UPSET", intValue = 5 },
            new IdleAnim() { name="COCKY", intValue = 7 },
            new IdleAnim() { name="EMBARRASSED", intValue = 6 },
            new IdleAnim() { name="STAND ARMS CROSSED", intValue = 8 }
        };
        foreach (var a in m_Loops)
            m_DropDownIdle.options.Add(new Dropdown.OptionData(a.name, m_SpriteTransparent));

        m_DropDownIdle.onValueChanged.AddListener( i => {
            SetLoop();
        });

        m_SliderShape.onValueChanged.AddListener( v => SetShape() );

        SetCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        this.cinemachine.m_Lens.FieldOfView += 
            Input.GetAxis("Mouse Y")  * (Input.GetMouseButton(1)?1:0) * Time.deltaTime * 10;

        this.cinemachine.m_Lens.FieldOfView = Mathf.Clamp(this.cinemachine.m_Lens.FieldOfView, 5, 30);

        this.cinemachine.transform.position = new Vector3(
            this.cinemachine.transform.position.x,
            Mathf.Clamp(this.cinemachine.transform.position.y + (Input.GetAxis("Vertical") ) * Time.deltaTime, 0 , 1.5f),
            Mathf.Clamp(this.cinemachine.transform.position.z + (Input.GetAxis("Horizontal") ) * Time.deltaTime, 1.6f , 5));
        
        if (this.m_TransformCharacter != null)
            this.m_TransformCharacter.Rotate(0, Input.GetAxis("Mouse X") * (Input.GetMouseButton(1)?1:0) * Time.deltaTime * 60f, 0, Space.Self);
    }

    void SetShape()
    {
        CharacterInitializer.ApplyBlendShape(this.m_TransformCharacter, SkeletonPaths.BLENDSHAPE_ENHANCED, (int) m_SliderShape.value );
    }

    void SetLoop()
    {
        var t = this.transform.Find(CHARACTER_PATH);
        if (t != null)
        {
            Animator anim = t.GetComponent<Animator>();
            anim.SetInteger("idle", m_Loops[m_DropDownIdle.value].intValue);
        }
    }

    void SetCharacter()
    {
        var t = this.transform.Find(CHARACTER_PATH);
        if (t != null)
            Destroy(t.gameObject);

        Character chr = this.m_Characters.Value[m_DropDownCharacter.value];
        if (!IsUnlocked(chr.name, CATEGORY_BASIC))
        {
            this.m_TextLockedCharacter.text = $"FIND {chr.DisplayName.ToUpper()}'S GALLERY TOKEN DURING GAME PLAY TO UNLOCK";
            this.m_TextLockedCharacter.gameObject.SetActive(true);
        }
        else
        {
            this.m_TextLockedCharacter.gameObject.SetActive(false);
            var path = $"Gallery/{chr.name}";
            var model = GameObject.Instantiate(Resources.Load<GameObject>(path));
            model.name = CHARACTER_PATH;
            model.transform.SetParent(this.transform);
            this.m_TransformCharacter = model.transform;
            Invoke("SetShape", 0.5f);
            Invoke("SetLoop", 0.5f);
        }
    }

    public void TogglePanelLeft()
    {
        if (this.m_TransformPanelLeft.anchoredPosition.x == 0) 
            this.m_TransformPanelLeft.anchoredPosition = new Vector2(-this.m_TransformPanelLeft.rect.width, this.m_TransformPanelLeft.anchoredPosition.y );
        else
            this.m_TransformPanelLeft.anchoredPosition = new Vector2(0, this.m_TransformPanelLeft.anchoredPosition.y );
    }

    public void QuitToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }


    public static void Unlock(string character, string category)
    {
        PlayerPrefs.SetInt(string.Format(PREF_CHARACTER_UNLOCK_TEMPLATE, character, character), 1);
        PlayerPrefs.Save();
    }

    public static bool IsUnlocked(string character, string category)
    {
        return PlayerPrefs.GetInt(string.Format(PREF_CHARACTER_UNLOCK_TEMPLATE, character, character), 0) > 0;
    }

    public struct IdleAnim {
        public string name;
        public int intValue;
    }
}
