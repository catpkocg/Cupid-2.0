using System;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{ 
    private void Awake()
    {
        // SetRect();
    }

    private void Update()
    {
        // SetRect();
    }

    private void SetRect()
    {
        var playCamera = GetComponent<Camera>();
        var rect = playCamera.rect;
        var height = ((float)Screen.width / Screen.height) / ((float)9 / 16);
        var width = 1f / height;
        if (height < 1)
        {
            rect.height = height;
            rect.y = (1f - height) / 2f;
            
        }
        else
        {
            rect.width = width;
            rect.x = (1f - width) / 2f;
        }
        playCamera.rect = rect;
    }
}
