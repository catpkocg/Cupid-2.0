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
    public int score;
    
    public States State { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        State = States.ReadyForInteraction;
        score = 0;
    }

    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }

        switch (State)
        {
            case States.ReadyForInteraction:
                //Map.Instance.MakeListForFindDir();
                Map.Instance.DrawDirectionOnBlock();
                //State = States.DeleteBlock;
                break;
            case States.DeleteBlock:
                //Debug.Log("이거 들어옴?");
                if (Input.GetMouseButtonDown(0))
                {
                    
                    Map.Instance.DeleteAllDraw();
                    
                    HitPoint();
                    
                }
                break;
            case States.CreateNewBlock:
                spawn.SpawnForEmptyPlace();
                State = States.CheckTarget;
                break;
            case States.CheckTarget:
                spawn.CheckTarget();
                State = States.DownNewBlock;
                break;
            case States.DownNewBlock:
                spawn.MoveAllDown();
                State = States.Waiting;
                break;
            case States.Waiting:
                break;
        }
    }
    
    public void ChangeState(States stateType)
    {
        State = stateType;
    }
    
    private void HitPoint()
    {
        var plane = new Plane();
        plane.Set3Points(Vector3.zero, Vector3.up, Vector3.right);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out var enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
            var cellCoord = grid.WorldToCell(hitPoint);
            //Map.Instance.DeleteBlock(Util.UnityCellToCube(cellCoord));
            var blockPos = Util.UnityCellToCube(cellCoord);
            var clickBlock = Map.Instance.BlockPlace[blockPos];
            var clickBlockNeighbor = Map.Instance.FindAllNearSameValue(clickBlock);

            if (clickBlock.specialValue != 0)
            {
                
            }
            
            
            
            Map.Instance.DeleteBlockList(clickBlockNeighbor);
            
            
            ChangeState(States.CreateNewBlock);
        }
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
