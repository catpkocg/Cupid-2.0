using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wayway.Engine;

//TODO 인터렉션은 저번에 없에기로 하지 않았나? 굳이 이게 여기 필요한가?
public class Interaction : MonoBehaviour
{
    [SerializeField] private Spawn spawn;
    public void OnTileClickHandler(Vector3Int coord)
    {
        
        var mapTile = MapManager.Instance.map.MapTiles[coord];
        var target = mapTile.MovableBlockOnMapTile;
        var gameConfig = MapManager.Instance.gameConfig;
        
        //TODO 이건 스폰하는 행위인데 인터렉션에 들어있느게 어색함. 객체지향적 사고에 어긋남. 좋지 않은 프랙티스
        switch (target.value)
        {
            case > 0 and < 10:
                DeleteClickedBlocks(mapTile);
                if (target.drawValue == gameConfig.SameColorClearBlockCondition)
                {
                    //TODO! 터트린 블럭과 같은 색의 세임컬러블럭을 만들어야함.
                    var footBlock = Instantiate(spawn.sameColorClearBlocks[Random.Range(0, gameConfig.BlockNumber)],
                        mapTile.transform.position, Quaternion.identity);
                    mapTile.MovableBlockOnMapTile = footBlock;
                    footBlock.Coord = mapTile.MapTileCoord;
                }
                else if (target.drawValue > 0)
                {
                    var lindBlock = Instantiate(spawn.lineClearBlocks[target.drawValue-1], mapTile.transform.position,
                        Quaternion.identity);
                    mapTile.MovableBlockOnMapTile = lindBlock;
                    lindBlock.Coord = mapTile.MapTileCoord;
                }
                break;
            case >10 and <40:
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
        var gameManager = GameManager.Instance;
        var sameBlockList = MapManager.Instance.FindAllNearSameValue(mapTile.MovableBlockOnMapTile);
        if (sameBlockList.Count > 1)
        {
            
            for (int i = 0; i < sameBlockList.Count; i++)
            {
                sameBlockList[i].Pang();
            }
            
            gameManager.touchCount++;
            gameManager.ChangeState(States.CheckTarget);
        }
    }

    
}
