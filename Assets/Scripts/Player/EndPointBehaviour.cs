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

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.parent = null;
        _sprite.color = _disconnectedColor;
    }  
}
