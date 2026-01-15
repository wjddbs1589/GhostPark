using UnityEngine;

//2025-06-24 OJY
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    int hasCoin = 0;
    int targetPrice = 0;

    public bool isTesterMode = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif
    }


    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
#endif
    }

    public bool TesterModeOn(string pw)
    {
        if(pw.Equals("yoyo"))
        {
            isTesterMode = true;
            Debug.Log("Tester mode activated!");

            GameController.Instance.SetTesetBtn();
            return true;
        }
        else
        {
            Debug.Log("Incorrect password for tester mode.");
            return false;
        }
    }

}
