using UnityEngine;
using TMPro;
using UnityEngine.UI;

//2025-06-17 OJY
//2025-06-30 OJY 수정
//2025-07-03 OJY 수정
public class SetGhostInfo : MonoBehaviour
{
    DocManager docManager;
    GhostData ghostData;
    GhostInfo[] ghostInfos;
    int ArrLength;

    [Header("이미지")]
    [SerializeField] Image BackgroundImage;
    [SerializeField] Image RareImage;
    [SerializeField] Image GhostImage;
    [SerializeField] Image infoImage;

    [Header("버튼")]
    [SerializeField] Button Btn_Left;
    [SerializeField] Button Btn_Right;
    [SerializeField] Button Btn_Exit;

    int dataIndex;

    private void Awake()
    {
        //scriptableobj 데이터 수집
        docManager = FindAnyObjectByType<DocManager>();

        ghostInfos = docManager.GetGhostInfos();
        ArrLength = ghostInfos.Length;

        ghostData = docManager.GetAllData();

        //버튼 바인딩
        Btn_Left.onClick.AddListener(MoveToPrev);
        Btn_Right.onClick.AddListener(MoveToNext);
        Btn_Exit.onClick.AddListener(Exit);
    }

    #region 페이지 정보 세팅
    /// <summary>
    /// 리스트에서 누른 버튼의 인덱스 가져옴
    /// </summary>
    /// <param name="index"></param>
    public void SetInfoIndex(int index)
    {
        dataIndex = index;
        CreatePage(dataIndex);
    }

    /// <summary>
    /// 인덱스 맞춘 정보를 가져와서 데이터 세팅
    /// </summary>
    /// <param name="index"></param>
    void CreatePage(int index)
    {
        PageBtnSet();
        PageSetting(ghostInfos[index]);
    }

    /// <summary>
    /// 이 클래스를 가진 프리펩이 생성될 때 실행.
    /// 받은 데이터를 토대로 UI 세팅
    /// </summary>
    /// <param name="info"></param>
    void PageSetting(GhostInfo info)
    {
        //혼령정보(이름, 설명) 세팅
        infoImage.sprite = info.infoSprite;

        if (info.isFound)
        {
            //배경 이미지 세팅
            BackgroundImage.sprite = ghostData.Background;

            //혼령 이미지 세팅
            GhostImage.sprite = info.ghostImage;

            //등급 이미지 세팅
            RareImage.enabled = true;
            RareImage.sprite = info.rareSprite;
        }
        else
        {
            BackgroundImage.sprite = ghostData.Background_Unknown;

            GhostImage.sprite = info.UnknownImage;

            RareImage.enabled = false;
        }
    }
    #endregion

    #region 페이지 변경
    void MoveToPrev()
    {
        dataIndex--;
        PageChange(dataIndex);
    }
    void MoveToNext()
    {
        dataIndex++;
        PageChange(dataIndex);
    }

    /// <summary>
    /// 버튼을 눌러서 페이지 변경 시작
    /// </summary>
    /// <param name="num"></param>
    void PageChange(int num)
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);
        dataIndex = Mathf.Clamp(dataIndex, 0, (ArrLength - 1));
        PageBtnSet();
        CreatePage(dataIndex);
    }

    /// <summary>
    /// 양 끝 페이지 일때 넘기기 버튼 비활성화
    /// </summary>
    void PageBtnSet()
    {
        Btn_Left.gameObject.SetActive(dataIndex > 0);              //좌버튼은 첫 페이지가 아닐때 활성화
        Btn_Right.gameObject.SetActive(dataIndex < ArrLength - 1); //우버튼은 마지막 페이지가 아닐때 활성화
    }
    #endregion

    /// <summary>
    /// 도감으로 돌아가기
    /// </summary>
    public void Exit()
    {
        docManager.OpenList();
    }
}
