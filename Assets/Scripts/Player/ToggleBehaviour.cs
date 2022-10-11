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
    public GameObject[] endPoints;
    public GameObject[] yellowTeamStartPoints;
    public GameObject[] blueTeamStartPoints;
    public GameObject[] redTeamStartPoints;
    public GameObject[] purpleTeamStartPoints;
    public GameObject[] orangeTeamStartPoints;
    public GameObject[] greenTeamStartPoints;
    List<GameObject[]> _teamStartPoints = new List<GameObject[]>();
    List<string> _startPointTagNames = new List<string>();

    public bool isCheckToggles;    
    private GameManager gameManagerScript;

    [SerializeField]
    BoxCollider2D thisCollider;

    void Start()
    {          

         _sprite = GetComponent<SpriteRenderer>();      
        _originalParent = transform.parent;

        endPoints = GameObject.FindGameObjectsWithTag("EndPoint");
        toggles = GameObject.FindGameObjectsWithTag("Toggle");

        GameObject gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        
    }

    void Update()
    {
      //  Debug.Log(isCheckToggles);
          if (isCheckToggles == true)
        {
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
                if (thisCollider.bounds.Intersects(toggles[i].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds))
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
                if (thisCollider.bounds.Intersects(toggles[i].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds))
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
                if (thisCollider.bounds.Intersects(toggles[i].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds))
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
                if (thisCollider.bounds.Intersects(toggles[i].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds))
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
                if (thisCollider.bounds.Intersects(toggles[i].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds))
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
                if (thisCollider.bounds.Intersects(toggles[i].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds))
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
                        photonView.RPC("UpdateTogglesRPC", RpcTarget.All);

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
                            for (int i = 0; i < endPoints.Length; i++)
                            {
                                endPoints[i].transform.parent = _originalParent;                               
                            }
                            for (int i = 0; i < toggles.Length; i++)
                            {                            
                                toggles[i].transform.parent = _originalParent;                                  
                            }


                            transform.eulerAngles = Vector3.forward * (transform.eulerAngles.z - 90);                            

                            // Reduce turns in round remaining for all team members.                       
                            photonView.RPC("ReduceTurnsRemaining", RpcTarget.All, PhotonNetwork.LocalPlayer.GetPhotonTeam().Name);
                       }

                        photonView.RPC("UpdateStartPoints", RpcTarget.All);                        
                    }
                }
            }
        }
    }


    private IEnumerator UpdateToggles()
    {
        Debug.Log("UpdateTogglesCoroutine");

        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].GetComponent<ToggleBehaviour>().isCheckToggles = true;
        }

        int h = 0;
        while (h < 20)
        {
          //  Debug.Log(h);
            h++;
            yield return null;
        }

        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].GetComponent<ToggleBehaviour>().isCheckToggles = false;
        }

        gameManagerScript.UpdateScore();
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
            for (int i = 0; i < endPoints.Length; i++)
            {
                endPoints[i].transform.parent = _originalParent;
            }

            for (int i = 0; i < toggles.Length; i++)
            {
                toggles[i].transform.parent = _originalParent;
            }

            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void UpdateStartPoints()
    {
        // Find all owned start points -----------------        
        yellowTeamStartPoints = GameObject.FindGameObjectsWithTag("Yellow Team StartPoint");
        blueTeamStartPoints = GameObject.FindGameObjectsWithTag("Blue Team StartPoint");
        redTeamStartPoints = GameObject.FindGameObjectsWithTag("Red Team StartPoint");
        purpleTeamStartPoints = GameObject.FindGameObjectsWithTag("Purple Team StartPoint");
        orangeTeamStartPoints = GameObject.FindGameObjectsWithTag("Orange Team StartPoint");
        greenTeamStartPoints = GameObject.FindGameObjectsWithTag("Green Team StartPoint");
    }

    [PunRPC]
    void UpdateTogglesRPC()
    {
        Debug.Log("UpdateToggles");       
        StartCoroutine(UpdateToggles());
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
