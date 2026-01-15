using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GhostInfoBtn : MonoBehaviour
{
    int index;

    [Header("유령 정보")] 
    [SerializeField] Image ghostImage;
    [SerializeField] TextMeshProUGUI nameText;

    /// <summary>
    /// 버튼에 보여질 이미지 및 이름 세팅
    /// </summary>
    /// <param name="image">유령 이미지</param>
    /// <param name="name">유령 이름</param>
    public void Setting(Sprite image, string name)
    {
        ghostImage.sprite = image;
        nameText.text = name;
    }
}
