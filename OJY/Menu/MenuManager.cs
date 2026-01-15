using UnityEngine;
using UnityEngine.UI;

//2025-06-16 OJY
public class MenuManager : MonoBehaviour
{
    static MenuManager instance;
    public static MenuManager Instance { get { return instance; } }


    [Header("타이틀 화면 버튼")]
    [SerializeField] Button Btn_Tutorial;
    [SerializeField] Button Btn_Document;
    [SerializeField] Button Btn_Exit;
    [SerializeField] Button Btn_Start;
    [SerializeField] Button Btn_Setting_title;

    [Header("기능 별 오브젝트")]
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Tutorial;
    [SerializeField] GameObject Document;
    [SerializeField] GameObject Game;

    [Header("상단UI")]
    [SerializeField] GameObject IconUI;

    [Header("환경설정")]
    [SerializeField] Button Btn_Setting;
    [SerializeField] GameObject SettingObj;

    GameObject currentObj;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentObj = MainMenu;

        Btn_Setting_title.onClick.AddListener(OpenSettiing);
        Btn_Setting.onClick.AddListener(OpenSettiing);
        Btn_Tutorial.onClick.AddListener(OpenTutorial);
        Btn_Document.onClick.AddListener(OpenDocument);
        Btn_Start.onClick.AddListener(StartGame);
        Btn_Exit.onClick.AddListener(() => { Application.Quit(); });

        IconUI.SetActive(false);
    }

    #region UI 활성화
    public void OpenMenu()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);
        ActiveObj(MainMenu);
    }
    public void OpenTutorial()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);
        ActiveObj(Tutorial);
        IconUI.SetActive(false);
    }
    public void OpenDocument()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);
        ActiveObj(Document);
    }
    public void StartGame()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);
        ActiveObj(Game);        
    }
    void OpenSettiing()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);
        SettingObj.SetActive(true);
    }

    /// <summary>
    /// 활성화 대상 변경
    /// </summary>
    /// <param name="obj"></param>
    void ActiveObj(GameObject obj) 
    {
        if(obj == MainMenu)
        {
            IconUI.SetActive(false); //아이콘 UI 비활성화
        }
        else
        {
            IconUI.SetActive(true); //아이콘 UI 활성화
        }

        //현재 오브젝트를 이전 오브젝트로 지정 및 비활성화
        currentObj.SetActive(false);
        currentObj = obj;
        currentObj.SetActive(true);
    }
    #endregion
}