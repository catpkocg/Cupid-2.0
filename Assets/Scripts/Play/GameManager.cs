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
    [SerializeField] private Interaction interaction;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private List<Map> mapList;
    [SerializeField] private Camera cam;

    public Map map;
    public int score;
    public States State { get; set; }
    
    protected override void Awake()
    {
        SettingStart();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var a = mapList[0].MapTiles.Count;
            Debug.Log(a);
        }
        
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Ray ray = new Ray(mousePosition, transform.forward * 1000);
            Debug.DrawRay(ray.origin, Vector3.forward * 1000, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, transform.forward * 1000);
            if (hit)
            {
                var a = hit.collider.transform.position;
                var b = new Vector3Int((int)a.x, (int)a.y, (int)a.z);
                map.MapTiles[b].MovableBlockOnMapTile.Pang();
                Debug.Log(hit.collider.transform.gameObject);
            }
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

    private void SettingStart()
    {
        map = Instantiate(mapList[0], transform.position, Quaternion.identity);
        for (int i = 0; i < map.MapTileKey.Count; i++)
        {
            Debug.Log(1);
            map.MapTiles.Add(map.MapTileKey[i],map.MapTileValue[i]);
        }
        State = States.ReadyForInteraction;
        score = 0;
        spawn.SpawnBlockOnTile(map);
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
