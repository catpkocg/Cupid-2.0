using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Cupid/Scene Functions")]
public class SceneFunctions : ScriptableObject
{
    [SerializeField] private string playSceneName;
    [SerializeField] private string settingSceneName;
    
    public void LoadPlayScene() => SceneManager.LoadScene(playSceneName);
    public void LoadSettingScene() => SceneManager.LoadScene(settingSceneName);
    public void ExitGame() => Application.Quit();
}
