using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NUI_Button : MonoBehaviour
{
    [SerializeField] private string buttonText;
    [SerializeField] private Color buttonColor;

    private TMP_Text txtButton;
    private Image imgButton;

    // Start is called before the first frame update
    void Start()
    {
        txtButton = transform.GetComponentInChildren<TMP_Text>();
        if(txtButton != null )
            txtButton.text = buttonText;
        imgButton = transform.GetComponentInChildren<Image>();
        if( imgButton != null )
            imgButton.color = buttonColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
