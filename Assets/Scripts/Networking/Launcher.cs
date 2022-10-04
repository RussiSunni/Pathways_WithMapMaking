using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
using System.Web;
using System;
using Photon.Pun.UtilityScripts;
using System.IO;
public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject controlPanel;

    [SerializeField]
    private Text feedbackText;
    private bool isConnecting;

    [SerializeField]
    private byte maxPlayersPerRoom = 4;
    string gameVersion = "1";

    public Text playerStatus;
    public Text connectionStatus;

    [SerializeField]
    private CanvasGroup readyButtonCanvasGroup;

    [SerializeField]
    private CanvasGroup startButtonCanvasGroup;

    string playerName = "";
    string roomName = "";

    int totalNumberOfRounds = 2;
    int movesPerRound = 10;
    float secondsPerRound = 20;
    int pointsPerToggle = 1;
    int pointsPerEndPoint = 50;
    int totalNumberOfSteals = 6;
    string mapJSON;

    // Make the room options a global variable.
    public static RoomOptions roomOptions = new RoomOptions();

    void Start()
    { 
        PlayerPrefs.DeleteAll();
        Debug.Log("Connecting to Photon Network");

        ConnectToPhoton();

        Debug.Log(Application.platform);

        // Get team name.
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            string urlString = Application.absoluteURL;
            Uri uri = new Uri(urlString);
            string nick_name = HttpUtility.ParseQueryString(uri.Query).Get("nick_name");
            string team = HttpUtility.ParseQueryString(uri.Query).Get("team");
            string roundsString = HttpUtility.ParseQueryString(uri.Query).Get("rounds");
            string movesString = HttpUtility.ParseQueryString(uri.Query).Get("moves");
            string secondsString = HttpUtility.ParseQueryString(uri.Query).Get("seconds");
            string pointsPerToggleString = HttpUtility.ParseQueryString(uri.Query).Get("toggle_points");
            string pointsPerEndPointString = HttpUtility.ParseQueryString(uri.Query).Get("endpoint_points");
            string totalNumberOfStealsString = HttpUtility.ParseQueryString(uri.Query).Get("steals");
            mapJSON = HttpUtility.ParseQueryString(uri.Query).Get("map");

            totalNumberOfRounds = int.Parse(roundsString);
            movesPerRound = int.Parse(movesString);
            secondsPerRound = float.Parse(secondsString);
            pointsPerToggle = int.Parse(pointsPerToggleString);
            pointsPerEndPoint = int.Parse(pointsPerEndPointString);
            totalNumberOfSteals = int.Parse(totalNumberOfStealsString);

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
                startButtonCanvasGroup.alpha = 1;
                startButtonCanvasGroup.interactable = true;

                readyButtonCanvasGroup.alpha = 0;
                readyButtonCanvasGroup.interactable = false;
            }
            playerName = nick_name;                
        }   
        
        // for testing/development
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            playerName = "tom";
            PhotonNetwork.LocalPlayer.JoinTeam(1);

            mapJSON = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Maps/" + "test3" + ".json");

        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            playerName = "sally";           
           
            PhotonNetwork.LocalPlayer.JoinTeam(2);
            //startButtonCanvasGroup.alpha = 1;
            //startButtonCanvasGroup.interactable = true;

            //readyButtonCanvasGroup.alpha = 0;
            //readyButtonCanvasGroup.interactable = false;

            mapJSON = File.ReadAllText(Application.dataPath + "/Maps/" + "test3" + ".json");
        }

        PhotonNetwork.LocalPlayer.NickName = playerName;

        // Total moves per round setting.
        // Preparing the room options.
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CustomRoomProperties.Add("MovesPerRound", movesPerRound);
        roomOptions.CustomRoomProperties.Add("SecondsPerRound", secondsPerRound);
        roomOptions.CustomRoomProperties.Add("TotalNumberOfRounds", totalNumberOfRounds);
        roomOptions.CustomRoomProperties.Add("PointsPerToggle", pointsPerToggle);
        roomOptions.CustomRoomProperties.Add("PointsPerEndpoint", pointsPerEndPoint);
        roomOptions.CustomRoomProperties.Add("TotalNumberOfSteals", totalNumberOfSteals);
        roomOptions.CustomRoomProperties.Add("MapJSON", mapJSON);      
    }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void ConnectToPhoton()
    {
        connectionStatus.text = "Connecting...";
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("PhotonNetwork.IsConnected! | Trying to Create/Join Room");
            TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default);
            PhotonNetwork.JoinOrCreateRoom("Pathways", roomOptions, typedLobby);
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");
        base.OnConnectedToMaster();       
    }

    public void LoadArena()
    {
        PhotonNetwork.LoadLevel("Main");
    }

    // Photon Methods
    public override void OnConnected()
    {
        base.OnConnected();

        connectionStatus.text = "Connected";
        connectionStatus.color = Color.green;
        readyButtonCanvasGroup.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        controlPanel.SetActive(true);
        Debug.LogError("Disconnected. Please check your Internet connection.");
    }

    public override void OnJoinedRoom()
    {
       LoadArena();
    }
}
