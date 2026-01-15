using UnityEngine;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
    [SerializeField] Button closeBtn;
    
    private void Awake()
    {
        closeBtn.onClick.AddListener(() => { Destroy(gameObject); });
    }    
}
