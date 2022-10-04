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
    List<Color> _teamColours = new List<Color>();
    private Color _disconnectedColor = new Color(1, 1, 1, 1);
    private Transform _originalParent;

    public GameObject[] toggles;  
    public GameObject[] yellowTeamStartPoints;
    public GameObject[] blueTeamStartPoints;
    public GameObject[] redTeamStartPoints;
    public GameObject[] purpleTeamStartPoints;
    public GameObject[] orangeTeamStartPoints;
    public GameObject[] greenTeamStartPoints;
    List<GameObject[]> _teamStartPoints = new List<GameObject[]>();
    List<string> _startPointTagNames = new List<string>();
    Collider2D thisCollider;

    void Start()
    {
       
        // Fill list of team colours.
        //_teamColours.Add(_yellowTeamConnectedColor);
        //_teamColours.Add(_blueTeamConnectedColor);
        //_teamColours.Add(_redTeamConnectedColor);
        //_teamColours.Add(_purpleTeamConnectedColor);
        //_teamColours.Add(_orangeTeamConnectedColor);
        //_teamColours.Add(_greenTeamConnectedColor);

        // Fill list of team startpoints.
        //_teamStartPoints.Add(yellowTeamStartPoints);
        //_teamStartPoints.Add(blueTeamStartPoints);
        //_teamStartPoints.Add(redTeamStartPoints);
        //_teamStartPoints.Add(purpleTeamStartPoints);
        //_teamStartPoints.Add(orangeTeamStartPoints);
        //_teamStartPoints.Add(greenTeamStartPoints);

        // Fill list of team startpoints tag names.
        //_startPointTagNames.Add("Yellow Team StartPoint");
        //_startPointTagNames.Add("Blue Team StartPoint");
        //_startPointTagNames.Add("Red Team StartPoint");
        //_startPointTagNames.Add("Purple Team StartPoint");
        //_startPointTagNames.Add("Orange Team StartPoint");
        //_startPointTagNames.Add("Green Team StartPoint");

        _sprite = GetComponent<SpriteRenderer>();      
        _originalParent = transform.parent;

        toggles = GameObject.FindGameObjectsWithTag("Toggle");         
       
        thisCollider = GetComponent<Collider2D>();
        Debug.Log(thisCollider.bounciness);
    }

    void Update()
    {
        // Find all owned start points -----------------        
        // Find all startpoints owned by yellow team.
        yellowTeamStartPoints = GameObject.FindGameObjectsWithTag("Yellow Team StartPoint");
        // Find all startpoints owned by blue team.
        blueTeamStartPoints = GameObject.FindGameObjectsWithTag("Blue Team StartPoint");
        // Find all startpoints owned by yellow team.
        redTeamStartPoints = GameObject.FindGameObjectsWithTag("Red Team StartPoint");
        // Find all startpoints owned by blue team.
        purpleTeamStartPoints = GameObject.FindGameObjectsWithTag("Purple Team StartPoint");
        // Find all startpoints owned by yellow team.
        orangeTeamStartPoints = GameObject.FindGameObjectsWithTag("Orange Team StartPoint");
        // Find all startpoints owned by blue team.
        greenTeamStartPoints = GameObject.FindGameObjectsWithTag("Green Team StartPoint");

        // trying to make one look for all teams.

        // Go through each team
        //for (int i = 0; i < _teamStartPoints.Count; i++)
        //{         
        //    // Check if bounds of this toggle are overlapping a yellow start point.
        //    for (int j = 0; j < _teamStartPoints[i].Length; j++)
        //    {
        //        if (thisCollider.bounds.Intersects(_teamStartPoints[i][j].GetComponent<CircleCollider2D>().bounds))
        //        {
        //            _sprite.color = _teamColours[i];
        //            gameObject.transform.parent = _teamStartPoints[i][j].transform;

        //            // If so, exit the Update method.
        //            return;
        //        }
        //        else
        //        {
        //            _sprite.color = _disconnectedColor;
        //            gameObject.transform.parent = _originalParent;
        //        }
        //    }

        //    //Otherwise, check if bounds of this toggle are overlapping the bounds of any other toggle.
        //    for (int k = 0; k < toggles.Length; k++)
        //    {
        //        // If they are...
        //        if (thisCollider.bounds.Intersects(toggles[k].GetComponent<BoxCollider2D>().bounds))
        //        {
        //            // If that toggle in a descendant of the startpoint...
        //            if (toggles[k].transform.root.tag == _startPointTagNames[i])
        //            {
        //                _sprite.color = _teamColours[i];
        //                gameObject.transform.parent = toggles[k].transform;

        //                // If so, exit the Update method.
        //                return;
        //            }
        //            // if not...
        //            else
        //            {
        //                _sprite.color = _disconnectedColor;
        //                gameObject.transform.parent = _originalParent;
        //            }
        //        }
        //        // If they are not...
        //        else
        //        {
        //            _sprite.color = _disconnectedColor;
        //            gameObject.transform.parent = _originalParent;
        //        }
        //    }
        //}       



        // Yellow ------------------------
        // Check if bounds of this toggle are overlapping a yellow start point.
        for (int i = 0; i < yellowTeamStartPoints.Length; i++)
        {
            if (thisCollider.bounds.Intersects(yellowTeamStartPoints[i].GetComponent<CircleCollider2D>().bounds))
            {
                _sprite.color = _yellowTeamConnectedColor;
                gameObject.transform.parent = yellowTeamStartPoints[i].transform;

                // If so, exit the Update method.
                return;
            }
            else
            {
                _sprite.color = _disconnectedColor;
                gameObject.transform.parent = _originalParent;
            }
        }

        // Otherwise, check if bounds of this toggle are overlapping the bounds of any other toggle.       
        for (int i = 0; i < toggles.Length; i++)
        {
            // If they are...
            if (thisCollider.bounds.Intersects(toggles[i].GetComponent<BoxCollider2D>().bounds))
            {
                // If that toggle in a descendant of the startpoint...
                if (toggles[i].transform.root.tag == "Yellow Team StartPoint")
                {
                    _sprite.color = _yellowTeamConnectedColor;
                    gameObject.transform.parent = toggles[i].transform;

                    // If so, exit the Update method.
                    return;
                }
                // if not...
                else
                {
                    _sprite.color = _disconnectedColor;
                    gameObject.transform.parent = _originalParent;
                }
            }
            // If they are not...
            else
            {
                _sprite.color = _disconnectedColor;
                gameObject.transform.parent = _originalParent;
            }
        }

        // Blue ------------------------
        // Check if bounds of this toggle are overlapping a blue start point.
        for (int i = 0; i < blueTeamStartPoints.Length; i++)
        {
            if (thisCollider.bounds.Intersects(blueTeamStartPoints[i].GetComponent<CircleCollider2D>().bounds))
            {
                _sprite.color = _blueTeamConnectedColor;
                gameObject.transform.parent = blueTeamStartPoints[i].transform;

                // If so, exit the Update method.
                return;
            }
            else
            {
                _sprite.color = _disconnectedColor;
                gameObject.transform.parent = _originalParent;
            }
        }

        // Otherwise, check if bounds of this toggle are overlapping the bounds of any other toggle.       
        for (int i = 0; i < toggles.Length; i++)
        {
            // If they are...
            if (thisCollider.bounds.Intersects(toggles[i].GetComponent<BoxCollider2D>().bounds))
            {
                // If that toggle in a descendant of the startpoint...
                if (toggles[i].transform.root.tag == "Blue Team StartPoint")
                {
                    _sprite.color = _blueTeamConnectedColor;
                    gameObject.transform.parent = toggles[i].transform;

                    // If so, exit the Update method.
                    return;
                }
                // if not...
                else
                {
                    _sprite.color = _disconnectedColor;
                    gameObject.transform.parent = _originalParent;
                }
            }
            // If they are not...
            else
            {
                _sprite.color = _disconnectedColor;
                gameObject.transform.parent = _originalParent;
            }
        }

        // Red ------------------------
        // Check if bounds of this toggle are overlapping a red start point.
        for (int i = 0; i < redTeamStartPoints.Length; i++)
        {
            if (thisCollider.bounds.Intersects(redTeamStartPoints[i].GetComponent<CircleCollider2D>().bounds))
            {
                _sprite.color = _redTeamConnectedColor;
                gameObject.transform.parent = redTeamStartPoints[i].transform;

                // If so, exit the Update method.
                return;
            }
            else
            {
                _sprite.color = _disconnectedColor;
                gameObject.transform.parent = _originalParent;
            }
        }

        // Otherwise, check if bounds of this toggle are overlapping the bounds of any other toggle.       
        for (int i = 0; i < toggles.Length; i++)
        {
            // If they are...
            if (thisCollider.bounds.Intersects(toggles[i].GetComponent<BoxCollider2D>().bounds))
            {
                // If that toggle in a descendant of the startpoint...
                if (toggles[i].transform.root.tag == "Red Team StartPoint")
                {
                    _sprite.color = _redTeamConnectedColor;
                    gameObject.transform.parent = toggles[i].transform;

                    // If so, exit the Update method.
                    return;
                }
                // if not...
                else
                {
                    _sprite.color = _disconnectedColor;
                    gameObject.transform.parent = _originalParent;
                }
            }
            // If they are not...
            else
            {
                _sprite.color = _disconnectedColor;
                gameObject.transform.parent = _originalParent;
            }
        }

        // Purple ------------------------
        // Check if bounds of this toggle are overlapping a purple start point.
        for (int i = 0; i < purpleTeamStartPoints.Length; i++)
        {
            if (thisCollider.bounds.Intersects(purpleTeamStartPoints[i].GetComponent<CircleCollider2D>().bounds))
            {
                _sprite.color = _purpleTeamConnectedColor;
                gameObject.transform.parent = purpleTeamStartPoints[i].transform;

                // If so, exit the Update method.
                return;
            }
            else
            {
                _sprite.color = _disconnectedColor;
                gameObject.transform.parent = _originalParent;
            }
        }

        // Otherwise, check if bounds of this toggle are overlapping the bounds of any other toggle.       
        for (int i = 0; i < toggles.Length; i++)
        {
            // If they are...
            if (thisCollider.bounds.Intersects(toggles[i].GetComponent<BoxCollider2D>().bounds))
            {
                // If that toggle in a descendant of the startpoint...
                if (toggles[i].transform.root.tag == "Purple Team StartPoint")
                {
                    _sprite.color = _purpleTeamConnectedColor;
                    gameObject.transform.parent = toggles[i].transform;

                    // If so, exit the Update method.
                    return;
                }
                // if not...
                else
                {
                    _sprite.color = _disconnectedColor;
                    gameObject.transform.parent = _originalParent;
                }
            }
            // If they are not...
            else
            {
                _sprite.color = _disconnectedColor;
                gameObject.transform.parent = _originalParent;
            }
        }

        // Orange ------------------------
        // Check if bounds of this toggle are overlapping a purple start point.
        for (int i = 0; i < orangeTeamStartPoints.Length; i++)
        {
            if (thisCollider.bounds.Intersects(orangeTeamStartPoints[i].GetComponent<CircleCollider2D>().bounds))
            {
                _sprite.color = _orangeTeamConnectedColor;
                gameObject.transform.parent = orangeTeamStartPoints[i].transform;

                // If so, exit the Update method.
                return;
            }
            else
            {
                _sprite.color = _disconnectedColor;
                gameObject.transform.parent = _originalParent;
            }
        }

        // Otherwise, check if bounds of this toggle are overlapping the bounds of any other toggle.       
        for (int i = 0; i < toggles.Length; i++)
        {
            // If they are...
            if (thisCollider.bounds.Intersects(toggles[i].GetComponent<BoxCollider2D>().bounds))
            {
                // If that toggle in a descendant of the startpoint...
                if (toggles[i].transform.root.tag == "Orange Team StartPoint")
                {
                    _sprite.color = _orangeTeamConnectedColor;
                    gameObject.transform.parent = toggles[i].transform;

                    // If so, exit the Update method.
                    return;
                }
                // if not...
                else
                {
                    _sprite.color = _disconnectedColor;
                    gameObject.transform.parent = _originalParent;
                }
            }
            // If they are not...
            else
            {
                _sprite.color = _disconnectedColor;
                gameObject.transform.parent = _originalParent;
            }
        }

        // Green ------------------------
        // Check if bounds of this toggle are overlapping a purple start point.
        for (int i = 0; i < greenTeamStartPoints.Length; i++)
        {
            if (thisCollider.bounds.Intersects(greenTeamStartPoints[i].GetComponent<CircleCollider2D>().bounds))
            {
                _sprite.color = _greenTeamConnectedColor;
                gameObject.transform.parent = greenTeamStartPoints[i].transform;

                // If so, exit the Update method.
                return;
            }
            else
            {
                _sprite.color = _disconnectedColor;
                gameObject.transform.parent = _originalParent;
            }
        }

        // Otherwise, check if bounds of this toggle are overlapping the bounds of any other toggle.       
        for (int i = 0; i < toggles.Length; i++)
        {
            // If they are...
            if (thisCollider.bounds.Intersects(toggles[i].GetComponent<BoxCollider2D>().bounds))
            {
                // If that toggle in a descendant of the startpoint...
                if (toggles[i].transform.root.tag == "Green Team StartPoint")
                {
                    _sprite.color = _greenTeamConnectedColor;
                    gameObject.transform.parent = toggles[i].transform;

                    // If so, exit the Update method.
                    return;
                }
                // if not...
                else
                {
                    _sprite.color = _disconnectedColor;
                    gameObject.transform.parent = _originalParent;
                }
            }
            // If they are not...
            else
            {
                _sprite.color = _disconnectedColor;
                gameObject.transform.parent = _originalParent;
            }
        }
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
                        bool isSteal = false;

                        if (_sprite.color != _disconnectedColor)
                        {
                            if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Yellow")
                            {
                                if (_sprite.color != _yellowTeamConnectedColor)
                                {
                                    isSteal = true;
                                    if (GameManager.NumberOfStealsRemaining > 0)
                                    {
                                        GameManager.NumberOfStealsRemaining--;
                                    }
                                }
                            }
                            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Blue")
                            {
                                if (_sprite.color != _blueTeamConnectedColor)
                                {
                                    isSteal = true;
                                    if (GameManager.NumberOfStealsRemaining > 0)
                                    {
                                        GameManager.NumberOfStealsRemaining--;
                                    }
                                }
                            }
                            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Red")
                            {
                                if (_sprite.color != _redTeamConnectedColor)
                                {
                                    isSteal = true;
                                    if (GameManager.NumberOfStealsRemaining > 0)
                                    {
                                        GameManager.NumberOfStealsRemaining--;                                        
                                    }
                                }
                            }
                            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Purple")
                            {
                                if (_sprite.color != _purpleTeamConnectedColor)
                                {
                                    isSteal = true;
                                    if (GameManager.NumberOfStealsRemaining > 0)
                                    {
                                        GameManager.NumberOfStealsRemaining--;                                        
                                    }
                                }
                            }
                            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Orange")
                            {
                                if (_sprite.color != _orangeTeamConnectedColor)
                                {
                                    isSteal = true;
                                    if (GameManager.NumberOfStealsRemaining > 0)
                                    {
                                        GameManager.NumberOfStealsRemaining--;                                        
                                    }
                                }
                            }
                            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Green")
                            {
                                if (_sprite.color != _greenTeamConnectedColor)
                                {
                                    isSteal = true;
                                    if (GameManager.NumberOfStealsRemaining > 0)
                                    {
                                        GameManager.NumberOfStealsRemaining--;                                        
                                    }
                                }
                            }
                        }

                        if (!isSteal || GameManager.NumberOfStealsRemaining > 0)
                        {
                            int numChildren = transform.childCount;

                            for (int i = 0; i < numChildren; i++)
                            {
                                if (numChildren > i)
                                {
                                    transform.GetChild(i).parent = _originalParent;
                                }
                            }

                            //transform.parent = _originalParent;
                            //_sprite.color = _disconnectedColor;

                            transform.eulerAngles = Vector3.forward * (transform.eulerAngles.z - 90);

                            // Reduce turns in round remaining for all team members.                       
                            photonView.RPC("ReduceTurnsRemaining", RpcTarget.All, PhotonNetwork.LocalPlayer.GetPhotonTeam().Name);
                       }
                        
                    }
                }
            }
        }
    }

  

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Yellow Team StartPoint")
    //    {
    //        _sprite.color = _yellowTeamConnectedColor;
    //        gameObject.transform.parent = collision.transform;           
    //    }
    //    else if (collision.transform.root.tag == "Yellow Team StartPoint")
    //    {
    //        _sprite.color = _yellowTeamConnectedColor;
    //        gameObject.transform.parent = collision.transform;
    //    }   
    //    else if (collision.gameObject.tag == "Blue Team StartPoint")
    //    {
    //        _sprite.color = _blueTeamConnectedColor;
    //        gameObject.transform.parent = collision.transform;           
    //    }
    //    else if (collision.transform.root.tag == "Blue Team StartPoint")
    //    {
    //        _sprite.color = _blueTeamConnectedColor;
    //        gameObject.transform.parent = collision.transform;
    //    }
    //    else if (collision.gameObject.tag == "Red Team StartPoint")
    //    {
    //        _sprite.color = _redTeamConnectedColor;
    //        gameObject.transform.parent = collision.transform;
    //    }
    //    else if (collision.transform.root.tag == "Red Team StartPoint")
    //    {
    //        _sprite.color = _redTeamConnectedColor;
    //        gameObject.transform.parent = collision.transform;
    //    }
    //    else if (collision.gameObject.tag == "Purple Team StartPoint")
    //    {
    //        _sprite.color = _purpleTeamConnectedColor;
    //        gameObject.transform.parent = collision.transform;
    //    }
    //    else if (collision.transform.root.tag == "Purple Team StartPoint")
    //    {
    //        _sprite.color = _purpleTeamConnectedColor;
    //        gameObject.transform.parent = collision.transform;
    //    }
    //    else if (collision.gameObject.tag == "Orange Team StartPoint")
    //    {
    //        _sprite.color = _orangeTeamConnectedColor;
    //        gameObject.transform.parent = collision.transform;
    //    }
    //    else if (collision.transform.root.tag == "Orange Team StartPoint")
    //    {
    //        _sprite.color = _orangeTeamConnectedColor;
    //        gameObject.transform.parent = collision.transform;
    //    }
    //    else if (collision.gameObject.tag == "Green Team StartPoint")
    //    {
    //        _sprite.color = _greenTeamConnectedColor;
    //        gameObject.transform.parent = collision.transform;
    //    }
    //    else if (collision.transform.root.tag == "Green Team StartPoint")
    //    {
    //        _sprite.color = _greenTeamConnectedColor;
    //        gameObject.transform.parent = collision.transform;
    //    }
    //    else
    //    {
    //        _sprite.color = _disconnectedColor;
    //    }  
    //}

    //private void OnTriggerStay2D(Collider2D collider)
    //{       
    //    if (collider.gameObject.tag == "Yellow Team StartPoint")
    //    {
    //        _sprite.color = _yellowTeamConnectedColor;
    //        gameObject.transform.parent = collider.transform;
    //    }
    //    else if (collider.transform.root.tag == "Yellow Team StartPoint")
    //    {
    //        _sprite.color = _yellowTeamConnectedColor;
    //        gameObject.transform.parent = collider.transform;
    //    }       
    //    else
    //    {          
    //        _sprite.color = _disconnectedColor;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    _sprite.color = _disconnectedColor;
    //}

    //private void OnTriggerExit2D(Collider2D collider)
    //{  
    //   _sprite.color = _disconnectedColor;
    //}

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

            //transform.parent = _originalParent;
            //_sprite.color = _disconnectedColor;

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
