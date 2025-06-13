using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.CompilerServices;

public class ObjectDetection : MonoBehaviour
{
    public NNModel modelAsset;
    public RawImage rawImage;
    public AspectRatioFitter aspectRatioFitter;
    public GameObject boundingBoxPrefab;  // �ٿ�� �ڽ��� �׸��� ���� Image ������
    public RectTransform boundingBoxContainer; // �ٿ�� �ڽ��� �׸� �θ� �����̳�
    public List<int> targetClasses = new List<int>();  // ���͸��� Ŭ���� �ε��� ����Ʈ
    public GameObject MainCamera;

    [Header("Output Target")]
    public GameObject posObject;
    public Cernerbee carnerbee;

    private IWorker worker;
    private Texture2D tempTexture;
    private Texture2D resizedTexture = null;
    private Texture2D finalTexture = null;

    // Confidence threshold
    private const float confidenceThreshold = 0.5f;
    // IoU threshold for NMS
    private const float iouThreshold = 0.5f;

    // ���� �ػ� ���� ����
    private int originalWidth;
    private int originalHeight;

    private List<GameObject> boundingBoxes = new List<GameObject>();

    float[][] keyPosition = new float[17][];

    void Start()
    {
        // WebCamTexture ����
        rawImage.texture = WebCamShader.webCamTexture;
        tempTexture = new Texture2D(WebCamShader.webCamTexture.width, WebCamShader.webCamTexture.height);

        // Barracuda �� �ε�
        var model = ModelLoader.Load(modelAsset);

        // GPU ��� ��Ŀ ���
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
    }

    // Update is called once per frame
    void Update()
    {
        if (WebCamShader.webCamTexture.didUpdateThisFrame)
        {
            Stopwatch preprocessStopwatch = new Stopwatch();
            Stopwatch inferenceStopwatch = new Stopwatch();
            Stopwatch postprocessStopwatch = new Stopwatch();


            // ��ķ �ؽ�ó�� �ټ��� ��ȯ
            preprocessStopwatch.Start();
            Tensor input = PreprocessImage(WebCamShader.webCamTexture);
            preprocessStopwatch.Stop();


            // �� ����
            inferenceStopwatch.Start();
            worker.Execute(input);
            // ��� ��������
            Tensor output = worker.PeekOutput();
            inferenceStopwatch.Stop();

            // ��� ó�� (��: ��ü ���� �ٿ�� �ڽ� �׸���)
            postprocessStopwatch.Start();
            ProcessOutput(output);
            postprocessStopwatch.Stop();

            //UnityEngine.Debug.Log(output);
            // ����� �α� ���
            //UnityEngine.Debug.Log($"Preprocess time: {preprocessStopwatch.ElapsedMilliseconds} ms, " +
            //                      $"Inference time: {inferenceStopwatch.ElapsedMilliseconds} ms, " +
            //                      $"Postprocess time: {postprocessStopwatch.ElapsedMilliseconds} ms");

            input.Dispose();
            output.Dispose();
        }
        // 1. 6, 7, 8 => ��
        // 2. 9, 10, 11 => �� ��
        // 3. 12, 13, 14 => �� ��
        // 4. 15, 16, 17 => �� ��
        // 5. 18, 19, 18 => �� ��
        // 6. 21. 22, 23 => �� ��
        // 7. 24. 25, 26 => �� ��
        // 8. 27, 28, 29 => �� �Ȳ�
        // 9. 30, 31, 32 => �� �Ȳ�
        // 10. 33, 34, 35 => �� ��
        // 11. 36, 37, 38 => �� ��
        // 12. 39, 40, 41 => �� ��
        // 13. 42, 43, 44 => �� ��
        // 14. 45, 46, 47 => �� ��
        // 15. 48, 49, 50 => �� ��
        // 16. 51, 52, 53 => �� ��
        // 17. 54, 55, 56 => �� ��

        void ProcessOutput(Tensor output)
        {
            
            // ���� �ٿ�� �ڽ� ����
            foreach (var box in boundingBoxes)
            {
                Destroy(box);
            }
            boundingBoxes.Clear();

            // Tensor �����͸� �迭�� ��ȯ
            float[] outputArray = output.ToReadOnlyArray();

            // �ټ��� ��ġ ũ��, ��Ŀ ��, ��� ������ ������
            int batch = output.shape.batch;   // 1
            int anchor = output.shape.width;  // 8400
            int outputDim = output.shape.channels;  // 56
            //UnityEngine.Debug.Log(output);


            List<BoundingBox> boxes = new List<BoundingBox>();
            float maxPercent = 0;
            int index = 0;


            // �� ���� ����Ʈ ��ǥ ����
            for (int a = 0; a < anchor; a++)
            {

                int offset = a * outputDim;
                float x = outputArray[offset + 0];
                float y = outputArray[offset + 1];
                float w = outputArray[offset + 2];
                float h = outputArray[offset + 3];
                float percent = outputArray[offset + 4];

                if (percent < 0.8) continue;
                if (percent < maxPercent) continue;
                for (int i = 5; i < 56; i += 3)
                {
                    float[] kPos = { outputArray[offset + i], outputArray[offset + i + 1], outputArray[offset + i + 2] };
                    //float kpercent = outputArray[offset + i + 2];
                    keyPosition[index] = kPos;
                    index++;
                    //UnityEngine.Debug.Log("Ʈ��ŷ : " + kx + ", " + ky + ", " + kpercent);
                }
                index = 0;


                if (x <120 || x > 570)
                {
                    //UnityEngine.Debug.Log("Ʈ��ŷ ���� �ƴ�");                    
                    carnerbee.setTraking(false);
                    Manager.Instance.hideStage();
                    
                    return;
                }
                else
                {
                    Manager.Instance.showStage();
                    carnerbee.setDefaultPelvis(new Vector3(Mathf.Ceil(keyPosition[13][0] * -1), Mathf.Ceil(keyPosition[13][1] * -1), 0), new Vector3(Mathf.Ceil(keyPosition[12][0] * -1), Mathf.Ceil(keyPosition[12][1] * -1), 0));
                    //UnityEngine.Debug.Log("Ʈ��ŷ ����");
                }
                //UnityEngine.Debug.Log("��� Ž�� : " + x + ", " + y + ", " + w + ", " + h + ", " + percent);

                
            }
            Vector3[] trakingCoordinate = new Vector3[17];
            for (int i = 0; i < trakingCoordinate.Length; i++)
            {
                trakingCoordinate[i] = new Vector3(-1, -1, -1);
            }

            for (int i = 0; i < keyPosition.Length; i++)
            {
                GameObject target = posObject.transform.Find(""+i).gameObject;
                if (target == null) return;
                target.SetActive(true);

                if (keyPosition[i] == null) continue;
                if (keyPosition[i][2] < 0.5f)
                {
                    target.SetActive(false);
                    trakingCoordinate[i] = new Vector3(-1, -1, -1);
                }
                else
                {
                    target.transform.position = new Vector3(Mathf.Ceil(keyPosition[i][0] * -1), Mathf.Ceil(keyPosition[i][1] * -1), 0);
                    trakingCoordinate[i] = new Vector3(Mathf.Ceil(keyPosition[i][0] * -1), Mathf.Ceil(keyPosition[i][1] * -1), 0);
                }
                //UnityEngine.Debug.Log("index : "+i+", x : " + keyPosition[i][0]+", y : " + keyPosition[i][1]);
            }
            //UnityEngine.Debug.Log(trakingCoordinate);
            //if(!carnerbee.isTraking()) carnerbee.setTraking(true);
            if (carnerbee.isTraking())
            {
                carnerbee.setTrakingCoordinate(trakingCoordinate);
            }
            
        }

    }

    void OnDestroy()
    {
        // �ڿ� ����
        worker.Dispose();
    }

    Tensor PreprocessImage(WebCamTexture texture)
    {
        int inputWidth = 640;
        int inputHeight = 640;
        int inputChannel = 3;

        // // ���� �ػ� ����
        originalWidth = texture.width;
        originalHeight = texture.height;

        // // �ؽ�ó���� ���� �����͸� �о�ͼ� ũ�⸦ �����ϰ� ����ȭ
        Color32[] pixels = texture.GetPixels32();
        tempTexture.SetPixels32(pixels);
        tempTexture.Apply();

        Texture2D processedTexture = LetterBox(tempTexture, inputWidth, inputHeight);

        // Texture2D�� Tensor�� ��ȯ
        Tensor inputTensor = new Tensor(processedTexture, inputChannel);
        return inputTensor;
    }

    Texture2D LetterBox(Texture2D source, int newWidth, int newHeight)
    {
        float aspectRatio = (float)source.width / source.height;
        int resizeWidth, resizeHeight;
        if (source.width > source.height)
        {
            resizeWidth = newWidth;
            resizeHeight = Mathf.RoundToInt(newWidth / aspectRatio);
        }
        else
        {
            resizeHeight = newHeight;
            resizeWidth = Mathf.RoundToInt(newHeight * aspectRatio);
        }

        RenderTexture rt = RenderTexture.GetTemporary(resizeWidth, resizeHeight);
        Graphics.Blit(source, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;
        if (resizedTexture == null)
        {
            resizedTexture = new Texture2D(resizeWidth, resizeHeight);
        }
        // Texture2D resizedTexture = new Texture2D(resizeWidth, resizeHeight);
        resizedTexture.ReadPixels(new Rect(0, 0, resizeWidth, resizeHeight), 0, 0);
        resizedTexture.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        if (finalTexture == null)
        {
            finalTexture = new Texture2D(newWidth, newHeight);
        }
        // Texture2D finalTexture = new Texture2D(newWidth, newHeight);
        Color32[] fillPixels = finalTexture.GetPixels32();
        for (int i = 0; i < fillPixels.Length; i++)
        {
            fillPixels[i] = new Color32(128, 128, 128, 255); // �е� ���� (ȸ��)
        }
        finalTexture.SetPixels32(fillPixels);
        finalTexture.Apply();

        int offsetX = (newWidth - resizeWidth) / 2;
        int offsetY = (newHeight - resizeHeight) / 2;
        Graphics.CopyTexture(resizedTexture, 0, 0, 0, 0, resizeWidth, resizeHeight, finalTexture, 0, 0, offsetX, offsetY);
        finalTexture.Apply();

        return finalTexture;
    }

}


public class BoundingBox
{
    public float x1, y1, x2, y2, score;
    public int classIndex;

    public BoundingBox(float x1, float y1, float x2, float y2, float score, int classIndex)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        this.score = score;
        this.classIndex = classIndex;
    }


}