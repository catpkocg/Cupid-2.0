using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Wayway.Engine.Singleton;

public class AspectRatioManager : MonoSingleton<AspectRatioManager>
{
    [SerializeField] private float widthRatio;
    [SerializeField] private float heightRatio;
    [ShowInInspector, DisplayAsString] private float ratioConversionLimit => widthRatio / heightRatio;
    
    private float prevAspectRatio;
    public static float CurrAspectRatio => (float) Screen.width / Screen.height;
    
    public event Action OnInitialRatio;
    public event Action OnRatioWider;
    public event Action OnRatioNarrower;
    

    private void Start()
    {
        prevAspectRatio = CurrAspectRatio;
        OnInitialRatio?.Invoke();
        CheckRatioAction();
    }

    private void Update()
    {
        if (prevAspectRatio == CurrAspectRatio) return;
        
        prevAspectRatio = CurrAspectRatio;
        CheckRatioAction();
    }
    private void CheckRatioAction()
    {
        if (CurrAspectRatio <= ratioConversionLimit)
        {
            OnRatioNarrower?.Invoke();
        }
        else if (ratioConversionLimit < CurrAspectRatio)
        {
            OnRatioWider?.Invoke();
        }
    }
}
