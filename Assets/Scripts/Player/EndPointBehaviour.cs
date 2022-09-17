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

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.root.tag == "Yellow Team StartPoint")
        {           
            _sprite.color = _yellowTeamConnectedColor;           
        }
        else if (collision.transform.root.tag == "Blue Team StartPoint")
        {          
            _sprite.color = _blueTeamConnectedColor;           
        }
        else if (collision.transform.root.tag == "Red Team StartPoint")
        {
            _sprite.color = _redTeamConnectedColor;
        }
        else if (collision.transform.root.tag == "Purple Team StartPoint")
        {
            _sprite.color = _purpleTeamConnectedColor;
        }
        else if (collision.transform.root.tag == "Orange Team StartPoint")
        {
            _sprite.color = _orangeTeamConnectedColor;
        }
        else if (collision.transform.root.tag == "Green Team StartPoint")
        {
            _sprite.color = _greenTeamConnectedColor;
        }

        transform.parent = collision.transform;
    }
}
