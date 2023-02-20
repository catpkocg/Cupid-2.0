using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Wayway.Engine.Singleton;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Spawn spawn;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Camera cam;
    
    public GameConfig gameConfig;
    public Interaction interaction;
    public int score;
    public States State { get; set; }


    private void Start()
    {
        State = States.ReadyForInteraction;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapManager.Instance.MoveAllBlock();
        }
        
        switch (State)
        {
            case States.ReadyForInteraction:
                break;
            case States.CheckTarget:
                MapManager.Instance.CheckTarget();
                State = States.CreateNewBlock;
                break;
            case States.CreateNewBlock:
                spawn.SpawnForEmptyPlace();
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
                    State = States.ReadyForInteraction;
                }
                //애니메이션 끝나는지 확인
                //애니메이션 꿑나면 readyforinteraction으로 스테이트 변환
                
                break;
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
    DeleteBlock,
    CreateNewBlock,
    DownAllBlock,
}
