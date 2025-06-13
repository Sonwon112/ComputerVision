using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamShader : MonoBehaviour
{
    public static WebCamTexture webCamTexture;
    private static WebCamDevice[] devices;
    public static WebCamDevice[] getCamList()
    {
        if (webCamTexture == null)
        {
            WebCamDevice[] tmp = WebCamTexture.devices;
            devices = tmp;
        }
        return devices;
    }

    public static bool setCam(string content)
    {
        //Debug.Log(content);
        if (devices.Length > 0)
        {
            webCamTexture = new WebCamTexture(devices[int.Parse(content)].name);
            webCamTexture.Play();
            return true;
        }
        else
        {
            Debug.LogError("No camera devices found.");
            return false;
        }
    }
}
