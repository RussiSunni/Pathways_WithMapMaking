using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine.SceneManagement;

public class MapLoadManager : MonoBehaviour
{
  //  public Tilemap tilemap;
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

    public void Start()
    {
        AssignMapName("test2");
        LoadMap();
    }
    public void AssignMapName (string mapName)
    {
        _mapName = mapName;               
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadMap();
    }
    public void LoadMap()
    {
        //string json = File.ReadAllText(Application.dataPath + "/Maps/" + _mapName + ".json");       

        string json = (string)CreateAndJoinRooms.roomOptions.CustomRoomProperties["MapJSON"];

        MapData data = JsonUtility.FromJson<MapData>(json);

        for (int i = 0; i < data.tiles.Count; i++)
        {
            Vector3 positionAdjustedForGrid = new Vector3(data.positions[i].x + 0.5f, data.positions[i].y + 0.5f, 0);

            if (data.tiles[i] == "HorizontalToggle")
            {              
                PhotonNetwork.InstantiateRoomObject("Toggle", positionAdjustedForGrid, Quaternion.identity, 0);
            }
            else if (data.tiles[i] == "VerticalToggle")
            {
                PhotonNetwork.InstantiateRoomObject("Toggle", positionAdjustedForGrid, transform.rotation * Quaternion.Euler(0f, 0f, 90f));
            }
            else if (data.tiles[i] == "StartPoint")
            {
                PhotonNetwork.InstantiateRoomObject("StartPoint", positionAdjustedForGrid, Quaternion.identity, 0);
            }
            else if (data.tiles[i] == "EndPoint")
            {
                PhotonNetwork.InstantiateRoomObject("EndPoint", positionAdjustedForGrid, Quaternion.identity, 0);
            }
            else if (data.tiles[i] == "Blocker")
            {
                Instantiate(_blocker, positionAdjustedForGrid, Quaternion.identity);
            }
        }
    }
}
