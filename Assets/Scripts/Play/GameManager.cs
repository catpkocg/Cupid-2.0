using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wayway.Engine.Singleton;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Spawn spawn;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject failPopUp;
    [SerializeField] private GameObject clearPopUp;
    [SerializeField] private List<GameObject> starFill;
    [SerializeField] private TextMeshProUGUI moveNumber;
    
    //public List<ConditionStates> ConditionStatesList;
    public ConditionImage conditionImage;
    public SerializeDictionary<int,int> ConditionStates = new ();
    public GameConfig gameConfig;
    public Interaction interaction;
    public PlayUI ui;
    
    public int score;
    public int touchCount;
    
    public States State { get; set; }

    public bool scaleIsDone = false;
    
    private void Start()
    {
        SettingConditionStates();
        State = States.ReadyForInteraction;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //MapManager.Instance.DrawDirectionOnBlock();
        }
        
        switch (State)
        {
            case States.ReadyForInteraction:
                break;
            case States.CheckTarget:
                MapManager.Instance.map.MoveLimit--;
                moveNumber.text = MapManager.Instance.map.MoveLimit.ToString();
                MapManager.Instance.CheckTarget();
                State = States.CreateNewBlock;
                break;
            case States.CreateNewBlock:
                spawn.SpawnForEmptyPlace();
                MapManager.Instance.DeleteAllDraw();
                State = States.DownAllBlock;
                break;
            case States.DownAllBlock:
                MapManager.Instance.MoveAllBlock();
                State = States.Waiting;
                break;
            case States.Waiting:
                if (!MapManager.Instance.IsThereMovingPang())
                {
                    Debug.Log("바뀜");
                    State = States.DrawDirection;
                }
                break;
            case States.DrawDirection:
                MapManager.Instance.DrawDirectionOnBlock();
                State = States.CheckClearCondition;
                break;
            case States.CheckClearCondition:
                
                //Check Score;
                
                var mapScore = (float)MapManager.Instance.map.PerfectScore;
                var percentScore = score / mapScore;
                ui.scoreEnergy.value = score / mapScore;
                
                
                
                if (MapManager.Instance.map.MoveLimit == touchCount)
                {
                    //실패창 출력
                    gameOverPanel.SetActive(true);
                    failPopUp.SetActive(true);
                    //확인버튼 누르면 스테이지씬으로 넘어감
                    
                }
                else
                {
                    if (ThisGameIsCleared())
                    {
                        ChangeState(States.LastPang);
                    }
                    else
                    {
                        ChangeState(States.ReadyForInteraction);
                        //게임스테이트 레디인터렉션으로 바꿔줌
                    }
                }
                break;
            case States.LastPang:
                MapManager.Instance.LastPangAction(MapManager.Instance.map.MoveLimit);
                ChangeState(States.WaitingLastPangScale);
                break;
            case States.WaitingLastPangScale:
                if (scaleIsDone)
                {
                    // 특수팡블럭들 삭제하고 실행

                    
                    
                    MapManager.Instance.CheckTarget();
                    spawn.SpawnForEmptyPlace();
                    MapManager.Instance.DeleteAllDraw();
                    MapManager.Instance.MoveAllBlock();
                    ChangeState(States.WaitingLastPangMove);
                }
                break;
            case States.WaitingLastPangMove:
                if (!MapManager.Instance.IsThereMovingPang())
                {
                    gameOverPanel.SetActive(true);
                    clearPopUp.SetActive(true);
                    Debug.Log("바뀜");
                    State = States.ReadyForInteraction;
                }
                break;
        }
    }
    
    
    

    public bool ThisGameIsCleared()
    {
        int clearCount = 0;
        var condition = MapManager.Instance.map.ClearConditionData;
        for (var i = 0; i < condition.Count; i++)
        {
            var conditionValue = (int)condition[i].ConditionBlock;
            if (ConditionStates[conditionValue] > condition[i].HowMuchForClear)
            {
                clearCount++;
            }
        }
        
        Debug.Log(clearCount);
        Debug.Log(condition.Count);

        if (clearCount == condition.Count)
        {
            return true;
        }
        
        
        return false;
    }
    
    
    public void SettingConditionStates()
    {
        var enumCount = Enum.GetValues(typeof(ClearConditionBlock)).Length;
        Debug.Log(enumCount);
        foreach (int blockType in Enum.GetValues(typeof(ClearConditionBlock)))
        {
            ConditionStates.Add(blockType, 0);
        }
    }

    public void ChangeState(States stateType)
    {
        State = stateType;
    }
}
public enum States
{
    ReadyForInteraction,
    CheckTarget,
    Waiting,
    CreateNewBlock,
    DownAllBlock,
    DrawDirection,
    CheckClearCondition,
    LastPang,
    WaitingLastPangScale,
    WaitingLastPangMove,
}
//
// [Serializable]
// public class ConditionStates
// {
//     [HorizontalGroup("ClearCondition"), HideLabel] public ClearConditionBlock ConditionBlock;
//     [HorizontalGroup("ClearCondition"), HideLabel] public int HowMuchForClear;
//
//     public ConditionStates(ClearConditionBlock conditionBlock, int howMuchForClear)
//     {
//         ConditionBlock = conditionBlock;
//         HowMuchForClear = howMuchForClear;
//     }
// }
//
