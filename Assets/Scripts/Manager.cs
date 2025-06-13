using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public const string IS_TRAKING = "isTracking";
    public static Manager Instance;

    [Header("3D ¸ðµ¨")]
    [SerializeField] private GameObject Honeybot;
    private Animator honeybotAnimator;
    [SerializeField] private GameObject StageBackground;
    private Animator stageBackgroundAnimator;
    [SerializeField] private GameObject Cornerbee;
    private Animator cornerbeeAnimator;
    [SerializeField] private GameObject ObjectDetection;

    [Header("UI")]
    [SerializeField] private GameObject CamList;
    [SerializeField] private GameObject Content;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        if(Instance != this) Instance = this;

        WebCamDevice[] camArr = WebCamShader.getCamList();
        AddCameList(camArr);
    }

    private void Start()
    {
        honeybotAnimator = Honeybot?.GetComponent<Animator>();
        stageBackgroundAnimator = StageBackground?.GetComponent<Animator>();
        cornerbeeAnimator = Cornerbee?.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showStage()
    {
        //Debug.Log("Show");
        honeybotAnimator.SetBool(IS_TRAKING, true);
        stageBackgroundAnimator.SetBool(IS_TRAKING, true);
        UIManager.instance.ShowButton();
    }

    public void hideStage()
    {
        //Debug.Log("Hide");
        honeybotAnimator.SetBool(IS_TRAKING, false);
        stageBackgroundAnimator.SetBool(IS_TRAKING, false);
        cornerbeeAnimator.SetBool(IS_TRAKING, false);
        UIManager.instance.HideButton();
    }

    public void callConerbeeEnter()
    {
        cornerbeeAnimator.SetBool(IS_TRAKING, true);
    }

    public void AddCameList(WebCamDevice[] devices)
    {
        Transform viewport = CamList.transform.Find("Viewport").Find("Content");
        float yPos = -150f;
        for (int i = 0; i < devices.Length; i++) {
            GameObject content = Instantiate(Content,viewport);
            content.GetComponent<RectTransform>().transform.localPosition = new Vector3(0,yPos,0);
            yPos -= 280f;

            content.name = i + "";
            content.GetComponentInChildren<TMP_Text>().text = devices[i].name;
            content.GetComponent<Button>().onClick.AddListener(() => { 
                bool result = WebCamShader.setCam(content.name); 
                if(result)
                    CamList.gameObject.SetActive(false);
                    ObjectDetection.SetActive(true);
            });
        }
    }

}
