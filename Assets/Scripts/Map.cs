using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Wayway.Engine;
using Wayway.Engine.Singleton;

public class Map : MonoSingleton<Map>
{
    [SerializeField] private Transform backGround;
    [SerializeField] private List<GameObject> mapList;
    [SerializeField] private Camera cam;
    [SerializeField] private NeighborPos neighborPos;
    
    public Tilemap tilemap;
    public GameConfig gameConfig;
    
    
    public List<Vector3> canSpwanPlace = new List<Vector3>();
    public Dictionary<Vector3Int, Block> BlockPlace = new Dictionary<Vector3Int, Block>();
    public List<Vector3Int> newBlockSpawnPos;
    private List<Block> allBlockForCheckDir;

    
    // material 생성
    // 이미지를 material - Albedo에 넣기
    // Material Shader 를 Sprites default로 변경
    // 그리고 그거 사용
    
    
    protected override void Awake()
    {
        var newMap = Instantiate(mapList[gameConfig.StageLevel], transform.position, Quaternion.identity);
        var presentTile = newMap.GetComponentInChildren<Tilemap>();
        tilemap = presentTile;
        newBlockSpawnPos = new List<Vector3Int>()
        {
        new Vector3Int(-3, -1, 4), new Vector3Int(-2, -2, 4), new Vector3Int(-1, -3, 4),
        new Vector3Int(0, -4, 4), new Vector3Int(1, -4, 3), new Vector3Int(2, -4, 2),
        new Vector3Int(3, -4, 1)
        };
    }

    void Start()
    {
        FindCanPutTile();
    }

    private void FindCanPutTile()
    {
        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
        {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = new Vector3Int(n, p, 0);
                Vector3 place = tilemap.CellToWorld(localPlace);
                var putPlace = new Vector3(place.x, place.y, 0);
                if (tilemap.HasTile(localPlace))
                {
                    canSpwanPlace.Add(putPlace);
                }
            }
        }

        cam.transform.GetComponent<Camera>().orthographicSize = 6f; //tilemap.cellBounds.xMax - tilemap.cellBounds.xMin;
    }
    
    // 모든 블럭을 훑는 리스트 생성, 딕셔너리에 있는 거 다 삽입
    //모든 블럭을 한번씩 훑는다. gameconfig.specialblock 2 number 인지 확인, 그리고 specialblock 1 인지 확인
    
    // 2인경우는 모든 블럭 이미지 2 number을 생성한다. 1 인경우는 모든 블럭에 이미지 1 number를 생성한다.
    // 훑은 블럭은 리스트에서 빼준다.
    
    

    public void DeleteBlockList(List<Block> sameBlockList)
    {
        for (int i = 0; i < sameBlockList.Count; i++)
        {
            DeleteBlock(sameBlockList[i].Coord);
        }
    }

    public void DeleteLineBlock(Vector3Int clickPos, int line)
    {

        List<Block> mustDeleteBlocks = new List<Block>();
        
        if (line == 0)
        {
            BlockPlace.Keys.ForEach(keys =>
            {
                if (keys.x == clickPos.x)
                {
                    mustDeleteBlocks.Add(BlockPlace[keys]);
                }
            });
        }
        else if (line == 1)
        {
            BlockPlace.Keys.ForEach(keys =>
            {
                if (keys.y == clickPos.y)
                {
                    mustDeleteBlocks.Add(BlockPlace[keys]);
                }
            });
        }
        else
        {
            BlockPlace.Keys.ForEach(keys =>
            {
                if (keys.z == clickPos.z)
                {
                    mustDeleteBlocks.Add(BlockPlace[keys]);
                }
                
            });
        }
        
        DeleteBlockList(mustDeleteBlocks);
    }


    public void DeleteSameColor(Vector3Int clickPos, int value)
    {
        List<Block> mustDeleteBlocks = new List<Block>();
        
        BlockPlace.Keys.ForEach(keys =>
        {
            if (BlockPlace[keys].value == value)
            {
                mustDeleteBlocks.Add(BlockPlace[keys]);
            }
                
        });
        
        DeleteBlockList(mustDeleteBlocks);
        DeleteBlock(clickPos);
    }
    
    
    public void DeleteBlock(Vector3Int clickPos)
    {
        Destroy(BlockPlace[clickPos].gameObject);
        BlockPlace[clickPos] = null;
        //Debug.Log(BlockPlace.Count);
    }

    public List<Block> FindAllNearSameValue(Block block)
    {
        var toSearch = new List<Block>();
        var searched = new List<Block>();
        var allSameBlocks = new List<Block>();
        toSearch.Add(block);
        allSameBlocks.Add(block);
        var tempCount = 0;
        while (!toSearch.IsNullOrEmpty())
        {
            var currSearchTarget = toSearch[0];
            var sameBlocks = FindNearSameValue(currSearchTarget);
            if (!sameBlocks.IsNullOrEmpty())
            {
                for (var i = 0; i < sameBlocks.Count; i++)
                {
                    var currSameBlock = sameBlocks[i];
                    if (!searched.Contains(currSameBlock) && !toSearch.Contains(currSameBlock))
                    {
                        allSameBlocks.Add(currSameBlock);
                        toSearch.Add(currSameBlock);
                    }
                }
            }
            searched.Add(currSearchTarget);
            toSearch.Remove(currSearchTarget);

            if(500 < tempCount) break;
            tempCount++;
        }
        return allSameBlocks;
    }
    
    
    public List<Block> FindNearSameValue(Block block)
    {
        List<Block> sameBlockList = new List<Block>();
        for (int i = 0; i < neighborPos.neighbor.Count; i++)
        {
            var neighbor = block.Coord + neighborPos.neighbor[i].neighborPos;
            var tilePos = Util.CubeToUnityCell(neighbor);
            if (tilemap.HasTile(tilePos) && BlockPlace[neighbor] != null)
            {
                var neiborValue = BlockPlace[neighbor].value;
                if (neiborValue == block.value)
                {
                    sameBlockList.Add(BlockPlace[neighbor]);
                }
            }
        }
        return sameBlockList;
    }

    public int CountNullPlace(Vector3Int wantToCheckPos)
    {
        var nullCount = 0;
        var targetPos = wantToCheckPos + new Vector3Int(0, 1, -1);
        var tilePos = Util.CubeToUnityCell(targetPos);
        //Debug.Log(targetPos);
        while (tilemap.HasTile(tilePos))
        {
            //Debug.Log(targetPos);
            if (BlockPlace[targetPos] == null)
            {
                nullCount++;
            }
            targetPos += new Vector3Int(0, 1, -1);
            tilePos = Util.CubeToUnityCell(targetPos);
        }
        return nullCount;
    }
    
    public void MakeListForFindDir()
    {
        allBlockForCheckDir= new List<Block>();
        //var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
        
        BlockPlace.Values.ForEach(value =>
        {
            allBlockForCheckDir.Add(value);
        });
    }

    public void DrawDirectionOnBlock()
    {
        
        MakeListForFindDir();
        
        
        for (int i = 0; i < allBlockForCheckDir.Count; i++)
        {
            if (allBlockForCheckDir[i].specialValue < gameConfig.SpecialBlock1Condition)
            {
                List<Block> sameBlock = FindAllNearSameValue(allBlockForCheckDir[i]);
                
            if (sameBlock.Count >= gameConfig.SpecialBlock2Condition)
            {
                for (int j = 0; j < sameBlock.Count; j++)
                {
                    sameBlock[j].foot.SetActive(true);
                    sameBlock[j].specialValue = (int)gameConfig.SpecialBlock2Condition;
                }
            }
            else if (sameBlock.Count >= gameConfig.SpecialBlock1Condition)
            {
                
                List<int> highNumDirSign = new List<int>();

                List<int> allLineDif = new List<int>();
                List<int> direction = new List<int>()
                {
                    1, 2, 3
                };

                List<int> allXValue = new List<int>();
                List<int> allYValue = new List<int>();
                List<int> allZValue = new List<int>();
                for (int k = 0; k < sameBlock.Count; k++)
                {
                    allXValue.Add(sameBlock[k].Coord.x);
                    allYValue.Add(sameBlock[k].Coord.y);
                    allZValue.Add(sameBlock[k].Coord.z);
                }
                
                allLineDif.Add(CalCulateMaxAndMinDif(allXValue));
                allLineDif.Add(CalCulateMaxAndMinDif(allYValue));
                allLineDif.Add(CalCulateMaxAndMinDif(allZValue));

                var highValue = allLineDif.Max();
                
                for (int m = 0; m < allLineDif.Count; m++)
                {
                    if (allLineDif[m] == highValue)
                    {
                        highNumDirSign.Add(direction[m]);   
                    }
                }

                if (highNumDirSign.Count > 1)
                {
                    
                    highNumDirSign = new List<int>();
                    
                    var totalX = 0;
                    var totalY = 0;
                    var totalZ = 0;
                    
                    var xAvg = CalCulateAverage(allXValue);
                    var yAvg = CalCulateAverage(allYValue);
                    var zAvg = CalCulateAverage(allZValue);
                    for (int z = 0; z < sameBlock.Count; z++)
                    {
                        var xAbs = Math.Abs(allXValue[z] - xAvg);
                        var yAbs = Math.Abs(allYValue[z] - yAvg);
                        var zAbs = Math.Abs(allZValue[z] - zAvg);
                        totalX += xAbs;
                        totalY += yAbs;
                        totalZ += zAbs;
                    }
                    
                    List<int> difWithAbg = new List<int>()
                    {
                        totalX, totalY, totalZ
                    };
                    
                    var maxValue = difWithAbg.Max();

                    for (int k = 0; k < difWithAbg.Count; k++)
                    {
                        if (difWithAbg[k] == maxValue)
                        {
                            highNumDirSign.Add(direction[k]);
                        }
                    }

                    if (highNumDirSign.Count > 1)
                    {
                        var minValue = highNumDirSign.Min();
                        var dirValue = 0;
                        for (int l = 0; l < highNumDirSign.Count; l++)
                        {
                            if (highNumDirSign[l] == minValue)
                            {
                                dirValue = l;
                            }
                        }

                        for (int j = 0; j < sameBlock.Count; j++)
                        {
                            sameBlock[j].dir[highNumDirSign[dirValue] - 1].SetActive(true);
                            sameBlock[j].specialValue = highNumDirSign[dirValue];
                        }
                        
                    }
                    
                    else
                    {
                        for (int j = 0; j < sameBlock.Count; j++)
                        {
                            sameBlock[j].dir[highNumDirSign[0] - 1].SetActive(true);
                            sameBlock[j].specialValue = highNumDirSign[0];
                        }
                    
                    }

                }
                
                else
                {
                    for (int j = 0; j < sameBlock.Count; j++)
                    {
                        sameBlock[j].dir[highNumDirSign[0] - 1].SetActive(true);
                        sameBlock[j].specialValue = highNumDirSign[0];
                    }
                }
            }
            
                for (int n = 0; n < sameBlock.Count; n++)
                {
                    allBlockForCheckDir.Remove(sameBlock[n]);
                }
            
            }
            
        }
        
        
        GameManager.Instance.ChangeState(States.DeleteBlock);
    }
    

    private int CalCulateMaxAndMinDif(List<int> allNum)
    {
        var min = allNum.Min();
        var max = allNum.Max();

        var amount = max - min;

        return amount;
    }

    private int CalCulateAverage(List<int> allNum)
    {
        int Sum = allNum.Sum();
        int Average = Sum / allNum.Count;

        return Average;
    }

    public void DeleteAllDraw()
    {
        allBlockForCheckDir= new List<Block>();
        
        BlockPlace.Values.ForEach(value =>
        {
            if (value != null && value.value < 10)
            {
                allBlockForCheckDir.Add(value);
            }
            
        });
        
        
        for (int i = 0; i < allBlockForCheckDir.Count; i++)
        { 
            for (int j = 0; j < allBlockForCheckDir[i].dir.Count; j++)
            {
                allBlockForCheckDir[i].dir[j].SetActive(false);
            }
            
            allBlockForCheckDir[i].foot.SetActive(false);
            allBlockForCheckDir[i].specialValue = 0;
            
            
        }
        
        //DrawDirectionOnBlock();
        GameManager.Instance.ChangeState(States.CreateNewBlock);
    }
    
}
