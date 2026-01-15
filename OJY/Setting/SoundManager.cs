using UnityEngine;
using UnityEngine.UI;
public enum BgmType
{
    normal, //평시
    collect //수집모드시
}

public enum SfxType
{
    btn,            //버튼 클릭
    bottleReady,    //도감 호리병 준비됨
    countdown,      //수집 카운트다운
    collectSuccess, //수집 성공
    collectFailure, //수집 실패
    tutorial,       //튜토리얼 페이지 변경
    popup,          //팝업
    bottleLid,      //호리 뚜껑 닫힐 때
    ghostSpawn      //유령 소환
}

public class SoundManager : MonoBehaviour
{
    static SoundManager instance;

    public static SoundManager Instance
    {
        get { return instance; }
    }

    AudioSource bgmSource;
    private float bgmVolume = 0.5f;

    AudioSource sfxSource;  
    private float sfxVolume = 0.5f;

    [Header("배경음")]
    [SerializeField] AudioClip bgm_normal;
    [SerializeField] AudioClip bgm_collect;

    [Header("준비된 호리병 소리")]
    [SerializeField] AudioClip sfx_bottleReady;

    [Header("버튼음")]
    [SerializeField] AudioClip sfx_btn;

    [Header("수집")]
    [SerializeField] AudioClip sfx_collectFailure;
    [SerializeField] AudioClip sfx_collectSuccess;
    [SerializeField] AudioClip sfx_countdown;

    [Header("귀신 생성음")]
    [SerializeField] AudioClip sfx_ghostSpawn;

    [Header("뚜껑 소리")]
    [SerializeField] AudioClip sfx_bottleLid;

    [Header("팝업 열리는 소리")]
    [SerializeField] AudioClip sfx_popup;
    
    [Header("튜토리얼 단계전환음")]
    [SerializeField] AudioClip sfx_tutorial;

    [Header("환경설정 슬라이더")]
    [SerializeField] Slider BgmSlider;
    [SerializeField] Slider SfxSlider;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //배경음 세팅
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;          //반복재생
        bgmSource.playOnAwake = false;  //시작시 재생
        BgmSlider.value = PlayerPrefs.GetFloat("BgmVolume", 0.5f);

        //효과음 세팅
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        SfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0.5f);

        PlayBGM(bgm_normal);
    }

    private void Update()
    {
        bgmVolume = BgmSlider.value;
        SetBgmVolume(bgmVolume);

        sfxVolume = SfxSlider.value;
        SetSfxVolume(sfxVolume);
    }

    #region 소리 조절
    /// <summary>
    /// 배경음 볼륨 조정
    /// </summary>
    /// <param name="volume">볼륨</param>
    public void SetBgmVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
            bgmSource.volume = bgmVolume;

        PlayerPrefs.SetFloat("BgmVolume",bgmVolume);
    }

    /// <summary>
    /// 효과음의 볼륨 조정
    /// </summary>
    /// <param name="volume">볼륨</param>
    public void SetSfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (!sfxSource) sfxSource.volume = sfxVolume;

        PlayerPrefs.SetFloat("SfxVolume", sfxVolume);
    }
    #endregion

    #region 배경음 재생
    /// <summary>
    /// 기본 배경음 재생
    /// </summary>
    public void PlayNormalBGM()
    {
        PlayBGM(bgm_normal);
    }

    /// <summary>
    /// 수집 배경음 재생
    /// </summary>
    public void PlayCollectBGM()
    {
        PlayBGM(bgm_collect);
    }
    #endregion


    #region 오디오 재생

    /// <summary>
    /// 배경음 소스 재생
    /// </summary>
    /// <param name="bgm"></param>
    void PlayBGM(AudioClip bgm)
    {
        if (!bgm) return;
        bgmSource.clip = bgm;
        bgmSource.volume = bgmVolume;
        bgmSource.Play(); 
    }

    /// <summary>
    /// 배경음 재생
    /// </summary>
    /// <param name="type">배경음 타입</param>
    public void PlayBGM(BgmType type)
    {
        switch (type)
        {
            case BgmType.normal:
                PlayBGM(bgm_normal);
                break;
            case BgmType.collect:
                PlayBGM(bgm_collect);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 효과음 소스 재생
    /// </summary>
    /// <param name="sfx">재생할 클립</param>
    void PlaySFX(AudioClip sfx)
    {
        if (!sfx) return;
        sfxSource.PlayOneShot(sfx, sfxVolume); //clip => 재생할 클립, sfxVolume => 볼륨 배수
    }

    /// <summary>
    /// 효과음 재생
    /// </summary>
    /// <param name="type">효과음 타입</param>
    public void PlaySFX(SfxType type)
    {
        //Debug.Log($"효과음 타입 : {type}");
        switch (type)
        {
            case SfxType.btn:
                PlaySFX(sfx_btn);
                break;

            case SfxType.bottleReady:
                PlaySFX(sfx_bottleReady);
                break;

            case SfxType.countdown:
                PlaySFX(sfx_countdown);
                break;

            case SfxType.collectSuccess:
                PlaySFX(sfx_collectSuccess);
                break;

            case SfxType.collectFailure:
                PlaySFX(sfx_collectFailure);
                break;

            case SfxType.tutorial:
                PlaySFX(sfx_tutorial);
                break;

            case SfxType.popup:
                PlaySFX(sfx_popup);
                break;

            case SfxType.bottleLid: //보류(UI없음)                
                PlaySFX(sfx_bottleLid);
                break;

            case SfxType.ghostSpawn:
                PlaySFX(sfx_ghostSpawn);
                break;

            default:
                break;
        }
    }
    #endregion
}
