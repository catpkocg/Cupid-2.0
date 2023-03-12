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
                    
                }
                else if (target.drawValue > 0)
                {
                    spawn.SpawnLineClearBlock(mapTile, target.drawValue - 1);
                }
                break;
            case >10 and <40:
                
                //클릭한애 주변에 살피기
                
                //클리한애가 라인클리어 블락이고 세임컬러블럭 한개면 세임컬러블럭 색깔에 랜덤한 라인클리어 블락 생성 후 터뜨리기
                
                //클릭한애가 라인클리어 블락이고 세임컬러 블럭이 두개 이상이면 전부다 터뜨리기
                
                //클릭한애가 라인클리어 블락이고 라인클리어 블럭이 한개 붙어있으면 
                //붙어있는애 방향이 같으면 랜덤방향으로 두방향 터뜨리기, 다르면 다르게 터뜨리기
                
                //클릭한애가 라인클리어 블락이고 라인클리어 블럭이 두개이상이면 3방향 터뜨리기
                
                //클릭한애가 세임컬러고 세임컬러 블럭이 한개이상 붙어있으면 다 터뜨리기
                //클릭한애가 세임컬러고 세임컬러블럭없이 라인클리어만 붙어있으면 라인클리어애들 세임컬러색깔에 다 생성하고 터뜨리기
                
                
                
                
                //아무도 안붙어있을경우 는 그냥 터뜨리기
                
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
