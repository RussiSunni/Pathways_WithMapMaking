using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.IO;

public class MapManager : MonoBehaviour
{
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
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
        {
            SaveMap();
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            LoadMap();
        }
    }

    void SaveMap()
    {
        Debug.Log("map saved");
        
        BoundsInt bounds = tilemap.cellBounds;

        MapData mapData = new MapData();

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

        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(Application.dataPath + "/Maps/" + _mapName + ".json", json);        
        //File.WriteAllText(Application.dataPath + "/testMap.json", json);
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
    public List<string> tiles = new List<string>();
    public List<Vector3Int> positions = new List<Vector3Int>();
}


