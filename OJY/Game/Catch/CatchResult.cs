using UnityEngine;
using UnityEngine.UI;

public class CatchResult : MonoBehaviour
{
    [Header("이미지")]
    [SerializeField] Image RareImage;
    [SerializeField] Image GhostImage;
    [SerializeField] Image InfoImage;

    [Header("버튼")]
    [SerializeField] Button Btn_Document;
    [SerializeField] Button Btn_Continue;

    GameController gameController;

    void Awake()
    {
        gameController = GameController.Instance;

        Btn_Document.onClick.AddListener(GoToDocument); 
        Btn_Continue.onClick.AddListener(KeepGoing);    
    }

    public void SetResult(GhostInfo info)
    {
        SoundManager.Instance.PlaySFX(SfxType.collectSuccess);
        RareImage.sprite = info.rareSprite;
        GhostImage.sprite = info.ghostImage;
        InfoImage.sprite = info.infoSprite;
    }

    void GoToDocument()
    {
        if (!gameController) return;
        gameController.OpenDocument();
        gameObject.SetActive(false);
    }

    void KeepGoing()
    {
        if (!gameController) return;
        gameController.ReturnToSearching();
        gameObject.SetActive(false);
    }
}

