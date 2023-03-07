using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    public List<Image> conditionImages;
    public List<TextMeshProUGUI> conditionCount;
    public Slider scoreEnergy;
    public void SettingUI(Map map)
    {
        var conditionList = map.ClearConditionData;
        var playUI = GameManager.Instance.ui;
        var conditionImage = GameManager.Instance.conditionImage;
        for (var i = 0; i < conditionList.Count; i++)
        {
            playUI.conditionImages[i].GetComponent<Image>().sprite 
                = conditionImage.ImagesForUI[conditionList[i].ConditionBlock];
            playUI.conditionCount[i].GetComponent<TextMeshProUGUI>().text = conditionList[i].HowMuchForClear.ToString();
        }
    }
    
}
