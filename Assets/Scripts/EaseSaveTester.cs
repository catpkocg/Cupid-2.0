using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaseSaveTester : MonoBehaviour
{
    public PlayerData playerData;
    // Start is called before the first frame update
    public void Save()
    {
        PlayerData saveData = ScriptableObject.CreateInstance<PlayerData>();
        playerData.clearedMaxStage = 2;
        ES3.Save("PlayerData", saveData);
    }

    public void Load()
    {
        PlayerData loadData = ScriptableObject.CreateInstance<PlayerData>();
        ES3.LoadInto("PlayerData", loadData);
        Debug.Log(playerData.clearedMaxStage);
        
    }
}
