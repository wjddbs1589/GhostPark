using UnityEngine;
using UnityEngine.EventSystems;

//2025-06-24 OJY
//2025-07-17 OJY 수정
public class CatchGhost : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool isPressed;        //눌림 여부

    float pressTime = 0;   //누른 시간
    float rate;            //누르기 진행 비율
    float needTime = 3.0f; //필요한 시간

    GameController controller;

    [Header("호리병")]
    [SerializeField] GameObject bottle;
    Animator anim;

    private void Awake()
    {
        controller = GameController.Instance;

        bottle.SetActive(true);
        anim = bottle.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isPressed)
        {
            pressTime += Time.deltaTime;  

            //완료시 처리
            if (pressTime >= needTime)
            {
                isPressed = false; 
                pressTime = 0;
                CatchEnd();
            }
        }

        rate = pressTime / needTime;
    }

    public void Init()
    {
        anim.SetTrigger("Start");
    }

    //버튼을 누를 때 실행
    //상태 변경
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        anim.SetBool("bPressed", true);
    }

    /// <summary>
    /// 버튼을 뗄 때 실행
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        pressTime = 0;
        anim.SetBool("bPressed", false);
    }

    /// <summary>
    /// 포획 완료후 이전 창으로 복귀
    /// </summary>
    void CatchEnd()
    {
        anim.SetTrigger("finish");
        controller.CatchSuccess();
    }
}