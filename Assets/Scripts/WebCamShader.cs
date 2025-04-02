using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamShader : MonoBehaviour
{
    public static WebCamTexture webCamTexture;

    void Awake()
    {
        if (webCamTexture == null)
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length > 0)
            {
                webCamTexture = new WebCamTexture(devices[0].name);
                webCamTexture.Play();
            }
            else
            {
                Debug.LogError("No camera devices found.");
            }
        }
    }
}
