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
    [SerializeField] private List<Map> mapList;
    [SerializeField] private Camera cam;
    
    public GameConfig gameConfig;
    public Interaction interaction;
    public Map map;
    public int score;
    public States State { get; set; }


    private void Start()
    {
        State = States.ReadyForInteraction;
        Debug.Log(gameConfig.StageLevel);
        SettingStart();
    }


    void Update()
    {
        switch (State)
        {
            case States.ReadyForInteraction:
                break;
            case States.CreateNewBlock:
                //빈곳찾아서 계산하고 새로운 애들 생성
                //spawn.SpawnForEmptyPlace();
                State = States.CheckTarget;
                break;
            case States.CheckTarget:
                // 애들이 이동할곳이랑 이동할 애들 계산
                //spawn.CheckTarget();
                State = States.DownNewBlock;
                break;
            
            case States.DownNewBlock:
                //애들 내리기
                //spawn.MoveAllDown();
                State = States.Waiting;
                break;
            case States.Waiting:
                
                //애니메이션 끝나는지 확인
                //애니메이션 꿑나면 readyforinteraction으로 스테이트 변환
                
                break;
        }
    }

    
    
    
    public void ChangeState(States stateType)
    {
        State = stateType;
    }

    private void SettingStart()
    {
        map = Instantiate(mapList[gameConfig.StageLevel-1], transform.position, Quaternion.identity);
        map.MapTilePresetDataList.ForEach(x =>
        {
            map.MapTiles.Add(x.Coord, x.MapTile);
        });
        State = States.ReadyForInteraction;
        score = 0;
        spawn.SpawnBlockOnTile(map);
    }
    
}
public enum States
{
    
    ReadyForInteraction,
    CheckTarget,
    Waiting,
    DeleteBlock,
    CreateNewBlock,
    DownNewBlock,
}
