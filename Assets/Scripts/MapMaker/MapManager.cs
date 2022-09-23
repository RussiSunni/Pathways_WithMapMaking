using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Runtime.InteropServices;

public class MapManager : MonoBehaviour
{  
    [DllImport("__Internal")]
    private static extern void ExportMapJSON(string mapJSON);

    public static MapManager instance;
    public List<CustomTile> tiles = new List<CustomTile>();
    private string _mapName;
   // public TileBase testTile;

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

    public Tilemap tilemap;

    private void Update()
    {
        //if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
        //{
        //    SaveMap();
        //}
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            LoadMap();
        }
    }

    public void SaveMap()
    {
        Debug.Log("map saved");
        
        BoundsInt bounds = tilemap.cellBounds;

        MapData mapData = new MapData();

        mapData.name = _mapName;
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                TileBase temp =  tilemap.GetTile(new Vector3Int(x, y, 0));
                CustomTile tempTile = tiles.Find(t => t.tile == temp);

                if (tempTile != null)
                {
                    mapData.tiles.Add(tempTile.id);
                    mapData.positions.Add(new Vector3Int(x, y, 0));
                }
            }
        }

        string json = JsonUtility.ToJson(mapData, false);

        Debug.Log(json);
                
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            File.WriteAllText(Application.dataPath + "/Maps/" + _mapName + ".json", json);
        }
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {        
            ExportMapJSON(json);           
        }
    }

    void LoadMap()
    {
        string json = File.ReadAllText(Application.dataPath + "/Maps/" + _mapName + ".json");
        //string json = File.ReadAllText(Application.dataPath + "/testMap.json");
        Debug.Log(json);

        MapData data = JsonUtility.FromJson<MapData>(json);

        tilemap.ClearAllTiles();

      //  tilemap.SetTile(data.positions[0], testTile);

        for (int i = 0; i < data.tiles.Count; i++)
        {
            //tilemap.SetTile(data.positions[i], tiles.Find(t => t.name == data.tiles[i]).tile);

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

