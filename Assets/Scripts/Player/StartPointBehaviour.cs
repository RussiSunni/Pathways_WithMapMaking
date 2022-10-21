using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class StartPointBehaviour : MonoBehaviourPun
{
    private SpriteRenderer _sprite;
    private Color _yellowTeamConnectedColor = new Color(1, 1, 0, 1);
    private Color _blueTeamConnectedColor = new Color(0, 0, 1, 1);
    private Color _redTeamConnectedColor = new Color(1, 0, 0, 1);
    private Color _purpleTeamConnectedColor = new Color(1, 0, 1, 1);
    private Color _orangeTeamConnectedColor = new Color(1, 0.65f, 0, 1);
    private Color _greenTeamConnectedColor = new Color(0, 1, 0, 1);
    private Color _disconnectedColor = new Color(1, 1, 1, 1); 
    private bool isOwnedByTeam = false;
    public GameObject[] toggles;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        toggles = GameObject.FindGameObjectsWithTag("Toggle");
    }
    private void OnMouseOver()
    {
        if (!GameManager.IsGameEnded)
        {
            // Check if the teacher.
            if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name != "Teacher")
            {
                if (!isOwnedByTeam)
                {
                    base.photonView.RequestOwnership();

                    if (Input.GetMouseButtonDown(0))
                    {                     
                        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Yellow")
                        {
                            photonView.RPC("ChangeColour", RpcTarget.All, "Yellow Team");
                        }
                        else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Blue")
                        {
                            photonView.RPC("ChangeColour", RpcTarget.All, "Blue Team");
                        }
                        else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Red")
                        {
                            photonView.RPC("ChangeColour", RpcTarget.All, "Red Team");
                        }
                        else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Purple")
                        {
                            photonView.RPC("ChangeColour", RpcTarget.All, "Purple Team");
                        }
                        else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Orange")
                        {
                            photonView.RPC("ChangeColour", RpcTarget.All, "Orange Team");
                        }
                        else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Green")
                        {
                            photonView.RPC("ChangeColour", RpcTarget.All, "Green Team");
                        }
                        else
                        {
                            Debug.Log("Team not known");
                        }

                        // So it can only be claimed once.
                        isOwnedByTeam = true;

                        // Reduce turns in round remaining for all team members.                       
                        photonView.RPC("ReduceTurnsRemaining", RpcTarget.All, PhotonNetwork.LocalPlayer.GetPhotonTeam().Name);

                        // Update the toggles.
                        toggles[0].GetComponent<ToggleBehaviour>().UpdateTogglesRPC();                        
                    }               
                }
            }
        }
    }
  

    [PunRPC]
    void ChangeColour(string team)
    {
        if (team == "Yellow Team")
        {
            _sprite.color = _yellowTeamConnectedColor;
            gameObject.tag = "Yellow Team StartPoint";
        }
        else if (team == "Blue Team")
        {
            _sprite.color = _blueTeamConnectedColor;
            gameObject.tag = "Blue Team StartPoint";
        }
        else if (team == "Red Team")
        {
            _sprite.color = _redTeamConnectedColor;
            gameObject.tag = "Red Team StartPoint";
        }
        else if (team == "Purple Team")
        {
            _sprite.color = _purpleTeamConnectedColor;
            gameObject.tag = "Purple Team StartPoint";
        }
        else if (team == "Orange Team")
        {
            _sprite.color = _orangeTeamConnectedColor;
            gameObject.tag = "Orange Team StartPoint";
        }
        else if (team == "Green Team")
        {
            _sprite.color = _greenTeamConnectedColor;
            gameObject.tag = "Green Team StartPoint";
        }

        isOwnedByTeam = true;        
    }

    [PunRPC]
    void ReduceTurnsRemaining(string team)
    {    
            if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == team)
            {
                GameManager.MovesInRoundRemaining--;
            }        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.tag);
        }
        else if (stream.IsReading)
        {
            gameObject.tag = (string)stream.ReceiveNext();
        }
    }
}
