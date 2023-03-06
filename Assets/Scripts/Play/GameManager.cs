using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wayway.Engine.Singleton;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] internal Spawn spawn;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject failPopUp;
    [SerializeField] private GameObject clearPopUp;
    [SerializeField] private List<GameObject> starFill;
    [SerializeField] private TextMeshProUGUI moveNumber;
    
    //public List<ConditionStates> ConditionStatesList;
    public PlayerData PlayerData;
    public ConditionImage conditionImage;
    public SerializeDictionary<int,int> ConditionStates = new ();
    public GameConfig gameConfig;
    public PlayUI ui;
    
    public int score;
    public int touchCount;
    public bool scaleIsDone = false;
    
    public States State { get; set; }

    private bool IsCleared
    {
        get
        {
            var clearCount = 0;
            var condition = MapManager.Instance.map.ClearConditionData;
            for (var i = 0; i < condition.Count; i++)
            {
                var conditionValue = (int)condition[i].ConditionBlock;
                if (ConditionStates[conditionValue] >= condition[i].HowMuchForClear)
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
    }
    
    private void Start()
    {
        SettingConditionStates();
        State = States.ReadyForInteraction;
    }
    private void Update()
    {
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
                CheckStarFill();
                //Check Condition Text Count;
                CheckConditionCount(MapManager.Instance.map);
                
                if (MapManager.Instance.map.MoveLimit == 0)
                {
                    //실패창 출력
                    gameOverPanel.SetActive(true);
                    failPopUp.SetActive(true);
                    //확인버튼 누르면 스테이지씬으로 넘어감
                }
                else
                {
                    if (IsCleared)
                    {
                        var stageLevel = MapManager.Instance.map.GameConfig.StageLevel;
                        if (stageLevel > PlayerData.clearedMaxStage)
                        {
                            PlayerData.clearedMaxStage = stageLevel;
                            
                        }
                        if (score > PlayerData.stagesInformation[stageLevel-1].StageScore)
                        {
                            PlayerData.stagesInformation[stageLevel-1].StageScore = score;
                        }
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
                MapManager.Instance.LastPangScaleAction(MapManager.Instance.map.MoveLimit);
                ChangeState(States.WaitingLastPangScale);
                break;
            case States.WaitingLastPangScale:
                if (scaleIsDone)
                {
                    // 특수팡블럭들 삭제하고 실행
                    MapManager.Instance.LastPangBlock();
                    
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

    public void ChangeState(States stateType)
    {
        State = stateType;
    }
    
    private void CheckStarFill()
    {
        var mapScore = (float)MapManager.Instance.map.PerfectScore;
        var percentScore = score / mapScore;
        var percent1 = (float)1 / 3;
        var percent2 = (float)2 / 3;
        if (percentScore < 1)
        {
            ui.scoreEnergy.value = percentScore;
            
        }
        else
        {
            ui.scoreEnergy.value = 1;
            
        }
        if (percentScore > percent1)
        {
            Debug.Log(percent1);
            Debug.Log(percentScore);
            starFill[0].SetActive(true);
            ConditionStates[(int)ClearConditionBlock.StarScore] = 1;
        }
        if (percentScore > percent2)
        {
            starFill[1].SetActive(true);
            ConditionStates[(int)ClearConditionBlock.StarScore] = 2;
        }
        if (percentScore > 1)
        {
            starFill[2].SetActive(true);
            ConditionStates[(int)ClearConditionBlock.StarScore] = 3;
        }
    }
    private void CheckConditionCount(Map map)
    {
        var conditionList = map.ClearConditionData;
        for (var i = 0; i < conditionList.Count; i++)
        {
            var checkNum = conditionList[i].HowMuchForClear - ConditionStates[(int)conditionList[i].ConditionBlock];
            if (checkNum > 0)
            {
                ui.conditionCount[i].GetComponent<TextMeshProUGUI>().text = 
                    checkNum.ToString();
            }
            else
            {
                ui.conditionCount[i].GetComponent<TextMeshProUGUI>().text = 0.ToString();
            }
        }
    }
    private void SettingConditionStates()
    {
        var enumCount = Enum.GetValues(typeof(ClearConditionBlock)).Length;
        Debug.Log(enumCount);
        foreach (int blockType in Enum.GetValues(typeof(ClearConditionBlock)))
        {
            ConditionStates.Add(blockType, 0);
        }
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