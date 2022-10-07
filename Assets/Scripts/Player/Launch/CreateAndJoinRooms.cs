using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using Photon.Pun.UtilityScripts;
using System;
using System.Web;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    // Make the room options a global variable.
    public static RoomOptions roomOptions = new RoomOptions();

    public CanvasGroup createRoomCanvasGroup;
    public CanvasGroup joinRoomCanvasGroup;
    public Image joinRoomButtonImage;
    public CanvasGroup startGameCanvasGroup;
    public CanvasGroup roomNotCreatedYetTextCanvasGroup;
    public CanvasGroup playerListHeadingTextCanvasGroup;
    public Text playerListText;
    List<string> playerList = new List<string>();

    string roomName;
    string playerName = "";

    // Room options for testing.
    int totalNumberOfRounds = 2;
    int movesPerRound = 30;
    float secondsPerRound = 1000;
    int pointsPerToggle = 1;
    int pointsPerEndPoint = 50;
    int totalNumberOfSteals = 6;
    string mapJSON;

    // Countdown timer.
    bool isCountDownStarted;
    float timeRemaining = 10;
    public Text _timeRemainingText;

    private void Start()
    {
        // Preparing the room options ------------------

        // For testing only -------------
        roomName = "test room";

        // Get the JSON for the map, for the room options.
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            playerName = "tom";
            PhotonNetwork.LocalPlayer.JoinTeam(0);
            mapJSON = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Maps/" + "test6" + ".json");           
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            playerName = "sally";
            mapJSON = File.ReadAllText(Application.dataPath + "/Maps/" + "test6" + ".json");
            PhotonNetwork.LocalPlayer.JoinTeam(1);
        }

        // --------------------

        // For real application ---------------------

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
            }
            playerName = nick_name;
        }

        // -----------------

        // Player name.
        PhotonNetwork.LocalPlayer.NickName = playerName;

        // The other room options.
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CustomRoomProperties.Add("MovesPerRound", movesPerRound);
        roomOptions.CustomRoomProperties.Add("SecondsPerRound", secondsPerRound);
        roomOptions.CustomRoomProperties.Add("TotalNumberOfRounds", totalNumberOfRounds);
        roomOptions.CustomRoomProperties.Add("PointsPerToggle", pointsPerToggle);
        roomOptions.CustomRoomProperties.Add("PointsPerEndpoint", pointsPerEndPoint);
        roomOptions.CustomRoomProperties.Add("TotalNumberOfSteals", totalNumberOfSteals);
        roomOptions.CustomRoomProperties.Add("MapJSON", mapJSON);


        // Only show the create room button for the teacher.
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Teacher")
        {
            createRoomCanvasGroup.alpha = 1;
            createRoomCanvasGroup.interactable = true;
            joinRoomCanvasGroup.alpha = 0;
            joinRoomCanvasGroup.interactable = false;
            playerListHeadingTextCanvasGroup.alpha = 1;
            playerListText.GetComponent<CanvasGroup>().alpha = 1;
        }
        else 
        {
            createRoomCanvasGroup.alpha = 0;
            createRoomCanvasGroup.interactable = false;
            joinRoomCanvasGroup.alpha = 1;
            joinRoomCanvasGroup.interactable = true;
            playerListHeadingTextCanvasGroup.alpha = 0;
            playerListText.GetComponent<CanvasGroup>().alpha = 0;
        }

        startGameCanvasGroup.alpha = 0;
        startGameCanvasGroup.interactable = false;
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }  

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        StartCoroutine(ExecuteAfterTime(2));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        roomNotCreatedYetTextCanvasGroup.alpha = 1;
        yield return new WaitForSeconds(time);

        roomNotCreatedYetTextCanvasGroup.alpha = 0;
    }

    public override void OnJoinedRoom()
    {     
        photonView.RPC("ReadyNotificationRPC", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.NickName);

        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Teacher")
        {
            startGameCanvasGroup.alpha = 1;
            startGameCanvasGroup.interactable = true;
        }
        else
        {
            joinRoomButtonImage.color = Color.green;
        }
        base.OnJoinedRoom();       
    }

    public void StartGame()
    {
        photonView.RPC("StartGameRPC", RpcTarget.All);
    }

    [PunRPC]
    void ReadyNotificationRPC(string playerName)
    {     
        playerList.Add(playerName);
        string playerListString = "";
           
        for (int i = 0; i < playerList.Count; i++)
        {
            playerListString = playerListString + System.Environment.NewLine + playerList[i];
        }
          
        playerListText.text = playerListString;       
    }

    [PunRPC]
    void DecreaseTimeRemainingRPC(float timeRemaining)
    {
        float roundedTimeRemaining = UnityEngine.Mathf.Round(timeRemaining);
        _timeRemainingText.text = roundedTimeRemaining.ToString();
    }

    [PunRPC]
    void StartGameRPC()
    {
        PhotonNetwork.LoadLevel("Main");
    }

    void Update()
    {
        // Time remaining
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (isCountDownStarted)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    photonView.RPC("DecreaseTimeRemainingRPC", RpcTarget.All, timeRemaining);
                }
                else
                {
                    
                }
            }     
        }        
    }
}
