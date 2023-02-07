using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Spawn spawn;
    
    // public void ClickBlock()
    // {
    //     var plane = new Plane();
    //     
    //     plane.Set3Points(Vector3.zero, Vector3.up, Vector3.right);
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     if (plane.Raycast(ray, out var enter))
    //     {
    //         Vector3 hitPoint = ray.GetPoint(enter);
    //         var grid = Map.Instance.Tilemap.GetComponentInParent<Grid>();
    //         var cellCoord = grid.WorldToCell(hitPoint);
    //         //Map.Instance.DeleteBlock(Util.UnityCellToCube(cellCoord));
    //         var blockPos = Util.UnityCellToCube(cellCoord);
    //         var clickBlock = Map.Instance.BlockPlace[blockPos];
    //         var clickBlockNeighbor = Map.Instance.FindAllNearSameValue(clickBlock);
    //
    //         var tilePos = Util.CubeToUnityCell(blockPos);
    //         var putPos = grid.CellToWorld(tilePos);
    //         
    //         var block = clickBlock;
    //
    //
    //         if (clickBlock.value >= 20)
    //         {
    //             Map.Instance.DeleteAllDraw();
    //             
    //             if (clickBlock.value - 20 == 0)
    //             {
    //                 Map.Instance.DeleteLineBlock(blockPos, 0);
    //                 
    //             }
    //             else if (clickBlock.value - 20 == 1)
    //             {
    //                 Map.Instance.DeleteLineBlock(blockPos, 1);
    //                
    //             }
    //             else
    //             {
    //                 Map.Instance.DeleteLineBlock(blockPos, 2);
    //             }
    //             
    //         }
    //         else if (clickBlock.value > 10)
    //         {
    //             Map.Instance.DeleteAllDraw();
    //             var value = clickBlock.value - 10;
    //             Map.Instance.DeleteSameColor(blockPos, value);
    //         }
    //         
    //         else
    //         {
    //             if (clickBlock.specialValue != 0)
    //             {
    //                 if (clickBlock.specialValue == (int)Map.Instance.GameConfig.SpecialBlock2Condition)
    //                 {
    //                     Map.Instance.DeleteAllDraw();
    //                     
    //                     Map.Instance.DeleteBlockList(clickBlockNeighbor);
    //                     
    //                     spawn.SpawnSpecialTwoBlock(putPos);
    //                 }
    //             
    //                 else if (clickBlock.specialValue > 0)
    //                 {
    //                     var a = clickBlock.specialValue;
    //                     Map.Instance.DeleteAllDraw();
    //                     Map.Instance.DeleteBlockList(clickBlockNeighbor);
    //                     spawn.SpawnSpecialOneBlock(putPos,a);
    //                     Debug.Log(a + " 값");
    //                 }
    //                 else
    //                 {
    //                     Debug.Log("Nothing");
    //                 }
    //             }
    //         
    //             else
    //             {
    //                 if (clickBlockNeighbor.Count > 1)
    //                 {
    //                     Map.Instance.DeleteAllDraw();
    //                     Map.Instance.DeleteBlockList(clickBlockNeighbor);
    //                 
    //                 }
    //                 else
    //                 {
    //                     Debug.Log("한개짜리 못터뜨림");
    //                 }
    //             }
    //             
    //         }
    //         
    //         
    //     }
    // }
    
}
