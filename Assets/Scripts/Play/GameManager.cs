using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wayway.Engine.Singleton;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] internal Spawn spawn;
    //카메라 사이즈 조절 필요
    //맵에서 가로세로 정보 불러와야함.
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

    private int shuffleCount;
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
        shuffleCount = 0;
        SettingConditionStates();
        State = States.ReadyForInteraction;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapManager.Instance.ShuffleBlocks();
        }
        
        switch (State)
        {
            case States.CheckThereIsBlockCanPang:
                if (MapManager.Instance.FindIsThereCanPangBlock())
                {
                    shuffleCount = 0;
                    State = States.ReadyForInteraction;
                }
                else
                {
                    if (shuffleCount > 2)
                    {
                        //더이상 섞을수 없어서 게임을 종료합니다.
                    }
                    else
                    {
                        State = States.Shuffle;
                    }
                }
                
                break;
            case States.Shuffle:
                shuffleCount++;
                //터질게 없다는 뜻임
                
                //섞습니다 라는 창이 뜨고 창이 다 출력이 완료되면 섞기
                
                //섞기 에니메이션 끝나면 다시 checkthere로 넘기기;
                
                break;
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
                        // 여기에 박스 블럭들 이미 터진 거 방지할려고 체크해 놨던거 지우기.
                        // canPangBox라는 bool값을 전부 true 로 변환.
                        MapManager.Instance.AlreadyPangChange();
                        
                        //선택할수있는 곳이 있는지 확인
                        
                        //없으면 랜덤으로 섞는 로직이 들어가야 함
                        
                        //두번까지 섞고 안되면 게임오버
                        
                        
                        // 이걸 checkisthereblockcanbepang;
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
    
    CheckThereIsBlockCanPang,
    Shuffle,
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