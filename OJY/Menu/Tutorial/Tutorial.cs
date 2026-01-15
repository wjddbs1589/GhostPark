using UnityEngine;
using UnityEngine.UI;
using TMPro;

//2025-06-27 OJY
public class Tutorial : MonoBehaviour
{
    int pageIndex = 0;
    int lastPage;

    MenuManager menuManager;

    [Header("튜토리얼 페이지 데이터")]
    [SerializeField] TutorialData data;

    [SerializeField] Image stepImage;
    [SerializeField] Image infoImage;
    [SerializeField] TextMeshProUGUI infoText;

    [Header("페이지 변경")]
    [SerializeField] GameObject Obj_prevBtn;
    [SerializeField] GameObject Obj_nextBtn;
    Button Btn_previousPage;
    Button Btn_nextPage;

    [Header("페이지 이동")]
    [SerializeField] Button Btn_Exit;
    [SerializeField] Button Btn_Start;

    [Header("게임시작 버튼")]
    [SerializeField] GameObject Obj_startBtn;    

    private void Awake()
    {
        menuManager = MenuManager.Instance;

        pageIndex = 0;
        lastPage = data.StepImages.Length - 1;

        Btn_previousPage = Obj_prevBtn.GetComponent<Button>();
        Btn_previousPage.onClick.AddListener(SetPerviousPage);

        Btn_nextPage = Obj_nextBtn.GetComponent<Button>();
        Btn_nextPage.onClick.AddListener(SetNextPage);

        Btn_Exit.onClick.AddListener(Close);
        Btn_Start.onClick.AddListener(StartGame);      
    }

    private void OnEnable()
    {
        pageIndex = 0; //페이지 번호 초기화
        SetPage();     //페이지 갱신
    }

    #region 페이지 정보 설정
    /// <summary>
    /// 이전 페이지로 이동
    /// </summary>
    void SetPerviousPage()
    {        
        pageIndex--;
        SoundManager.Instance.PlaySFX(SfxType.tutorial);
        SetPage();
    }

    /// <summary>
    /// 다음 페이지로 이동
    /// </summary>
    void SetNextPage()
    {
        pageIndex++;

        SoundManager.Instance.PlaySFX(SfxType.tutorial);
        SetPage();
    }

    /// <summary>
    /// 페이지 정보 갱신
    /// </summary>
    void SetPage()
    {
        stepImage.sprite = data.StepImages[pageIndex];
        infoImage.sprite = data.InfoImages[pageIndex];
        infoText.text = data.InfoTexts[pageIndex];

        BtnSetting(pageIndex);
    }

    /// <summary>
    /// 첫 페이지와 끝 페이지의 넘기기 버튼 비활성화
    /// </summary>
    /// <param name="index"></param>
    void BtnSetting(int index)
    {
        Obj_prevBtn.SetActive(true);   //이전 페이지 버튼
        Obj_nextBtn.SetActive(true);   //다음 페이지 버튼
        Obj_startBtn.SetActive(false); //게임 시작 버튼

        //첫페이지
        if (pageIndex == 0)
        {
            Obj_prevBtn.SetActive(false);
        }
        //마지막 페이지
        else if (pageIndex == lastPage)
        {
            Obj_nextBtn.SetActive(false);
            Obj_startBtn.SetActive(true); //튜토리얼 마지막 페이지에서만 시작버튼 활성화
        }
    }
    #endregion
    #region 버튼
    void Close()
    {
        menuManager.OpenMenu();
    }

    void StartGame()
    {
        menuManager.StartGame();
    }
    #endregion


}