using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFrame : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

}
