using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wayway.Engine;
using Wayway.Engine.Singleton;

public class MapManager : MonoSingleton<MapManager>
{
    [SerializeField] private List<Map> mapList;
    [SerializeField] private Spawn spawn;
    [SerializeField] private NeighborPos neighborPos;

    public GameConfig gameConfig;
    public Map map;
    public List<Block> WhatWillMove;
    public List<Vector3Int> WhereToMove;
    private List<Block> allBlockForCheckDir;
    
    private void Start()
    {
        SettingMap();
        DrawDirectionOnBlock();
    }

    private void SettingMap()
    {
        map = Instantiate(mapList[gameConfig.StageLevel - 1], transform.position, Quaternion.identity);
        map.MapTilePresetDataList.ForEach(x => { map.MapTiles.Add(x.Coord, x.MapTile); });
        GameManager.Instance.State = States.ReadyForInteraction;
        spawn.SpawnBlockOnTile(map);
    }

    public void MoveAllBlock()
    {
        var mapTiles = map.MapTiles;
        for (var i = 0; i < WhatWillMove.Count; i++)
        {
            WhatWillMove[i].Move(mapTiles[WhereToMove[i]]);
        }
    }

    public void CheckTarget()
    {
        WhatWillMove = new List<Block>();
        WhereToMove = new List<Vector3Int>();
        var mapTiles = MapManager.Instance.map.MapTiles;
        mapTiles.Keys.ForEach(key =>
        {
            if (mapTiles[key].MovableBlockOnMapTile != null)
            {
                var moveCount = MapUtil.CountNullPlace(mapTiles, key);
                if (moveCount != 0)
                {
                    WhatWillMove.Add(mapTiles[key].MovableBlockOnMapTile);
                    WhereToMove.Add((key + (new Vector3Int(0, 1, -1) * moveCount)));
                }
            }
        });
    }

    public bool IsThereMovingPang()
    {
        var mapTiles = map.MapTiles;
        for (var i = 0; i < map.MapTilePresetDataList.Count; i++)
        {
            if (mapTiles[map.MapTilePresetDataList[i].Coord].MovableBlockOnMapTile != null)
            {
                var currBlock = mapTiles[map.MapTilePresetDataList[i].Coord].MovableBlockOnMapTile;
                if (currBlock.IsMoving)
                {
                    Debug.Log(currBlock + "이거 움직이고 있음");
                    return true;
                }
            }
        }
        return false;
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

                    if (!searched.Contains(currSameBlock)
                        && !toSearch.Contains(currSameBlock))
                    {
                        allSameBlocks.Add(currSameBlock);
                        toSearch.Add(currSameBlock);
                    }
                }
            }

            searched.Add(currSearchTarget);
            toSearch.Remove(currSearchTarget);

            if (500 < tempCount) break;
            tempCount++;
        }

        return allSameBlocks;
    }

    public List<Block> FindNearSameValue(Block block)
    {
        var map = MapManager.Instance.map;
        List<Block> sameBlockList = new List<Block>();
        for (int i = 0; i < neighborPos.neighbor.Count; i++)
        {
            var neighbor = block.Coord + neighborPos.neighbor[i].neighborPos;
            if (map.MapTiles.ContainsKey(neighbor) && map.MapTiles[neighbor].MovableBlockOnMapTile != null)
            {
                var neighborValue = map.MapTiles[neighbor].MovableBlockOnMapTile.value;
                // 블럭끼리의 값이 같은거 + 나무상자의 value일경우 같은 블럭에 넣어서 삭제할수있도록 한다.
                if (neighborValue == block.value)
                {
                    sameBlockList.Add(map.MapTiles[neighbor].MovableBlockOnMapTile);
                }
            }
        }

        return sameBlockList;
    }
    
    
    
    
    


    public void MakeListForFindDir()
    {
        allBlockForCheckDir = new List<Block>();
        map.MapTiles.Keys.ForEach(keys =>
        {
            var block = map.MapTiles[keys].MovableBlockOnMapTile;
            if (block.value < 10)
            {
                allBlockForCheckDir.Add(map.MapTiles[keys].MovableBlockOnMapTile);
            }
        });
    }

    public void DrawDirectionOnBlock()
    {
        MakeListForFindDir();

        for (int i = 0; i < allBlockForCheckDir.Count; i++)
        {
            if (allBlockForCheckDir[i].drawValue == 0)
            {
                List<Block> sameBlock = FindAllNearSameValue(allBlockForCheckDir[i]);
                if (sameBlock.Count >= gameConfig.SameColorClearBlockCondition)
                {
                    for (int j = 0; j < sameBlock.Count; j++)
                    {
                        sameBlock[j].foot.SetActive(true);
                        sameBlock[j].drawValue = gameConfig.SameColorClearBlockCondition;
                    }
                }
                else if (sameBlock.Count >= gameConfig.LineClearBlockCondition)
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

                    allLineDif.Add(MapUtil.CalCulateMaxAndMinDif(allXValue));
                    allLineDif.Add(MapUtil.CalCulateMaxAndMinDif(allYValue));
                    allLineDif.Add(MapUtil.CalCulateMaxAndMinDif(allZValue));

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

                        var xAvg = MapUtil.CalCulateAverage(allXValue);
                        var yAvg = MapUtil.CalCulateAverage(allYValue);
                        var zAvg = MapUtil.CalCulateAverage(allZValue);
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
                                sameBlock[j].drawValue = highNumDirSign[dirValue];
                            }

                        }

                        else
                        {
                            for (int j = 0; j < sameBlock.Count; j++)
                            {
                                sameBlock[j].dir[highNumDirSign[0] - 1].SetActive(true);
                                sameBlock[j].drawValue = highNumDirSign[0];
                            }

                        }
                    }
                    else
                    {
                        for (int j = 0; j < sameBlock.Count; j++)
                        {
                            sameBlock[j].dir[highNumDirSign[0] - 1].SetActive(true);
                            sameBlock[j].drawValue = highNumDirSign[0];
                        }
                    }
                }
                else
                {
                    for (int n = 0; n < sameBlock.Count; n++)
                    {
                        allBlockForCheckDir.Remove(sameBlock[n]);
                    }
                }
            }
        }

        //GameManager.Instance.ChangeState(States.DeleteBlock);
    }
    
    public void DeleteAllDraw()
    {
        for (int i = 0; i < allBlockForCheckDir.Count; i++)
        { 
            for (int j = 0; j < allBlockForCheckDir[i].dir.Count; j++)
            {
                if (allBlockForCheckDir[i].dir[j] != null)
                {
                    allBlockForCheckDir[i].dir[j].SetActive(false);
                }
                
            }
            if (allBlockForCheckDir[i].foot != null)
            {
                allBlockForCheckDir[i].foot.SetActive(false);
            }
            
            allBlockForCheckDir[i].drawValue = 0;
        }

        allBlockForCheckDir.Clear();
        
        GameManager.Instance.ChangeState(States.CreateNewBlock);
        
    }

    
    
    
    
    
}