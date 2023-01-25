using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HitPoint();
        }
    }
    
    private void HitPoint()
    {
        var plane = new Plane();
        plane.Set3Points(Vector3.zero, Vector3.up, Vector3.right);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out var enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
            var cellCoord = grid.WorldToCell(hitPoint);
            Debug.Log(hitPoint);
            Debug.Log(cellCoord);
        }
    }
    
}
