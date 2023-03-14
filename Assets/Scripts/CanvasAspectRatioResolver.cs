using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasAspectRatioResolver : MonoBehaviour
{
    private CanvasScaler canvasScaler;

    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
    }
    private void OnEnable()
    {
        AspectRatioManager.Instance.OnInitialRatio += Setup;
        AspectRatioManager.Instance.OnRatioNarrower += RatioNarrowerHandler;
        AspectRatioManager.Instance.OnRatioWider += RatioWiderHandler;
    }
    private void OnDisable()
    {
        if (AspectRatioManager.Instance != null)
        {
            AspectRatioManager.Instance.OnInitialRatio -= Setup;
            AspectRatioManager.Instance.OnRatioNarrower -= RatioNarrowerHandler;
            AspectRatioManager.Instance.OnRatioWider -= RatioWiderHandler;
        }
    }

    private void Setup()
    {
        
    }
    private void RatioNarrowerHandler()
    {
        canvasScaler.matchWidthOrHeight = 0;
    }
    private void RatioWiderHandler()
    {
        canvasScaler.matchWidthOrHeight = 1;
        Camera.main.orthographicSize = 8;
    }
}
