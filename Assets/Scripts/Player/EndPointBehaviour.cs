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

    private Transform _originalParent;
    Collider2D thisCollider;
    public GameObject[] toggles;

    public bool isConnectedToYellow;
    public bool isConnectedToBlue;
    public bool isConnectedToRed;
    public bool isConnectedToPurple;
    public bool isConnectedToOrange;
    public bool isConnectedToGreen;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _originalParent = transform.parent;

        // Find all toggles in the scene.
        toggles = GameObject.FindGameObjectsWithTag("Toggle");

        // Get the collider for this endpoint.
        thisCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(ExecuteAfterTimeEnter(0.1f, collision));       
    }

    IEnumerator ExecuteAfterTimeEnter(float time, Collider2D collision)
    {
        yield return new WaitForSeconds(time);

        if (collision.transform.root.tag == "Yellow Team StartPoint")
        {           
            _sprite.color = _yellowTeamConnectedColor;
            isConnectedToYellow = true;
            gameObject.transform.parent = collision.transform;         
        }

        if (collision.transform.root.tag == "Blue Team StartPoint")
        {
            _sprite.color = _blueTeamConnectedColor;
            isConnectedToBlue = true;
            gameObject.transform.parent = collision.transform;
        }

        if (collision.transform.root.tag == "Red Team StartPoint")
        {
            _sprite.color = _redTeamConnectedColor;
            isConnectedToRed = true;
            gameObject.transform.parent = collision.transform;
        }

        if (collision.transform.root.tag == "Purple Team StartPoint")
        {
            _sprite.color = _purpleTeamConnectedColor;
            isConnectedToPurple = true;
            gameObject.transform.parent = collision.transform;
        }

        if (collision.transform.root.tag == "Orange Team StartPoint")
        {
            _sprite.color = _orangeTeamConnectedColor;
            isConnectedToOrange = true;
            gameObject.transform.parent = collision.transform;
        }

        if (collision.transform.root.tag == "Green Team StartPoint")
        {
            _sprite.color = _greenTeamConnectedColor;
            isConnectedToGreen = true;
            gameObject.transform.parent = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {    
        _sprite.color = _disconnectedColor;
        gameObject.transform.parent = _originalParent;
        StartCoroutine(ExecuteAfterTimeExit(0.1f, collision));
    }

    IEnumerator ExecuteAfterTimeExit(float time, Collider2D collision)
    {
        yield return new WaitForSeconds(time);
        isConnectedToYellow = false;
        isConnectedToBlue = false;
        isConnectedToRed = false;
        isConnectedToPurple = false;
        isConnectedToOrange = false;
        isConnectedToGreen = false;
    }

    //private void Update()
    //{
    //    // For all the toggles.
    //    for (int i = 0; i < toggles.Length; i++)
    //    {
    //        // if any of them overlap this endpoint.
    //        if (thisCollider.bounds.Intersects(toggles[i].GetComponent<BoxCollider2D>().bounds))
    //        {
    //            // If it belongs to the yellow team.
    //            if (toggles[i].transform.root.tag == "Yellow Team StartPoint")
    //            {
    //                _sprite.color = _yellowTeamConnectedColor;
    //                gameObject.transform.parent = toggles[i].transform;

    //                // If so, exit the Update method.
    //                return;
    //            }
    //            // If it belongs to the blue team.
    //            else if (toggles[i].transform.root.tag == "Blue Team StartPoint")
    //            {
    //                _sprite.color = _blueTeamConnectedColor;
    //                gameObject.transform.parent = toggles[i].transform;

    //                // If so, exit the Update method.
    //                return;
    //            }
    //            // If it belongs to the red team.
    //            else if (toggles[i].transform.root.tag == "Red Team StartPoint")
    //            {
    //                _sprite.color = _redTeamConnectedColor;
    //                gameObject.transform.parent = toggles[i].transform;

    //                // If so, exit the Update method.
    //                return;
    //            }
    //            // If it belongs to the purple team.
    //            else if (toggles[i].transform.root.tag == "Purple Team StartPoint")
    //            {
    //                _sprite.color = _purpleTeamConnectedColor;
    //                gameObject.transform.parent = toggles[i].transform;

    //                // If so, exit the Update method.
    //                return;
    //            }
    //            // If it belongs to the orange team.
    //            else if (toggles[i].transform.root.tag == "Orange Team StartPoint")
    //            {
    //                _sprite.color = _orangeTeamConnectedColor;
    //                gameObject.transform.parent = toggles[i].transform;

    //                // If so, exit the Update method.
    //                return;
    //            }
    //            // If it belongs to the green team.
    //            else if (toggles[i].transform.root.tag == "Green Team StartPoint")
    //            {
    //                _sprite.color = _greenTeamConnectedColor;
    //                gameObject.transform.parent = toggles[i].transform;

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
    //        // if not...
    //        else
    //        {
    //            _sprite.color = _disconnectedColor;
    //            gameObject.transform.parent = _originalParent;
    //        }
    //    }
    //}
}
