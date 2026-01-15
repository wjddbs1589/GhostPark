using UnityEngine;
using UnityEngine.UI;

//2025-06-27 OJY
[CreateAssetMenu(fileName = "Tutorial Data", menuName = "Scriptable Object/Tutorial Data", order = int.MaxValue)]
public class TutorialData : ScriptableObject
{
    public Sprite[] StepImages;
    public Sprite[] InfoImages;
    public string[] InfoTexts;
}