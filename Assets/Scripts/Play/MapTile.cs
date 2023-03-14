using System;
using System.Collections.Generic;
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
                    
                }
                else if (target.drawValue > 0)
                {
                    spawn.SpawnLineClearBlock(mapTile, target.drawValue - 1);
                }
                break;
            case >10 and <40:

                var clickedBlock = mapTile.MovableBlockOnMapTile;
                var specialBlockList = MapUtil.FindAllNearSpecialBlocks(clickedBlock);
                Debug.Log("스페셜 블럭 개수" + specialBlockList.Count);
                if (specialBlockList.Count > 1)
                {

                    SpecialBlockSum(mapTile, specialBlockList);

                }
                else
                {
                    //mapTile.MovableBlockOnMapTile.Pang();
                    //GameManager.Instance.ChangeState(States.CheckTarget);
                }
                
                
                //클릭한애 주변에 살피기
                
                //클리한애가 라인클리어 블락이고 세임컬러블럭 한개면 세임컬러블럭 색깔에 랜덤한 라인클리어 블락 생성 후 터뜨리기
                
                //클릭한애가 라인클리어 블락이고 세임컬러 블럭이 두개 이상이면 전부다 터뜨리기
                
                //클릭한애가 라인클리어 블락이고 라인클리어 블럭이 한개 붙어있으면 
                //붙어있는애 방향이 같으면 랜덤방향으로 두방향 터뜨리기, 다르면 다르게 터뜨리기
                
                //클릭한애가 라인클리어 블락이고 라인클리어 블럭이 두개이상이면 3방향 터뜨리기
                
                //클릭한애가 세임컬러고 세임컬러 블럭이 한개이상 붙어있으면 다 터뜨리기
                //클릭한애가 세임컬러고 세임컬러블럭없이 라인클리어만 붙어있으면 라인클리어애들 세임컬러색깔에 다 생성하고 터뜨리기
                
                
                
                
                //아무도 안붙어있을경우 는 그냥 터뜨리기
                
                
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

    private void SpecialBlockSum(MapTile mapTile, List<Block> specialBlockList)
    {
        var lineClearBlockCount = 0;
        var sameColorBlockCount = 0;
        for (int i = 0; i < specialBlockList.Count; i++)
        {
            if (specialBlockList[i].value < 30)
            {
                lineClearBlockCount++;
            }
            else
            {
                sameColorBlockCount++;
            }
            
            MapManager.Instance.map.MapTiles[specialBlockList[i].Coord].MovableBlockOnMapTile = null;
            specialBlockList[i].MoveBlock(mapTile);
            
            //이동끝나면 터뜨려야겠다
            
        }

        Debug.Log("라인컬러블럭" + lineClearBlockCount);
        Debug.Log("세임컬러블럭" + sameColorBlockCount);

        if (sameColorBlockCount > 1)
        {
            // 전부다 터뜨림
            SpecialBlockSumPang(specialBlockList);
            MapManager.Instance.AllPang();
            GameManager.Instance.ChangeState(States.CheckTarget);
            
        }
        else if(sameColorBlockCount == 1)
        {
            // 세임컬러 블락색깔에 랜덤한 라인클리어 블락 생성후 다 터뜨림
        }
        else if (lineClearBlockCount > 2)
        {
            // 3방향 터뜨림
        }
        else if (lineClearBlockCount > 1)
        {
            // 방향이 같으면 랜덤한 방향추가로 하나 더 터뜨림
            
            // 방향이 같지않으면 둘다 터뜨림
        }
    }

    private void SpecialBlockSumPang(List<Block> specialBlockList)
    {
        for (int i = 0; i < specialBlockList.Count; i++)
        {
            specialBlockList[i].PangMainBlock(specialBlockList[i]);
        }
    }
    

    private void CreateLineClearBlockOnSameColor()
    {
        
    }

    private void AllLineClearBlocksPang()
    {
        
    }

    private void AllDirectionPang()
    {
        
    }

    private void TwoDirectionPang()
    {
        
    }
    
    
    
    
    
}
