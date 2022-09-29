using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Runtime.InteropServices;
using System;
using System.Web;

public class MapManager : MonoBehaviour
{  
    [DllImport("__Internal")]
    private static extern void ExportMapJSON(string mapJSON);

    public static MapManager instance;
    public List<CustomTile> tiles = new List<CustomTile>();
    private string _mapName;
    private string _mapJSON = "";

    public Tilemap squareTileMap;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            string urlString = Application.absoluteURL;
            Uri uri = new Uri(urlString);
            _mapJSON = HttpUtility.ParseQueryString(uri.Query).Get("map");

            if (_mapJSON != "")
            {
                LoadMap(_mapJSON);
            }
        }
    }

    public void SaveMap()
    {
        Debug.Log("map saved");
        
        // Get bounds of both tilemaps
        BoundsInt bounds = squareTileMap.cellBounds;        

        // Get data for the tilemap.
        MapData squareTileMapData = new MapData();

        squareTileMapData.name = _mapName;
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                TileBase temp = squareTileMap.GetTile(new Vector3Int(x, y, 0));
                CustomTile tempTile = tiles.Find(t => t.tile == temp);

                if (tempTile != null)
                {
                    squareTileMapData.tiles.Add(tempTile.id);
                    squareTileMapData.positions.Add(new Vector3Int(x, y, 0));
                }
            }
        }       

        string json = JsonUtility.ToJson(squareTileMapData, false);
                
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            File.WriteAllText(Application.dataPath + "/Maps/" + _mapName + ".json", json);
        }
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {        
            ExportMapJSON(json);           
        }
    }

    public void LoadMap(string mapJSON)
    {
        string json = "";

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            json = mapJSON;
        }
        // For testing only.
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            json = File.ReadAllText(Application.dataPath + "/maps/" + "test10" + ".json");
        }          
      
        MapData data = JsonUtility.FromJson<MapData>(json);

        squareTileMap.ClearAllTiles();

        for (int i = 0; i < data.tiles.Count; i++)
        {
            //squareTileMap.SetTile(data.positions[i], tiles.Find(t => t.name == data.tiles[i]).tile);
            //
            Debug.Log(data.tiles[i]);
            int tileListNum = 0;

            if (data.tiles[i] == "HorizontalToggle")
            {
                tileListNum = 0;
            }
            else if (data.tiles[i] == "VerticalToggle")
            {
                tileListNum = 1;
            }
            else if (data.tiles[i] == "StartPoint")
            {
                tileListNum = 2;
            }
            else if (data.tiles[i] == "EndPoint")
            {
                tileListNum = 3;
            }
            else if (data.tiles[i] == "Blocker")
            {
                tileListNum = 4;
            }

            squareTileMap.SetTile(data.positions[i], tiles[tileListNum].tile);
        }
    }

    public void AssignMapName(string mapName)
    {
        _mapName = mapName;      
    }
}

public class MapData
{
    public string name;
    public List<string> tiles = new List<string>();
    public List<Vector3Int> positions = new List<Vector3Int>();
}


