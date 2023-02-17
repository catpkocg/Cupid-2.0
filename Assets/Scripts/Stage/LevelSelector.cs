using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private int stageNumber;
    public GameConfig gameConfig;

    private int StageNumber => stageNumber;

    public void LoadPlayScene()
    {
        gameConfig.StageLevel = StageNumber;
        Debug.Log(StageNumber);
        SceneManager.LoadScene(1);
        
    }
}
