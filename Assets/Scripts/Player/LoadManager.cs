using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class LoadManager : MonoBehaviour
{
    public Tilemap tilemap;
  //  public List<CustomTile> tiles = new List<CustomTile>();
    private string _mapName;
   // public TileBase HorizontalToggleTile;
  //  public TileBase VerticalToggleTile;

    [SerializeField]
    private GameObject _toggle;
    [SerializeField]
    private GameObject _startPoint;
    [SerializeField]
    private GameObject _endPoint;
    [SerializeField]
    private GameObject _blocker;


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
            Vector3 positionAdjustedForGrid = new Vector3(data.positions[i].x + 0.5f, data.positions[i].y + 0.5f, 0);

            if (data.tiles[i] == "HorizontalToggle")
            {              
                Instantiate(_toggle, positionAdjustedForGrid, Quaternion.identity);
            }
            else if (data.tiles[i] == "VerticalToggle")
            {
                Instantiate(_toggle, positionAdjustedForGrid, transform.rotation * Quaternion.Euler(0f, 0f, 90f));
            }
            else if (data.tiles[i] == "StartPoint")
            {
                Instantiate(_startPoint, positionAdjustedForGrid, Quaternion.identity);
            }
            else if (data.tiles[i] == "EndPoint")
            {
                Instantiate(_endPoint, positionAdjustedForGrid, Quaternion.identity);
            }
            else if (data.tiles[i] == "Blocker")
            {
                Instantiate(_blocker, positionAdjustedForGrid, Quaternion.identity);
            }
        }
    }
}
