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
    public int touchCount;
    
    public States State { get; set; }


    private void Start()
    {
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
}
