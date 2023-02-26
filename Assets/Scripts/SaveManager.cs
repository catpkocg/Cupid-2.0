using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wayway.Engine.Singleton;

public class SaveManager : MonoSingleton<SaveManager>
{
    public PlayerData DefaultPlayerData;
    public PlayerData PlayerData;
    // Start is called before the first frame update

    private void Awake()
    {
        Load();
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        Save();
    }
    private void OnApplicationQuit()
    {
        Save();
    }
    
    private void Save()
    {
        ES3.Save("PlayerData", PlayerData);
        PlayerData = null;
    }

    private void Load()
    {
        var loaded = ES3.Load<PlayerData>("PlayerData");
        if (loaded != null)
        {
            PlayerData = loaded;    
        }
        else
        {
            PlayerData = DefaultPlayerData;
        }
        Debug.Log(PlayerData.clearedMaxStage);
    }
}
