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
    public CanvasGroup readyCanvasGroup;
    public Image readyButtonImage;
    public CanvasGroup startGameCanvasGroup;
    public CanvasGroup playerListHeadingTextCanvasGroup;
    public Text playerListText;
    List<string> playerList = new List<string>();

    public CanvasGroup roomNotReadyYetTextCanvasGroup;
    public Text roomNameText;

    // Countdown timer.
    bool isCountDownStarted;
    float timeRemaining = 10;
    public Text _timeRemainingText;
    bool isLoadLevelCalled;

    private void Start()
    {  
        // Only show the create room button for the teacher.
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Teacher")
        {
            //createRoomCanvasGroup.alpha = 1;
            //createRoomCanvasGroup.interactable = true;
            readyCanvasGroup.alpha = 0;
            readyCanvasGroup.interactable = false;
            playerListHeadingTextCanvasGroup.alpha = 1;
            playerListText.GetComponent<CanvasGroup>().alpha = 1;
            startGameCanvasGroup.alpha = 1;
            startGameCanvasGroup.interactable = true;

            PhotonNetwork.CreateRoom(GameAndMapSettings.roomName, GameAndMapSettings.roomOptions);
        }
        else 
        {
            //createRoomCanvasGroup.alpha = 0;
            //createRoomCanvasGroup.interactable = false;
            readyCanvasGroup.alpha = 1;
            readyCanvasGroup.interactable = true;
            playerListHeadingTextCanvasGroup.alpha = 0;
            playerListText.GetComponent<CanvasGroup>().alpha = 0;
            startGameCanvasGroup.alpha = 0;
            startGameCanvasGroup.interactable = false;
        }      

        //PhotonNetwork.JoinOrCreateRoom(roomName, GameAndMapSettings.roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {        
        roomNameText.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
    }

    public void Ready()
    {
        PhotonNetwork.JoinRoom(GameAndMapSettings.roomName);    
    }

    public override void OnJoinedRoom()
    {
        roomNameText.text = "Room: " + PhotonNetwork.CurrentRoom.Name;

        readyButtonImage.color = Color.green;
        photonView.RPC("ReadyNotificationRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
        readyCanvasGroup.interactable = false;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Joining room failed.");
        Debug.Log(message);

        roomNotReadyYetTextCanvasGroup.alpha = 1;
        StartCoroutine(ExecuteAfterTime());
    }

    IEnumerator ExecuteAfterTime()
    {
        yield return new WaitForSeconds(2);
        roomNotReadyYetTextCanvasGroup.alpha = 0;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!readyCanvasGroup.interactable)
        {
            if (newPlayer.NickName == "Teacher")
            {
                photonView.RPC("ReadyNotificationRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
            }
        }     
    }  

    public void StartGame()
    {
        isCountDownStarted = true;
    }

    [PunRPC]
    void ReadyNotificationRPC(string playerName)
    {
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Teacher")
        {
            playerList.Add(playerName);
            string playerListString = "";

            for (int i = 0; i < playerList.Count; i++)
            {
                playerListString = playerListString + System.Environment.NewLine + playerList[i];
            }

            playerListText.text = playerListString;
        }
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
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Teacher")
        {
            if (isCountDownStarted)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    photonView.RPC("DecreaseTimeRemainingRPC", RpcTarget.All, timeRemaining);
                }
                else if (!isLoadLevelCalled)
                {
                    isLoadLevelCalled = true;
                    photonView.RPC("StartGameRPC", RpcTarget.All);
                }
            }     
        }        
    }
}
