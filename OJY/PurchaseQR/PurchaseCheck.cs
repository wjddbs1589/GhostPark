using UnityEngine;
using UnityEngine.UI;

//2025-07-28 OJY
public class PurchaseCheck : MonoBehaviour
{
    [Header("본게임 오브젝트")]
    [SerializeField] GameObject MainGame;

    [Header("안내")]
    [SerializeField] GameObject main;
    [SerializeField] Button btn_start;

    [Header("스캔")]
    [SerializeField] GameObject scan;
    [SerializeField] Button btn_back;
    [SerializeField] GameObject Notion;
    [SerializeField] Button btn_Notion;
    PurchaseInfo data;

    [Header("안내")]
    [SerializeField] GameObject guide;
    [SerializeField] Button btn_guide;
    bool bPurchase;

    private void Awake()
    {
        //QR을 찍은 내역이 있으면 게임 시작
        if (PlayerPrefs.GetInt("bPurchase", 0) == 1)
        {
            GameStart();
        }

        btn_start.onClick.AddListener(StartBtn);
        btn_back.onClick.AddListener(BackBtn);
        btn_Notion.onClick.AddListener(NotionBtnClick);
        btn_guide.onClick.AddListener(GuideBtnClick);
    }

    /// <summary>
    /// 스캔 활성화
    /// </summary>
    void StartBtn()
    {
        scan.SetActive(true);
        main.SetActive(false);
        CameraOnOff();
    }

    /// <summary>
    /// 스캔 취소 및 돌아가기 
    /// </summary>
    void BackBtn()
    {
        main.SetActive(true);
        scan.SetActive(false);
        CameraOnOff();
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    void GameStart()
    {
        PlayerPrefs.SetInt("bPurchase", 1);
        PlayerPrefs.Save();

        MainGame.SetActive(true);
        CameraOnOff();
        gameObject.SetActive(false);
    }

    // <summary>
    /// QR스캔 버튼
    /// </summary>
    public void CameraOnOff()
    {
        if (!ARManager.Instance) return;
        if (!ARManager.Instance.IsAR) ARManager.Instance.OnAR(); //AR 활성화
        else ARManager.Instance.OffAR(); //AR 활성화
    }

    /// <summary>
    /// QR코드 정보를 받았을때
    /// </summary>
    public void UseQRcodeData(PurchaseInfo purchaseInfo)
    {
        data = purchaseInfo;
        if (data.bPurchase == 1)
        {
            Notion.SetActive(true);
        }
    }

    public void NotionBtnClick()
    {
        if (data.bPurchase == 1)
        {
            GameStart();
        }
    }

    public void GuideBtnClick()
    {
        main.SetActive(true);
        guide.SetActive(false);
    }
}
