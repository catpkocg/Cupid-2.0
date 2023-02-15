using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Wayway.Engine.Singleton;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Spawn spawn;
    [SerializeField] private Interaction interaction;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private List<Map> mapList;
    
    public int score;
    public States State { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        State = States.ReadyForInteraction;
        score = 0;
        var map = Instantiate(mapList[0], transform.position, Quaternion.identity);
        spawn.SpawnBlockOnTile(map);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var a = mapList[0].MapTiles.Count;
            Debug.Log(a);
        }

        // switch (State)
        // {
        //     case States.ReadyForInteraction:
        //         Map.Instance.DrawDirectionOnBlock();
        //         break;
        //     case States.DeleteBlock:
        //         
        //         if (Input.GetMouseButtonDown(0))
        //         {
        //             interaction.ClickBlock();
        //             
        //         }
        //         break;
        //     case States.CreateNewBlock:
        //         spawn.SpawnForEmptyPlace();
        //         State = States.CheckTarget;
        //         break;
        //     case States.CheckTarget:
        //         spawn.CheckTarget();
        //         State = States.DownNewBlock;
        //         break;
        //     case States.DownNewBlock:
        //         spawn.MoveAllDown();
        //         State = States.Waiting;
        //         break;
        //     case States.Waiting:
        //         break;
        // }
    }
    
    public void ChangeState(States stateType)
    {
        State = stateType;
    }
    
}
public enum States
{
    None = 0,
    ReadyForInteraction,
    CheckTarget,
    Waiting,
    DeleteBlock,
    CreateNewBlock,
    DownNewBlock,
}
