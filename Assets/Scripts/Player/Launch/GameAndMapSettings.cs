using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using System.Web;
using System;
using Photon.Pun.UtilityScripts;

public class GameAndMapSettings : MonoBehaviour
{
    string urlProtocolHostPort;
    public static RoomOptions roomOptions = new RoomOptions();
    private string team;
    private string playerName;
    private string gameId;
    public static string roomName;
    private string urlBeginning;

    void Start()
    {
        // Game and map settings from URL
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            string urlString = Application.absoluteURL;
            Uri uri = new Uri(urlString);
            string urlProtocolHostPort = uri.GetLeftPart(UriPartial.Authority);

            Debug.Log(urlProtocolHostPort);

            playerName = HttpUtility.ParseQueryString(uri.Query).Get("nick_name");
            team = HttpUtility.ParseQueryString(uri.Query).Get("team");
            gameId = HttpUtility.ParseQueryString(uri.Query).Get("game_id");
            roomName = HttpUtility.ParseQueryString(uri.Query).Get("room_name");

            urlBeginning = urlProtocolHostPort;
        }
        else
        {
            roomName = "RoomC1G1RoomTHASD";
            gameId = "82";
            urlBeginning = "localhost:3000";
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {         
                playerName = "Teacher";
                team = "Teacher";
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                playerName = "testPlayer";
                team = "Yellow";
            }
        }


        if (team == "Yellow")
        {
            PhotonNetwork.LocalPlayer.JoinTeam(1);
        }
        else if (team == "Blue")
        {
            PhotonNetwork.LocalPlayer.JoinTeam(2);
        }
        else if (team == "Red")
        {
            PhotonNetwork.LocalPlayer.JoinTeam(3);
        }
        else if (team == "Purple")
        {
            PhotonNetwork.LocalPlayer.JoinTeam(4);
        }
        else if (team == "Orange")
        {
            PhotonNetwork.LocalPlayer.JoinTeam(5);
        }
        else if (team == "Green")
        {
            PhotonNetwork.LocalPlayer.JoinTeam(6);
        }
        else if (team == "Teacher")
        {
            PhotonNetwork.LocalPlayer.JoinTeam(0);
        }

        PhotonNetwork.LocalPlayer.NickName = playerName;

        Debug.Log(PhotonNetwork.LocalPlayer.GetPhotonTeam().Name);

        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        // get id from the URL
        StartCoroutine(GetGameSettingsRequest(urlBeginning + "/games/" + gameId));
    }

    IEnumerator GetGameSettingsRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                    GameSettings gameSettings = GameSettings.CreateFromJSON(webRequest.downloadHandler.text);
                  //  Debug.Log(gameSettings.map_id);

                     StartCoroutine(GetMapSettingsRequest(urlBeginning + "/maps/show/" + gameSettings.map_id));
                   
                    roomOptions.CustomRoomProperties.Add("MovesPerRound", gameSettings.number_moves);
                    roomOptions.CustomRoomProperties.Add("SecondsPerRound", gameSettings.number_seconds);
                    roomOptions.CustomRoomProperties.Add("TotalNumberOfRounds", gameSettings.number_rounds);
                    roomOptions.CustomRoomProperties.Add("PointsPerToggle", gameSettings.points_toggle);
                    roomOptions.CustomRoomProperties.Add("PointsPerEndpoint", gameSettings.points_endpoint);
                    roomOptions.CustomRoomProperties.Add("TotalNumberOfSteals", gameSettings.number_steals);

                    break;
            }
        }
    }

    IEnumerator GetMapSettingsRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                  //  Debug.Log(webRequest.downloadHandler.text);

                    MapSettings mapSettings = MapSettings.CreateFromJSON(webRequest.downloadHandler.text);

                 //   Debug.Log(mapSettings.data);
                    roomOptions.CustomRoomProperties.Add("MapJSON", mapSettings.data);

                    //Map map = Map.CreateFromJSON(mapSettings.data);
                    //Debug.Log(map.tiles[0]);
                    //Debug.Log(map.positions[0]);

                    break;
            }
        }
    }
}


[System.Serializable]
public class GameSettings
{
    public int map_id;
    public int number_rounds;
    public int number_moves;
    public float number_seconds;
    public int number_steals;
    public int points_endpoint;
    public int points_toggle;

    public static GameSettings CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameSettings>(jsonString);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}

[System.Serializable]
public class MapSettings
{
    public int id;
    public string name;
    public string data;

    public static MapSettings CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<MapSettings>(jsonString);
    }
}

//[System.Serializable]
//public class Map
//{
//    public string name;
//    public string[] tiles;
//    public string[] positions;

//    public static Map CreateFromJSON(string jsonString)
//    {
//        return JsonUtility.FromJson<Map>(jsonString);
//    }
//}

