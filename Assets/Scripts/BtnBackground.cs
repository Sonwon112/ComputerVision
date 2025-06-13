using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnBackground : MonoBehaviour, BtnEvent
{
    [Header("¹è°æ")]
    [SerializeField] private Image background;
    [SerializeField] private Sprite[] backgroundSrc;
    [SerializeField] private float defaultTerm = 1f;

    private List<Color> colors = new List<Color>();
    private int idx = 0;

    private float term;
    private bool onCount = false;

    // Start is called before the first frame update
    void Start()
    {
        term = defaultTerm;
        for (int i = 0; i < backgroundSrc.Length; i++)
        {
            colors.Add(i == 0 ? new Color(1, 0.99f, 0.82f) : Color.white);
        }
    }

    void FixedUpdate()
    {
        if (onCount)
        {
            term -= 0.01f;
            Debug.Log("" + term);
            if (term <= 0f)
            {
                onCount = false;
                term = defaultTerm;
            }
        }
    }

    public void PlayEvent()
    {
        if (!onCount)
        {
            idx = idx + 1 >= backgroundSrc.Length ? 0 : idx + 1;
            UpdateBackground();
            onCount = true;
        }
    }

    private void UpdateBackground()
    {
        background.color = colors[idx];
        background.sprite = backgroundSrc[idx];
    }
}
