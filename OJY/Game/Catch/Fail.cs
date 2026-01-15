using UnityEngine;
using UnityEngine.UI;

public class Fail : MonoBehaviour
{
    GameController controller;
    Button Btn_Close;

    private void Awake()
    {
        controller = GameController.Instance;
        Btn_Close = GetComponentInChildren<Button>();

        Btn_Close.onClick.AddListener(ClosePopup);
    }

    void ClosePopup()
    {
        controller.ReturnToSearching();
    }
}
