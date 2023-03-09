using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Wayway.Engine.Singleton;

public class SaveManager : MonoSingleton<SaveManager>
{
    public PlayerData DefaultPlayerData;
    public PlayerData PlayerData;
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        
        Load();

        Application.targetFrameRate = 120;
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        Save();
    }
    private void OnApplicationQuit()
    {
        Save();
    }

    [Button]
    public void DeleteSaveFileFromDevice()
    {
        ES3.DeleteKey("PlayerData");
    }
    
    private void Save()
    {
        ES3.Save("PlayerData", PlayerData);
        PlayerData = null;
    }

    private void Load()
    {
        PlayerData loaded;
        try
        {
            loaded = ES3.Load<PlayerData>("PlayerData");
            PlayerData = loaded;
            
            Debug.Log("Found PlayerData and Loaded");
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            
            PlayerData = DefaultPlayerData;

            Debug.Log("Did not Find PlayerData and Loaded Default PlayerData");
        }
    }
}
