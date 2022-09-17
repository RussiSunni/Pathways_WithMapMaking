using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class LoadManager : MonoBehaviour
{
    public Tilemap tilemap;
    public List<CustomTile> tiles = new List<CustomTile>();
    private string _mapName;
    public TileBase HorizontalToggleTile;
    public TileBase VerticalToggleTile;
    public GameObject toggle;

    public void AssignMapName (string mapName)
    {
        _mapName = mapName;       
    }
    public void LoadMap()
    {
        string json = File.ReadAllText(Application.dataPath + "/Maps/" + _mapName + ".json");       

        MapData data = JsonUtility.FromJson<MapData>(json);
     
        tilemap.ClearAllTiles();

        for (int i = 0; i < data.tiles.Count; i++)
        {          
            if (data.tiles[i] == "HorizontalToggle")
            {              
                Instantiate(toggle, data.positions[i], Quaternion.identity);
            }
            else if (data.tiles[i] == "VerticalToggle")
            {
                Instantiate(toggle, data.positions[i], transform.rotation * Quaternion.Euler(0f, 0f, 90f));
            }           
        }
    }
}
