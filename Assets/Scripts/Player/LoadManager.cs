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
                tilemap.SetTile(data.positions[i], HorizontalToggleTile);
            }
            else if (data.tiles[i] == "VerticalToggle")
            {
                tilemap.SetTile(data.positions[i], VerticalToggleTile);
            }
            //tilemap.SetTile(data.positions[i], testTile1);
            //tilemap.SetTile(data.positions[i], tiles.Find(t => t.name == data.tiles[i]).tile);
        }
    }
}
