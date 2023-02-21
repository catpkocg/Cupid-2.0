using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wayway.Engine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Spawn spawn;
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
                    //DeleteClickedBlocks(mapTile);
                    var footBlock = Instantiate(spawn.sameColorClearBlocks[Random.Range(0, gameConfig.BlockNumber)],
                        mapTile.transform.position, Quaternion.identity);
                    mapTile.MovableBlockOnMapTile = footBlock;
                    footBlock.Coord = mapTile.MapTileCoord;
                }
                else if (target.drawValue > 0)
                {
                    //DeleteClickedBlocks(mapTile);
                    var lindBlock = Instantiate(spawn.lineClearBlocks[target.drawValue-1], mapTile.transform.position,
                        Quaternion.identity);
                    mapTile.MovableBlockOnMapTile = lindBlock;
                    lindBlock.Coord = mapTile.MapTileCoord;
                }
                GameManager.Instance.ChangeState(States.CheckTarget);
                break;
            case >10:
                mapTile.MovableBlockOnMapTile.Pang();
                GameManager.Instance.ChangeState(States.CheckTarget);
                break;
            // case >10 and < 20:
            //     mapTile.MovableBlockOnMapTile.Pang();
            //     Debug.Log("같은색깔 다터뜨리는거 실행중");
            //     break;
            // case >20 and < 30:
            //     mapTile.MovableBlockOnMapTile.Pang();
            //     Debug.Log("같은라인 다터뜨리는거 실행중");
            //     break;
        }
        
    }

    private void DeleteClickedBlocks(MapTile mapTile)
    {
        var sameBlockList = MapManager.Instance.FindAllNearSameValue(mapTile.MovableBlockOnMapTile);
        if (sameBlockList.Count > 1)
        {
            
            for (int i = 0; i < sameBlockList.Count; i++)
            {
                sameBlockList[i].Pang();
            }
            
            GameManager.Instance.touchCount++;
        }
    }

    

    private void DeleteSameColorBlock(MapTile mapTile)
    {
        
    }
    
    
}
