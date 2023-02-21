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
                    var lindBlock = Instantiate(spawn.lineClearBlocks[target.drawValue], mapTile.transform.position,
                        Quaternion.identity);
                    mapTile.MovableBlockOnMapTile = lindBlock;
                    lindBlock.Coord = mapTile.MapTileCoord;
                }
                break;
            case >10 and < 20:
                //랜덤한 숫자 정하기
                int randomNumber = Random.Range(0, gameConfig.BlockNumber);
                
                //랜덤한 숫자의 값의 블럭들 전부다 삭제
                break;
            case >20 and < 30:
                //target.value - 20 라인의 블럭들 전부다삭제
                break;
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
            GameManager.Instance.ChangeState(States.CheckTarget);
        }
        
    }
    
}
