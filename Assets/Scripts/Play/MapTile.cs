using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapTile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Interaction interaction;
    
    public Vector3Int MapTileCoord;
    public Block MovableBlockOnMapTile;
    public Block UnMovalbleBlockOnMapTile;

    private void Start()
    {
        interaction = GameManager.Instance.interaction;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(MapTileCoord);
        if (GameManager.Instance.State == States.ReadyForInteraction)
        {
            interaction.OnTileClickHandler(MapTileCoord);
        }
        
    }
    
}
