using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapEditor : MonoBehaviour
{
    [SerializeField]
    Tilemap currentTileMap;

    [SerializeField]
    TileBase currentTile;

    [SerializeField]
    TileBase horToggleTile;

    [SerializeField]
    TileBase verToggleTile;

    [SerializeField]
    Camera cam;

    private void Update()
    {
        Vector3Int pos = currentTileMap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButton(0))
        {
            PlaceTile(pos);
        }
        if (Input.GetMouseButton(1))
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


}
