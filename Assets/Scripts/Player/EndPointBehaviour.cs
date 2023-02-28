using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class EndPointBehaviour : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Color _yellowTeamConnectedColor = new Color(1, 1, 0, 1);
    private Color _blueTeamConnectedColor = new Color(0, 0, 1, 1);
    private Color _redTeamConnectedColor = new Color(1, 0, 0, 1);
    private Color _purpleTeamConnectedColor = new Color(1, 0, 1, 1);
    private Color _orangeTeamConnectedColor = new Color(1, 0.65f, 0, 1);
    private Color _greenTeamConnectedColor = new Color(0, 1, 0, 1);
    private Color _disconnectedColor = new Color(1, 1, 1, 1);
    private Collider2D _toggle;

    // In order to call methods in this script.
    private GameManager gameManagerScript;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();

        // In order to call methods in this script.
        GameObject gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.parent = null;
        _sprite.color = _disconnectedColor;       
    }

    // Check if a teams' toggle has connected.
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnTriggerEnter2D");

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            _toggle = col;
            // Add a delay, because first the toggle itself must be connected to the path way.
            Invoke("CheckToggleRootTag", 0.5f);         
        }
    }

    void CheckToggleRootTag()
    {
        Debug.Log("CheckToggleRootTag");
        if (_toggle.gameObject.transform.root.tag == "Yellow Team StartPoint" ||
            _toggle.gameObject.transform.root.tag == "Red Team StartPoint" ||
            _toggle.gameObject.transform.root.tag == "Blue Team StartPoint" ||
            _toggle.gameObject.transform.root.tag == "Purple Team StartPoint" ||
            _toggle.gameObject.transform.root.tag == "Orange Team StartPoint" ||
            _toggle.gameObject.transform.root.tag == "Green Team StartPoint")
        {
            Debug.Log("past if statements");
            gameManagerScript.NextRound();
        }
    }
}
