using UnityEngine;
using UnityEngine.UI;

public class CollectionCheck : MonoBehaviour
{
    int foundCount;
    Button btn_bottle;

    DocManager docManager;
    GhostInfo[] datas;    
    GhostData ghostData;

    Image UpgradeBottle;

    [Header("드라큘라 팝업")]
    public GameObject popup_dracula;

    private void Awake()
    {
        docManager = DocManager.Instance;

        btn_bottle = GetComponent<Button>();    
        btn_bottle.onClick.AddListener(StartPopup);
        UpgradeBottle = transform.GetChild(0).GetComponent<Image>();
    }

    private void OnEnable()
    {
        CountCheck();
    }

    void CountCheck()
    {
        datas = docManager.GetGhostInfos();

        foundCount = 0;
        foreach (GhostInfo info in datas)
        {
            if (info.isFound) foundCount += 1;
        }

        //9종을 모았을 때만 버튼 활성화
        if (foundCount != 9)
        {
            btn_bottle.interactable = false;
        }

        if (foundCount == 9) 
        {
            SoundManager.Instance.PlaySFX(SfxType.bottleReady); //드라큘라를 잡을 준비가 되었을때 소리 재생
            btn_bottle.interactable = true; 
        }

        //9종을 모았으면 이미지 강화
        if (foundCount >= 9) 
        {
            UpgradeBottle.enabled = true;
        } 
        else UpgradeBottle.enabled = false;
    }

    void StartPopup()
    {
        if (foundCount == 9)
        {
            popup_dracula.SetActive(true);
        }
    }
}
