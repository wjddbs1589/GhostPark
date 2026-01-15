using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//2025-06-17 OJY
//2025-06-30 OJY 수정
public class DocManager : MonoBehaviour
{
    static DocManager instance;
    public static DocManager Instance { get { return instance; } }

    GameManager gameManager;

    [Header("귀신 정보")]
    [SerializeField] GhostData ghostData;

    GhostInfo[] ghostInfos;
    int ghostCount;

    [Header("리스트")]
    [SerializeField] GameObject Obj_List;
    [SerializeField] GameObject Obj_Info;
    SetGhostList setList;
    SetGhostInfo setInfo;
    bool inInfo;

    [Header("UI")]
    [SerializeField] Button Btn_Exit;

    [Header("도감 버튼 관리용")]
    [SerializeField] GameObject obj_docBtn;

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

        gameManager = GameManager.Instance;

        setList = Obj_List.GetComponent<SetGhostList>();
        setInfo = Obj_Info.GetComponent<SetGhostInfo>(); 

        Btn_Exit.onClick.AddListener(Exit);

        InitSetting();
    }

    private void OnEnable()
    {
        obj_docBtn.SetActive(false);
    }
    private void OnDisable()
    {
        obj_docBtn.SetActive(true);
        Obj_Info.SetActive(false);
    }

    /// <summary>
    /// 초기 세팅
    /// </summary>
    void InitSetting()
    {
        //모든 귀신 정보 데이터 반환
        var infos = ghostData.GetAllInfos();

        //귀신 정보 리스트의 길이 합 계산
        ghostCount = 0;
        foreach (var info in infos)
        {
            ghostCount += info.Length; //7 + 2 + 1 = 10
        }
        //배열 초기화
        ghostInfos = new GhostInfo[ghostCount];

        DataRenew();
        GetGhostFoundInfo();
    }

    /// <summary>
    /// 첫 실행시 고스트 발견 여부 불러오기
    /// </summary>
    void GetGhostFoundInfo()
    {
        for (int i = 0; i < ghostCount; i++)
        {
            int index = i;

            string key = $"isFound_{index}";
            ghostInfos[i].isFound = (PlayerPrefs.GetInt(key, 0) == 1); 
        }
    }

    #region 데이터 관리
    /// <summary>
    /// 도감에 표시하기 위해 귀신 정보 갱신
    /// </summary>
    void DataRenew()
    {
        var infos = ghostData.GetAllInfos(); //저장된 데이터 불러오기

        int arrIndex = 0;
        foreach (var data in infos)
        {
            DataMerge(data, arrIndex);
            arrIndex += data.Length; //저장위치를 맞추기 위해 인덱스 변경
        }
    }    

    /// <summary>
    /// 통합 배열에 귀신 정보 병합
    /// </summary>
    /// <param name="datas">등급에 따라 나뉘어진 귀신 리스트</param>
    /// <param name="arrIndex">초기에 저장 위치 인덱스 값</param>
    void DataMerge(GhostInfo[] datas, int arrIndex)
    {
        for (int i = 0; i < datas.Length; i++)
        {
            ghostInfos[arrIndex + i] = datas[i];
        }
    }

    /// <summary>
    /// 귀신정보 반환
    /// </summary>
    /// <returns></returns>
    public GhostInfo[] GetGhostInfos()
    {
        DataRenew();

        if (ghostInfos.Length <= 0) return null;
        return ghostInfos;
    }

    /// <summary>
    /// 모든 정보 반환
    /// </summary>
    /// <returns></returns>
    public GhostData GetAllData()
    {
        return ghostData;
    }
    #endregion

    #region 창 이동    
    /// <summary>
    /// 도감에서 버튼을 눌렀을때, 상세정보 창 오픈
    /// </summary>
    public void OpenInfo(int index)
    {
        inInfo = true;

        SoundManager.Instance.PlaySFX(SfxType.btn);
        Obj_List.SetActive(false);
        Obj_Info.SetActive(true); 
        setInfo.SetInfoIndex(index);
    }

    /// <summary>
    /// 상세정보 창에서 도감으로 돌아갈때 
    /// </summary>
    public void OpenList()
    {
        inInfo = false;

        SoundManager.Instance.PlaySFX(SfxType.btn);
        Obj_Info.SetActive(false);
        Obj_List.SetActive(true); //오브젝트 활성화
    }
    #endregion

    /// <summary>
    /// 도감 나가기
    /// </summary>
    public void CloseDoc()
    {
        MenuManager.Instance.OpenMenu(); 
    }

    /// <summary>
    /// 뒤로가기 버튼 기능 분할
    /// </summary>
    void Exit()
    {
        if (inInfo) //세부 정보 창 일때
        {
            OpenList();
        }
        else //도감 일떼
        {
            CloseDoc();
        }
    }
}