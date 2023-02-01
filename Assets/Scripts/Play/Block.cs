using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    
    [SerializeField] public int value;

    public int specialValue = 0;
    
    public List<GameObject> dir;
    // public GameObject dirX;
    // public GameObject dirY;
    // public GameObject dirZ;
    public GameObject foot;
    public GameObject boxBlock;
    
    public Vector3Int Coord { get; set; }

    
    public int score = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pang()
    {
        OnPang();
    }
    
    public abstract void OnSpawn();
    public abstract void OnPang();
}
