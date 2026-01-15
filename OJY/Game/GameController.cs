using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//2025-06-24 OJY
//2025-07-01 OJY 수정
//2025-07-08 OJY 수정
public class GameController : MonoBehaviour
{
    enum State
    {
        Searching = 0,
        Spawn,
        Choice,
        Catch
    }
    State currentState;
    bool isQR = false;

    static GameController instance;
    public static GameController Instance { get { return instance; } }


    GameManager gameManager;  //게임 매니저
    MenuManager menuManager;  //UI이동 매니저

    Image background; //기본배경. 카메라 가리개

    [Header("귀신 데이터")]
    [SerializeField] GhostData data;
    [SerializeField] Image GhostImageTarget;
    GhostInfo info;
    int ghostIndex;

    [Header("UI")]
    [SerializeField] Button Btn_Exit;
    [SerializeField] Button Btn_Document;

    [Header("스캔")]
    [SerializeField] GameObject ScanObj;

    [Header("탐색")]
    [SerializeField] GameObject SearchingObj;
    [SerializeField] MonsterSeraching monsterSeraching; //탐색 매니저

    [Header("소환")]
    [SerializeField] GameObject SpawnObj;
    [SerializeField] Button Btn_Spawn;    //소환 

    [Header("선택")]
    [SerializeField] GameObject ChoiceObj;
    [SerializeField] Button Btn_Select;   //선택
    [SerializeField] Button Btn_Pass;     //지나치기

    [Header("수집")]
    [SerializeField] GameObject CatchObj;
    [SerializeField] GameObject SuccessOj;
    [SerializeField] GameObject FailObj;

    CatchResult catchResult;
    GameObject currentObj;

    [Header("호리병")]
    [SerializeField] GameObject obj_ghostbottle;

    [Header("테스트용 버튼")]
    [SerializeField] Button testButton;

    private void Awake()
    {
        testButton.gameObject.SetActive(false);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentObj = SearchingObj;

        gameManager = GameManager.Instance;
        menuManager = MenuManager.Instance;

        background = transform.parent.GetComponent<Image>();
        catchResult = SuccessOj.GetComponent<CatchResult>();

        BtnBinding();

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        MonsterManager.Inst.ClearMonster();
    }

    private void Update()
    {
        if(currentState == State.Searching)
        {
            //탐색 중일때 걸음 수 체크
            if (monsterSeraching.isSerching) //100m 이동시
            {
                SearchSuccess(); //탐색 성공
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
        {
            SearchSuccess();
        }
#endif
    }

    private void OnEnable()
    {
        ChangeState(State.Searching); //활성화 시 탐색 단계로 세팅
    }

    void BtnBinding()
    {
        Btn_Spawn.onClick.AddListener(SpawnGhost);
        Btn_Select.onClick.AddListener(Select);
        Btn_Pass.onClick.AddListener(Pass);
        Btn_Exit.onClick.AddListener(Exit);
        Btn_Document.onClick.AddListener(OpenDocument);
    }

    void ChangeState(State state)
    {
        currentObj.SetActive(false);
        obj_ghostbottle.SetActive(false);

        currentState = state;
        switch (currentState)
        {
            //탐색
            case State.Searching:
                currentObj = SearchingObj;
                InitSetting();
                break;
            //혼령 소환
            case State.Spawn:
                monsterSeraching.ResetSeraching(); //탐색 매니저 초기화
                currentObj = SpawnObj;
                break;
            //선택(수집 or 지나치기)
            case State.Choice:
                currentObj = ChoiceObj;
                break;
            //혼령 수집 (호리병 누르기)
            case State.Catch:
                obj_ghostbottle.SetActive(true);
                currentObj = CatchObj;                
                break;
            default:
                break;
        }

        currentObj.SetActive(true);
    }

    /// <summary>
    /// 탐색단계 돌입시 세팅 초기화
    /// </summary>
    void InitSetting()
    {
        background.enabled = true;        //배경 활성화
        GhostImageTarget.enabled = false; //혼령 이미지 숨김
    }

    public void OpenDocument()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);
        background.enabled = true;
        menuManager.OpenDocument();
    }

    #region 탐색 및 소환
    /// <summary>
    /// 100걸음 이동 후 호출, 랜덤으로 혼령 생성
    /// </summary>
    public void SearchSuccess()
    {
        ChangeState(State.Spawn);
    }

    /// <summary>
    /// UI변경 및 혼령이미지 소환
    /// </summary>
    void SpawnGhost()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);

        ChangeState(State.Choice); //상태 변환
        int rand = Random.Range(0, 100); //랜덤값 생성

        //랜덤 귀신 선택
        if (90 > rand) //90% 확률로 일반귀신 소환
        {
            if (data.CommonInfos.Length > 0)
            {
                ghostIndex = Random.Range(0, data.CommonInfos.Length);
                info = data.CommonInfos[ghostIndex];
            }
        }
        else // 10% 확률로 레어귀신 소환
        {
            if (data.RareInfos.Length > 0)
            {
                ghostIndex = Random.Range(0, data.RareInfos.Length);
                info = data.RareInfos[ghostIndex];
            }
        }

        //이미지 세팅
        GhostImageTarget.enabled = true;
        if (info.ghostImage == null)
        {
            return;
        }
        GhostImageTarget.sprite = info.ghostImage;

#if UNITY_EDITOR
        Debug.Log($"혼령 소환 : {info.name} (인덱스 : {ghostIndex})");
#endif
        
    }
    #endregion

    #region QR스캔
    /// <summary>
    /// QR스캔 버튼
    /// </summary>
    public void QRcodeScan()
    {
        if (currentState == State.Catch) return; //AR카메라 사용중일땐 QR 스캔불가

        isQR = true;

        menuManager.StartGame();     //다른 UI에서 전환할때 필요
        currentObj.SetActive(false); //현재 오브젝트 비활성화
        ScanObj.SetActive(true);

        if (!ARManager.Instance) return;
        if (!ARManager.Instance.IsAR) ARManager.Instance.OnAR(); //AR 활성화
        else ARManager.Instance.OffAR(); //AR 활성화

        background.enabled = false; //배경 활성화
    }

    /// <summary>
    /// QR코드 정보를 받았을때
    /// </summary>
    public void UseQRcodeData(VampireInfo jsonData) 
    {
        ghostIndex = 0;
        info = data.LegendaryInfos[ghostIndex];

        //드라큘라 QR 이고 미발견 상태일 때
        if (!info.isFound && jsonData.FindLegendary)
        {
            ScanObj.SetActive(false);
            
            background.enabled = false;       //카메라 사용위해 배경 감추기
            GhostImageTarget.enabled = false; //발견된 혼령 이미지 감추기

            ChangeState(State.Catch);

            if (!ARManager.Instance) return;
            ARManager.Instance.OnAR();

            MonsterManager.Inst.SpawnMonster(info, ghostIndex);
        }
    }

    /// <summary>
    /// QR스캔 UI의 나가기 버튼
    /// </summary>
    void ScanCancel()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);

        background.enabled = true;        //배경 활성화

        if (!ARManager.Instance) return;
        if (!ARManager.Instance.IsAR) ARManager.Instance.OnAR(); //AR 비활성화
        else ARManager.Instance.OffAR(); //AR 비활성화

        isQR = false;
        ScanObj.SetActive(false);
        currentObj.SetActive(true); //현재 오브젝트 활성화
    }
    #endregion

    #region 선택/패스
    /// <summary>
    /// 현재 탐색된 혼령을 선택
    /// </summary>
    void Select()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);
        ChangeState(State.Catch);

        background.enabled = false;       //카메라 사용위해 배경 감추기
        GhostImageTarget.enabled = false; //발견된 혼령 이미지 감추기

        //여기에 카메라 기능 추가
        if (ARManager.Instance == null)
        {
#if UNITY_EDITOR
            Debug.LogError("ARManager is not initialized.");
#endif
            return;
        }

        if (!ARManager.Instance) return;
        if (!ARManager.Instance.IsAR) ARManager.Instance.OnAR(); //AR 활성화
        else ARManager.Instance.OffAR(); //AR 활성화

        MonsterManager.Inst.SpawnMonster(info, ghostIndex); //혼령 소환
    }

    /// <summary>
    /// 현재 탐색된 혼령을 포기하고 처음으로 복귀
    /// </summary>
    void Pass()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);

        ChangeState(State.Searching);     //처음으로 돌아감

        if (!ARManager.Instance) return;
        if (!ARManager.Instance.IsAR)
        {
            ARManager.Instance.OnAR(); //AR 비활성화
        }
        else
        {
            ARManager.Instance.OffAR(); //AR 비활성화
        }
    }
    #endregion

    #region 수집단계
    /// <summary>
    /// 보상 획득 및 초기 화면으로 이동
    /// </summary>
    public void CatchSuccess()
    {
        info.isFound = true;

        SaveFoundData(info.ghostType);

        FindAnyObjectByType<CatchTimer>().StopTimer();

        //도감 띄움        
        SuccessOj.SetActive(true);
        catchResult.SetResult(info);
    }

    void SaveFoundData(MonsterType type)
    {
        switch (type) 
        {
            case MonsterType.Rare:
                ghostIndex += 7;
                break;
            case MonsterType.Legendary:
                ghostIndex += 9;
                break;
        }

        string key = $"isFound_{ghostIndex}";
        PlayerPrefs.SetInt(key, info.isFound ? 1 : 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 혼령 탐색 단계로 이동
    /// </summary>
    public void ReturnToSearching()
    {
        Pass();
        FailObj.SetActive(false);
        MonsterManager.Inst.ClearMonster(); //소환된 몬스터 초기화
    }

    /// <summary>
    /// 시간 초과
    /// </summary>
    public void TimeOver()
    {
        FailObj.SetActive(true);
    }
    #endregion

    void Exit()
    {
        if (isQR) 
        {
            ScanCancel(); 
        }
        else
        {
            ChangeState(State.Searching);
            MonsterManager.Inst.ClearMonster();
            menuManager.OpenMenu();
        }
    }

    public void SetTesetBtn()
    {
        testButton.gameObject.SetActive(true);
        testButton.onClick.AddListener(() => { SearchSuccess(); });
    }
}