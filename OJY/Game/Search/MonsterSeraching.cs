using System.Collections;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 작성 : KKH
// 생성일 : 2025-07-10
public class MonsterSeraching : MonoBehaviour
{
    public PedometerManager pedometerManager;
    public Image serachingImage; // 탐색 이미지

    public string strSerachinh = "혼령 탐색중..."; // 탐색 중 텍스트
    public TMP_Text serachinhTxt; // 탐색 중 텍스트
    [Range(0.1f, 1.0f)] public float delay = 0.1f;

    public bool isSerching = false; // 탐색 중인지 여부

    private void Awake()
    {
        serachingImage.color = new Color(1f, 1f, 1f, 0f); // 초기 알파값을 0으로 설정
    }

    private void OnEnable()
    {
        StartCoroutine(TypeText());
    }

    private void Update()
    {
        if (!isSerching)
        {
            float alpa = pedometerManager.Distance / 100f; // 거리 100m 기준으로 알파값 계산
            alpa = Mathf.Clamp(alpa, 0f, 1f); // 알파값을 0과 1 사이로 제한
            serachingImage.color = new Color(1f, 1f, 1f, alpa); // 이미지의 알파값 설정
            if (alpa >= 1.0f)
            {
                isSerching = true;
            }
        }
    }

    private IEnumerator TypeText()
    {
        int index = 0;
        serachinhTxt.text = ""; // 초기화

        while (true)
        {
            if (isSerching) // 탐색 중이면 텍스트를 비우고 종료
            {
                serachinhTxt.text = "";
                yield break;
            }

            if (index >= strSerachinh.Length) // 텍스트 끝에 도달하면
            {
                index = 0; // 인덱스 초기화
                serachinhTxt.text = ""; // 텍스트 비우기
            }

            serachinhTxt.text += strSerachinh[index]; // 텍스트 추가
            index++;

            yield return new WaitForSeconds(delay);
        }
    }

    public void ResetSeraching()
    {
        PedometerManager.ResetStepCount(); // 걸음 수 초기화
        isSerching = false; // 탐색 상태 초기화
        serachingImage.color = new Color(1f, 1f, 1f, 0f); // 이미지의 알파값을 0으로 설정
    }
}
