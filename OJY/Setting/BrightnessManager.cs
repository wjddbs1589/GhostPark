using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    [Header("세팅 오브젝트")]
    [SerializeField] GameObject settingObj;

    [Header("슬라이더")]
    [SerializeField] Slider Slider_Brightness;

    [Header("밝기 조절용 패널")]
    [SerializeField] Image BrightPanel;
    Color BrightPanelColor;

    [Header("버튼")]
    [SerializeField] Button Btn_X;

    float brightness;

    private void Awake()
    {
        Btn_X.onClick.AddListener(ClosePopup);

        BrightPanelColor = BrightPanel.color;

        brightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        Slider_Brightness.value = 1.0f - (brightness / 0.9f);  //역변환
    }

    private void Update()
    {
        brightness = Mathf.Lerp(0.9f, 0.0f, Slider_Brightness.value);  //슬라이더 0 ~ 1 => 알파값 0.9f ~ 0.0f
        SetAlpha(brightness);
    }

    void SetAlpha(float bright)
    {
        BrightPanelColor.a = bright;
        BrightPanel.color = BrightPanelColor;
    }

    /// <summary>
    /// 세팅창 닫기
    /// </summary>
    void ClosePopup()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);

        PlayerPrefs.SetFloat("Brightness", brightness);
        PlayerPrefs.Save();

        settingObj.SetActive(false);
    }
}
