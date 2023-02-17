using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToStage : MonoBehaviour
{
    
    public void LoadStageScene()
    {
        SceneManager.LoadScene(0);
    }
}
