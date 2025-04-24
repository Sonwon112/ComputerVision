using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if(Instance != this) Instance = this;
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
}
