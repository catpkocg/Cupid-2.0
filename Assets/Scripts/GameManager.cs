using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
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
        if (Input.GetMouseButtonDown(0))
        {
            HitPoint();
            //State = States.CreateNewBlock;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            spawn.MoveAllBlock();
        }
        
        switch (State)
        {
            case States.ReadyForInteraction:
                //interaction.ClickForMerge();
                break;
            case States.DeleteBlock:
                //interaction.DeleteMergedObj(interaction.sameBlocks);
                State = States.CreateNewBlock;
                break;
            case States.CreateNewBlock:
                //spawn.SpawnForEmptyPlace();
                State = States.CheckTarget;
                break;
            case States.CheckTarget:
                //spawnAndDelete.CheckTarget();
                State = States.DownNewBlock;
                break;
            case States.DownNewBlock:
                //spawnAndDelete.MoveAllBlocks();
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
            Map.Instance.DeleteBlockList(clickBlockNeighbor);
            //Debug.Log(Map.Instance.CountNullPlace(blockPos));
            
            spawn.SpawnForEmptyPlace();
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
