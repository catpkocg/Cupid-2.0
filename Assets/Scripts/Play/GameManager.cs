using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
                State = States.ReadyForInteraction;
                break;
            case States.CheckClearCondition:
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
                        //성공창 출력
                        gameOverPanel.SetActive(true);
                        clearPopUp.SetActive(true);
                        //확인버튼 누르면 스테이지씬으로 넘어감
                        
                        //스테이지씬에 지금게임번호 완료했다고 표시해줘야함
                        
                    }
                    else
                    {
                        ChangeState(States.ReadyForInteraction);
                        //게임스테이트 레디인터렉션으로 바꿔줌
                    }
                }
                break;
        }
    }

    public bool ThisGameIsCleared()
    {
        
        
        
        return true;
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
