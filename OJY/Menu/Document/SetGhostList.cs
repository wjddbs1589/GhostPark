using UnityEngine;
using UnityEngine.UI;

//2025-06-30 OJY
//2025-07-03 OJY 수정
public class SetGhostList : MonoBehaviour
{
    [Header("버튼 프리펩")]
    [SerializeField] GameObject BtnPrefab;

    [Header("스크롤뷰 부모 오브젝트")]
    [SerializeField] Transform Obj_Content;
    RectTransform rect;
    GameObject[] contents;

    //귀신 정보
    DocManager docManager;
    GhostInfo[] allDatas;

    void Awake()
    {
        //제일 위에 있는 DocManager가 가진 scriptableobj 데이터 수집
        docManager = FindAnyObjectByType<DocManager>();
        allDatas = docManager.GetGhostInfos();
        contents = new GameObject[allDatas.Length];
        rect = Obj_Content.GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        CreateBtns();

        rect.anchoredPosition = new Vector2(0f, 0f); //리스트 맨 위로 올리기
    }

    private void OnDisable()
    {
        foreach (GameObject obj in contents) 
        {
            Destroy(obj);
        }
    }

    /// <summary>
    /// 페이지에 버튼 생성
    /// </summary>
    public void CreateBtns()
    {
        for (int i = 0; i < allDatas.Length; i++)
        {
            int index = i;

            //버튼 스폰 및 부모 지정
            contents[i] = Instantiate(BtnPrefab, Obj_Content);
            GhostInfoBtn btnInfo = contents[i].GetComponent<GhostInfoBtn>();

            if (allDatas[i].isFound)
            {
                btnInfo.Setting(allDatas[i].ghostImage, allDatas[i].name);
            }
            else
            {
                btnInfo.Setting(allDatas[i].UnknownImage, allDatas[i].name);
            }

            //버튼 바인딩
            contents[i].GetComponent<Button>().onClick.AddListener(() => { docManager.OpenInfo(index); });
        }
    }
}