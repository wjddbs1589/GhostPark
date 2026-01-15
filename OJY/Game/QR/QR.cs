using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;

public class JsonLoader
{
    public static VampireInfo VampireDataLoad(string jsonText)
    {
        VampireInfo data = JsonUtility.FromJson<VampireInfo>(jsonText);
        return data;
    }

    public static PurchaseInfo PurchaseInfoLoad(string jsonText)
    {
        PurchaseInfo data = JsonUtility.FromJson<PurchaseInfo>(jsonText);
        return data;
    }
}

public class VampireInfo
{
    public bool FindLegendary;
}

public class PurchaseInfo
{
    public int bPurchase;
}

public class QR : MonoBehaviour
{
    [SerializeField] ARCameraManager cameraManager;

    [Header("뱀파이어")]
    [SerializeField] Button btn_Capture;

    [Header("구매인증")]
    [SerializeField] Button btn_purchaseQR;

    Texture2D cameraTexture;
    GameController gameController;

    void Awake()
    {
        btn_Capture.onClick.AddListener(delegate { Capture(); });
        btn_purchaseQR.onClick.AddListener(delegate { Capture(); });
    }

    void Capture()
    {
        SoundManager.Instance.PlaySFX(SfxType.btn);

        if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            // 이미지 데이터를 ZXing으로 넘길 준비
            // image.ConvertToTexture2D() 비슷한 처리 필요
            StartCoroutine(ProcessImage(image));
        }
    }

    IEnumerator ProcessImage(XRCpuImage image)
    {
        // 변환 파라미터 설정 (ARGB32로)
        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = new Vector2Int(image.width, image.height),
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.MirrorY // AR 카메라는 좌우 반전됨
        };

        // 버퍼 생성
        int size = image.GetConvertedDataSize(conversionParams);
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        // 변환 실행
        image.Convert(conversionParams, buffer);
        image.Dispose();

        // Texture2D 생성 또는 재사용
        if (cameraTexture == null || cameraTexture.width != image.width || cameraTexture.height != image.height)
        {
            cameraTexture = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
        }

        cameraTexture.LoadRawTextureData(buffer);
        cameraTexture.Apply();

        // 이제 cameraTexture를 ZXing에 넘길 수 있음
        Debug.Log("Texture 변환 완료!");

        buffer.Dispose();

        QRCheck(cameraTexture);

        yield return null;
    }

    void QRCheck(Texture2D texture2D)
    {
        IBarcodeReader reader = new BarcodeReader();

        //뱀파이어 탐색 QR
        var result = reader.Decode(texture2D.GetPixels32(), texture2D.width, texture2D.height);
        if(result != null)
        {
            VampireInfo data = JsonLoader.VampireDataLoad(result.Text);
            if(data != null)
            {
                gameController = FindAnyObjectByType<GameController>();
                if (gameController) 
                {
                    gameController.UseQRcodeData(data);
                }
            }

            //구매확인 QR
            PurchaseInfo info = JsonLoader.PurchaseInfoLoad(result.Text);
            if (info != null)
            {
                FindAnyObjectByType<PurchaseCheck>().UseQRcodeData(info);
            }
        }
    }
}
