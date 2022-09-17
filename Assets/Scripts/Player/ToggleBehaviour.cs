using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class ToggleBehaviour : MonoBehaviourPun, IPunObservable
{
    private SpriteRenderer _sprite;
    private Color _yellowTeamConnectedColor = new Color(1, 1, 0, 1);
    private Color _blueTeamConnectedColor = new Color(0, 0, 1, 1);
    private Color _redTeamConnectedColor = new Color(1, 0, 0, 1);
    private Color _purpleTeamConnectedColor = new Color(1, 0, 1, 1);
    private Color _orangeTeamConnectedColor = new Color(1, 0.65f, 0, 1);
    private Color _greenTeamConnectedColor = new Color(0, 1, 0, 1);
    private Color _disconnectedColor = new Color(1, 1, 1, 1);
    private Transform _originalParent;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();      
        _originalParent = transform.parent;
    }

    private void OnMouseOver()
    {
        if (GameManager.MovesInRoundRemaining > 0 && GameManager.SecondsInRoundRemaining > 0)
        {
            // Check if the teacher.
            if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name != "Teacher")
            {
                base.photonView.RequestOwnership();

                // Check if the player has ownership of this object. If they dont, they cant move it.        
                if (photonView.IsMine)
                {
                    // Rotate toggle on mouse click.
                    if (Input.GetMouseButtonDown(0))
                    {
                        int numChildren = transform.childCount;

                        for (int i = 0; i < numChildren; i++)
                        {
                            if (numChildren > i)
                            {
                                transform.GetChild(i).parent = _originalParent;
                            }
                        }

                        transform.parent = _originalParent;
                        _sprite.color = _disconnectedColor;

                        transform.eulerAngles = Vector3.forward * (transform.eulerAngles.z - 90);

                        //GameManager.MovesInRoundRemaining--;
                        // Reduce turns in round remaining for all team members.                       
                        photonView.RPC("ReduceTurnsRemaining", RpcTarget.All, PhotonNetwork.LocalPlayer.GetPhotonTeam().Name);
                    }
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Yellow Team StartPoint")
        {
            _sprite.color = _yellowTeamConnectedColor;
            gameObject.transform.parent = collision.transform;           
        }
        else if (collision.transform.root.tag == "Yellow Team StartPoint")
        {
            _sprite.color = _yellowTeamConnectedColor;
            gameObject.transform.parent = collision.transform;
        }   
        else if (collision.gameObject.tag == "Blue Team StartPoint")
        {
            _sprite.color = _blueTeamConnectedColor;
            gameObject.transform.parent = collision.transform;           
        }
        else if (collision.transform.root.tag == "Blue Team StartPoint")
        {
            _sprite.color = _blueTeamConnectedColor;
            gameObject.transform.parent = collision.transform;
        }
        else if (collision.gameObject.tag == "Red Team StartPoint")
        {
            _sprite.color = _redTeamConnectedColor;
            gameObject.transform.parent = collision.transform;
        }
        else if (collision.transform.root.tag == "Red Team StartPoint")
        {
            _sprite.color = _redTeamConnectedColor;
            gameObject.transform.parent = collision.transform;
        }
        else if (collision.gameObject.tag == "Purple Team StartPoint")
        {
            _sprite.color = _purpleTeamConnectedColor;
            gameObject.transform.parent = collision.transform;
        }
        else if (collision.transform.root.tag == "Purple Team StartPoint")
        {
            _sprite.color = _purpleTeamConnectedColor;
            gameObject.transform.parent = collision.transform;
        }
        else if (collision.gameObject.tag == "Orange Team StartPoint")
        {
            _sprite.color = _orangeTeamConnectedColor;
            gameObject.transform.parent = collision.transform;
        }
        else if (collision.transform.root.tag == "Orange Team StartPoint")
        {
            _sprite.color = _orangeTeamConnectedColor;
            gameObject.transform.parent = collision.transform;
        }
        else if (collision.gameObject.tag == "Green Team StartPoint")
        {
            _sprite.color = _greenTeamConnectedColor;
            gameObject.transform.parent = collision.transform;
        }
        else if (collision.transform.root.tag == "Green Team StartPoint")
        {
            _sprite.color = _greenTeamConnectedColor;
            gameObject.transform.parent = collision.transform;
        }
        else
        {
            _sprite.color = _disconnectedColor;
        }  
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _sprite.color = _disconnectedColor;
    }

    // For other clients.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading)
        {
            int numChildren = transform.childCount;

            for (int i = 0; i < numChildren; i++)
            {
                transform.GetChild(i).parent = _originalParent;
            }

            transform.parent = _originalParent;
            _sprite.color = _disconnectedColor;

            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void ReduceTurnsRemaining(string team)
    {
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == team)
        {
            GameManager.MovesInRoundRemaining--;
        }
    }
}
