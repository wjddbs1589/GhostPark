using UnityEngine;
using UnityEngine.UI;

public class CollectionPopup : MonoBehaviour
{
    [Header("¹öÆ°")]
    [SerializeField] Button Btn_Close;
    [SerializeField] Button Btn_QR;

    GameController gameController;

    private void Awake()
    {
        gameController = GameController.Instance;

        Btn_Close.onClick.AddListener(Close);
        Btn_QR.onClick.AddListener(QR);
    }

    void Close()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);
        gameObject.SetActive(false);
    }

    void QR()
    {
        MenuManager.Instance.StartGame();
        gameController.QRcodeScan();

        gameObject.SetActive(false);
    }
}
