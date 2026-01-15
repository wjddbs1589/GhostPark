using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

public class CatchTimer : MonoBehaviour
{
    TextMeshProUGUI timerText;
    GameController gameController;

    float time = 30.0f;
    float currentTime;

    bool bStartTimer;
    bool bCountdown;
    bool bfailSound;
    private void Awake()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();
        gameController = GameController.Instance;
    }

    private void OnEnable()
    {
        currentTime = time;
        bStartTimer = true;
        bCountdown = false;
        bfailSound = false;

        StartTimer();
    }

    private void FixedUpdate()
    {
        if (bStartTimer && currentTime >= 0.0f)
        {
            currentTime -= Time.deltaTime;

            if (currentTime > 5.0f)
            {
                timerText.color = Color.white;
            }
            else
            {
                if (!bCountdown) Countdown();

                timerText.color = Color.red;
            }

            if (currentTime <= 0)
            {
                currentTime = 0.0f;
                gameController.TimeOver();

                if(!bfailSound) Fail();
            }

            timerText.text = currentTime.ToString("F2");
        }
    }

    void Countdown()
    {
        bCountdown = true;
        SoundManager.Instance.PlaySFX(SfxType.countdown);
    }

    void Fail()
    {
        bfailSound = true;
        SoundManager.Instance.PlaySFX(SfxType.collectFailure);
    }

    public void StartTimer()
    {
        bStartTimer = true;
    }

    public void StopTimer() 
    {
        bStartTimer = false;
    }
}