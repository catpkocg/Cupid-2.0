using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Wayway.Engine;
using Wayway.Engine.Singleton;
using Random = UnityEngine.Random;

public class MapManager : MonoSingleton<MapManager>
{
    [SerializeField] private Spawn spawn;
    [SerializeField] private PlayUI ui;
    
    public NeighborPos neighborPos;
    public GameConfig gameConfig;
    public Map map;
    public List<Block> WhatWillMove;
    public List<Vector3Int> WhereToMove;
    private List<Block> allBlockForCheckDir;
    private List<MapTile> canCreatPosList = new ();
    
    
    private void Start()
    {
        SettingMap();
        DrawDirectionOnBlock();
    }

    public bool FindIsThereCanPangBlock()
    {
        var canPangNum = 0;
        var mapTiles = map.MapTiles;
        mapTiles.Values.ForEach(mapTile =>
        {
            var nearBlockListNum = MapUtil.FindAllNearSameValue(mapTile.MovableBlockOnMapTile).Count;
            if ((mapTile.MovableBlockOnMapTile.value < 10 && nearBlockListNum > 1) || mapTile.MovableBlockOnMapTile.value is > 10 and < 40)
            {
                canPangNum++;
            }
        });

        if (canPangNum != 0)
        {
            return true;
        }
        
        return false;
    }


    public void AllPang()
    {
        var mapTiles = map.MapTiles;
        mapTiles.Values.ForEach(mapTile =>
        {
            if (mapTile.MovableBlockOnMapTile != null)
            {
                if (mapTile.MovableBlockOnMapTile.value < 40)
                {
                    mapTile.MovableBlockOnMapTile.PangMainBlock(mapTile.MovableBlockOnMapTile);
                }
                else
                {
                    mapTile.MovableBlockOnMapTile.Pang();
                }
                
            }
        });
    }
    
    
    public void AlreadyPangChange()
    {
        var mapTiles = map.MapTiles;
        mapTiles.Values.ForEach(mapTile =>
        {
            if (mapTile.MovableBlockOnMapTile.value is >= 60 and <= 63)
            {
                var box = mapTile.MovableBlockOnMapTile as BoxBlocker;
                if (box != null) box.alreadyPang = false;
            }
        });
    }
    
    private void SettingMap()
    {
        map = Instantiate(GetMapByStageNumber(gameConfig.StageLevel), transform.position, Quaternion.identity);
        map.MapTilePresetDataList.ForEach(x => { map.MapTiles.Add(x.Coord, x.MapTile); });
        //map.BlockNumber = gameConfig.BlockNumber;
        GameManager.Instance.State = States.ReadyForInteraction;
        spawn.SpawnBlockOnTile(map);
        ui.SettingUI(map);
    }

    public void ShuffleBlocks()
    {
        List<MapTile> normalTileList = new List<MapTile>();
        List<Block> normalBlockList = new List<Block>();
        var mapTiles = map.MapTiles;
        mapTiles.Values.ForEach(mapTile =>
        {
            if (mapTile.MovableBlockOnMapTile.value < 10)
            {
                normalTileList.Add(mapTile);
                normalBlockList.Add(mapTile.MovableBlockOnMapTile);
            }
        });
        var random = new System.Random();
        var shuffledBlock = normalTileList.OrderBy(x => random.Next()).ToList();

        for (int i = 0; i < normalBlockList.Count; i++)
        {
            normalBlockList[i].Move(shuffledBlock[i]);
        }
    }

    public void LastPangScaleAction(int howManyBlockNeedToCreate)
    {
        var mySequence = DOTween.Sequence();
        var mapTiles = map.MapTiles;
        mapTiles.Values.ForEach(mapTile =>
        {
            if (mapTile.MovableBlockOnMapTile.value < 10)
            {
                canCreatPosList.Add(mapTile);   
            }
        });

        if (howManyBlockNeedToCreate > canCreatPosList.Count)
        {
            for (int j = 0; j < canCreatPosList.Count; j++)
            {
                var randomTile = ValidRandomTileSelect(canCreatPosList);
                var lineBlock = spawn.SpawnRandomLineBlock(randomTile);
                mySequence.Join(lineBlock.transform.DOScale(new Vector3(1, 1, 0), 1f));
                canCreatPosList.Remove(randomTile);
            }
        }
        else
        {
            for (int i = 0; i < howManyBlockNeedToCreate; i++)
            {
                var randomTile = ValidRandomTileSelect(canCreatPosList);
                var lineBlock = spawn.SpawnRandomLineBlock(randomTile);
                mySequence.Join(lineBlock.transform.DOScale(new Vector3(1, 1, 0), 1f));
                canCreatPosList.Remove(randomTile);
            }
        }
        mySequence.OnComplete(ChangeScaleState);
    }

    public void LastPangBlock()
    {
        var mapTiles = map.MapTiles;
        mapTiles.Values.ForEach(mapTile =>
        {
            if (mapTile.MovableBlockOnMapTile != null)
            {
                if (mapTile.MovableBlockOnMapTile.value > 10)
                {
                    mapTile.MovableBlockOnMapTile.Pang();  
                }
            }
        });
    }
    
    private void ChangeScaleState()
    {
        GameManager.Instance.scaleIsDone = true;
    }

    private MapTile ValidRandomTileSelect(List<MapTile> validMapTiles)
    {
        var random = Random.Range(0, validMapTiles.Count);
        return validMapTiles[random];
    }
    
    private Map GetMapByStageNumber(int stageNumber)
    {
        var stageMap = Resources.Load<Map>($"Map{stageNumber}");

        if (stageMap != null)
        {
            return stageMap;
        }

        Debug.LogError($"There is no map of the stage requested {stageNumber}");
        
        return null;
    }
    
    public void MoveAllBlock()
    {
        var mapTiles = map.MapTiles;
        for (var i = 0; i < WhatWillMove.Count; i++)
        {
            WhatWillMove[i].MoveBlock(mapTiles[WhereToMove[i]]);
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
                    return true;
                }
            }
        }
        return false;
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
                List<Block> sameBlock = MapUtil.FindAllNearSameValue(allBlockForCheckDir[i]);
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
        
        
    }

    
    
    
    
    
}
