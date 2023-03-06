using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class MapTile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Spawn spawn;
    
    public Vector3Int MapTileCoord;
    public Block MovableBlockOnMapTile;
    public Block UnMovalbleBlockOnMapTile;

    private void Start()
    {
        spawn = GameManager.Instance.spawn;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.State == States.ReadyForInteraction)
        {
            OnTileClickHandler(MapTileCoord);
            
            //util.asdfksehoifhaef;
            
            // dddddd
            
        }
    }
    
    public void OnTileClickHandler(Vector3Int coord)
    {
        var mapTile = MapManager.Instance.map.MapTiles[coord];
        var target = mapTile.MovableBlockOnMapTile;
        var gameConfig = MapManager.Instance.gameConfig;
        
        switch (target.value)
        {
            case > 0 and < 10:
                DeleteClickedBlocks(mapTile);
                if (target.drawValue == gameConfig.SameColorClearBlockCondition)
                {
                    spawn.SpawnSameColorBlock(mapTile,target.value-1);
                    //condition state 수정해야함
                    
                }
                else if (target.drawValue > 0)
                {
                    spawn.SpawnLineClearBlock(mapTile, target.drawValue - 1);
                }
                break;
            case >10 and <40:
                mapTile.MovableBlockOnMapTile.Pang();
                GameManager.Instance.ChangeState(States.CheckTarget);
                break;
        }
        
    }

    private void DeleteClickedBlocks(MapTile mapTile)
    {
        var gameManager = GameManager.Instance;
        var sameBlockList = MapUtil.FindAllNearSameValue(mapTile.MovableBlockOnMapTile);
        if (sameBlockList.Count > 1)
        {
            
            for (int i = 0; i < sameBlockList.Count; i++)
            {
                sameBlockList[i].OnPang();
            }
            
            gameManager.touchCount++;
            gameManager.ChangeState(States.CheckTarget);
        }
    }
    
    
    
    
    
    
}
