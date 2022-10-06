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
    public CanvasGroup roomNotCreatedYetTextCanvasGroup;

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
            mapJSON = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Maps/" + "test3" + ".json");
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            playerName = "sally";
            mapJSON = File.ReadAllText(Application.dataPath + "/Maps/" + "test3" + ".json");
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
        }
        else 
        {
            createRoomCanvasGroup.alpha = 0;
            createRoomCanvasGroup.interactable = false;
            joinRoomCanvasGroup.alpha = 1;
            joinRoomCanvasGroup.interactable = true;
        }
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
        roomNotCreatedYetTextCanvasGroup.alpha = 1;
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Main");
        //base.OnJoinedRoom();       
    }  
}
