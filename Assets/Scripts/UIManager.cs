using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("¹öÆ°")]
    [SerializeField] private GameObject[] rightButton;
    [SerializeField] private GameObject[] leftButton;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float hideX = 127f;
    [SerializeField] private float showX = 57f;

    public static UIManager instance;
    private bool isShow = false;
    
    private void Awake()
    {
        if(instance == null) instance = this;
        if (instance != this) instance = this;
    }

    private void Update()
    {
        if (isShow)
        {
            foreach(GameObject btn in rightButton)
            {
                Vector3 currPos = btn.transform.position;
                btn.transform.position = Vector3.MoveTowards(currPos, new Vector3(showX, currPos.y, currPos.z),speed*Time.deltaTime);
            }
        }
        else
        {
            foreach (GameObject btn in rightButton)
            {
                Vector3 currPos = btn.transform.position;
                btn.transform.position = Vector3.MoveTowards(currPos, new Vector3(hideX, currPos.y, currPos.z), speed * Time.deltaTime);
            }
        }
    }

    public void ShowButton()
    {
        isShow = true;
    }

    public void HideButton()
    {
        isShow = false;
    }
}
