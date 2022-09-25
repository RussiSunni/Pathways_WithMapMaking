using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class MapEditor : MonoBehaviour
{
    Tilemap currentTileMap;

    [SerializeField]
    Tilemap squareTileMap;
        
    [SerializeField]
    TileBase currentTile;

    [SerializeField]
    TileBase horToggleTile;

    [SerializeField]
    TileBase verToggleTile;

    [SerializeField]
    TileBase startPointTile;

    [SerializeField]
    TileBase endPointTile;

    [SerializeField]
    TileBase blockerTile;

    [SerializeField]
    Camera cam;

    private void Start()
    {
        currentTileMap = squareTileMap;
    }

    private void Update()
    {
        Vector3Int pos = currentTileMap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            PlaceTile(pos);      
        }
        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            DeleteTile(pos);
        }
    }

    void PlaceTile(Vector3Int pos)
    {       
        currentTileMap.SetTile(pos, currentTile);
    }

    void DeleteTile(Vector3Int pos)
    {
        currentTileMap.SetTile(pos, null);       
    }

    public void ChooseHorizontalTile()
    {
        currentTile = horToggleTile;
    }

    public void ChooseVerticalTile()
    {
        currentTile = verToggleTile;
    }

    public void ChooseStartPointTile()
    {
        currentTile = startPointTile;
    }

    public void ChooseEndPointTile()
    {
        currentTile = endPointTile;
    }

    public void ChooseBlockerTile()
    {
        currentTile = blockerTile;
    }
}
