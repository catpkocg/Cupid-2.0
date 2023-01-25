using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public Transform backGround;
    public Tilemap tilemap;
    public List<Vector3> availablePlaces = new List<Vector3>();
    public Camera cam;
    private void Awake()
    {
        var tile = Instantiate(tilemap, transform.position, Quaternion.identity);
        tile.transform.SetParent(backGround);
        tilemap = tile;
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
                Vector3Int localPlace = new Vector3Int(n, p, (int)tilemap.transform.position.y);
                Vector3 place = tilemap.CellToWorld(localPlace);
                if (tilemap.HasTile(localPlace))
                {
                    availablePlaces.Add(place);
                    Debug.Log(n + "da"+p);
                }
            }
        }
        cam.transform.GetComponent<Camera>().orthographicSize = tilemap.cellBounds.xMax - tilemap.cellBounds.xMin;

    }

    private void SpawnBlockOnTile()
    {
        for (int i = 0; i < availablePlaces.Count; i++)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HitPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var plane = new Plane();
            plane.Set3Points(Vector3.zero, Vector3.up, Vector3.right);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out var enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                var grid = tilemap.GetComponentInParent<Grid>();
                var cellCoord = grid.WorldToCell(hitPoint);
                Debug.Log(cellCoord);
            }
        }
    }
}
