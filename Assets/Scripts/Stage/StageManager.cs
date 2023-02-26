using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private List<GameObject> clearImage;

    private void Start()
    {
        CheckStageNumber();
    }

    private void CheckStageNumber()
    {
        for (int i = 0; i < playerData.clearedMaxStage; i++)
        {
            clearImage[i].SetActive(true);
        }
    }
    
}
