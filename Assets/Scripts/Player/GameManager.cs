using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameManager : MonoBehaviourPun
{
    public Text TeamNameText;
    public Text PlayerNameText;
   // public Text RoundScoreText;
    public Text ScoreText;
    public Text _roundNumberText;
    public Text _movesRemainingText;
    public Text _timeRemainingText;
    public Text NumberOfStealsRemainingText;
    public Text GameCompleteText;

    [SerializeField]
    private CanvasGroup _infoPanelCanvasGroup;
    [SerializeField]
    private CanvasGroup _gameCompletePanelCanvasGroup;

    private int _score = 0;

    private int _yellowTeamScore = 0;
    private int _blueTeamScore = 0;
    private int _redTeamScore = 0;
    private int _purpleTeamScore = 0;
    private int _orangeTeamScore = 0;
    private int _greenTeamScore = 0;

    private int _roundNumber = 1;
    public static int TotalNumberOfRounds;
    public static int MovesInRoundRemaining;
    public static float SecondsInRoundRemaining;
    public static int PointsPerToggle;
    public static int PointsPerEndpoint;
    public static int NumberOfStealsRemaining;
    private bool _isGameStarted = false;
    public static bool IsGameEnded;

    private Color _yellowTeamConnectedColor = new Color(1, 1, 0, 1);
    private Color _blueTeamConnectedColor = new Color(0, 0, 1, 1);
    private Color _redTeamConnectedColor = new Color(1, 0, 0, 1);
    private Color _purpleTeamConnectedColor = new Color(1, 0, 1, 1);
    private Color _orangeTeamConnectedColor = new Color(1, 0.65f, 0, 1);
    private Color _greenTeamConnectedColor = new Color(0, 1, 0, 1);  

    private GameObject[] yellowTeamStartpoints;
    private GameObject[] blueTeamStartpoints;
    private GameObject[] redTeamStartpoints;
    private GameObject[] purpleTeamStartpoints;
    private GameObject[] orangeTeamStartpoints;
    private GameObject[] greenTeamStartpoints;
    int numberOfToggles = 0;
    int numberOfEndPoints = 0;
    private Color _disconnectedColor = new Color(1, 1, 1, 1);

    public GameObject[] toggles;
    public GameObject[] endPoints;


    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _infoPanelCanvasGroup.alpha = 1;
        _isGameStarted = true;
        ScoreText.text = _score.ToString();


        endPoints = GameObject.FindGameObjectsWithTag("EndPoint");
        Debug.Log(endPoints.Length);
        toggles = GameObject.FindGameObjectsWithTag("Toggle");
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        MovesInRoundRemaining = (int)GameAndMapSettings.roomOptions.CustomRoomProperties["MovesPerRound"];
        SecondsInRoundRemaining = (float)GameAndMapSettings.roomOptions.CustomRoomProperties["SecondsPerRound"];
        TotalNumberOfRounds = (int)GameAndMapSettings.roomOptions.CustomRoomProperties["TotalNumberOfRounds"];
        PointsPerToggle = (int)GameAndMapSettings.roomOptions.CustomRoomProperties["PointsPerToggle"];
        PointsPerEndpoint = (int)GameAndMapSettings.roomOptions.CustomRoomProperties["PointsPerEndpoint"];
        NumberOfStealsRemaining = (int)GameAndMapSettings.roomOptions.CustomRoomProperties["TotalNumberOfSteals"];

        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name != "Teacher")
        {
            TeamNameText.text = PhotonNetwork.LocalPlayer.GetPhotonTeam().Name;

            if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Yellow")
            {
                TeamNameText.color = _yellowTeamConnectedColor;
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Blue")
            {
                TeamNameText.color = _blueTeamConnectedColor;
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Red")
            {
                TeamNameText.color = _redTeamConnectedColor;
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Purple")
            {
                TeamNameText.color = _purpleTeamConnectedColor;
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Orange")
            {
                TeamNameText.color = _orangeTeamConnectedColor;
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Green")
            {
                TeamNameText.color = _greenTeamConnectedColor;
            }
        }

        PlayerNameText.text = (string)PhotonNetwork.LocalPlayer.NickName;

        _roundNumberText.text = _roundNumber.ToString();
        _movesRemainingText.text = MovesInRoundRemaining.ToString();
        _timeRemainingText.text = SecondsInRoundRemaining.ToString();
        NumberOfStealsRemainingText.text = NumberOfStealsRemaining.ToString();       
    }

    public void Update()
    {   
        if (_isGameStarted && !IsGameEnded)
        {         
            // Time remaining
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                SecondsInRoundRemaining -= Time.deltaTime;
                if (SecondsInRoundRemaining > 0)
                {
                    photonView.RPC("DecreaseTimeRemaining", RpcTarget.All, SecondsInRoundRemaining);
                }
                // End game if no time remaining.
                if (SecondsInRoundRemaining < 0)
                {
                    if (_roundNumber == TotalNumberOfRounds)
                    {
                        Debug.Log("End of game");
                        photonView.RPC("MasterClientEndGame", RpcTarget.MasterClient);                        
                    }
                    else
                    {
                        photonView.RPC("NextRound", RpcTarget.All);
                    }
                }
            }
        }
    }

    public void UpdateScore()
    {
        if (_isGameStarted && !IsGameEnded)
        { 
            photonView.RPC("UpdateScoreRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    void UpdateScoreRPC()
    {
      //  Debug.Log("update score");

        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name != "Teacher")
        {
            if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Yellow")
            {
                numberOfToggles = 0;
                numberOfEndPoints = 0;

                yellowTeamStartpoints = GameObject.FindGameObjectsWithTag("Yellow Team StartPoint");
                if (yellowTeamStartpoints.Length > 0)
                {
                    for (int i = 0; i < yellowTeamStartpoints.Length; i++)
                    {
                        Transform[] children = yellowTeamStartpoints[i].transform.GetComponentsInChildren<Transform>();
                       
                        foreach (Transform child in children)
                        {
                            if (child.tag == "Toggle")
                            {
                                numberOfToggles++;
                            }
                            else if (child.tag == "EndPoint")
                            {
                                numberOfEndPoints++;
                            }
                        }
                    }                 
                }
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Blue")
            {
                numberOfToggles = 0;
                numberOfEndPoints = 0;

                blueTeamStartpoints = GameObject.FindGameObjectsWithTag("Blue Team StartPoint");
                if (blueTeamStartpoints.Length > 0)
                {
                    for (int i = 0; i < blueTeamStartpoints.Length; i++)
                    {
                        Transform[] children = blueTeamStartpoints[i].transform.GetComponentsInChildren<Transform>();

                        foreach (Transform child in children)
                        {
                            if (child.tag == "Toggle")
                            {
                                numberOfToggles++;
                            }
                            else if (child.tag == "EndPoint")
                            {
                                numberOfEndPoints++;
                            }
                        }
                    }
                }
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Red")
            {
                numberOfToggles = 0;
                numberOfEndPoints = 0;

                redTeamStartpoints = GameObject.FindGameObjectsWithTag("Red Team StartPoint");
                if (redTeamStartpoints.Length > 0)
                {
                    for (int i = 0; i < redTeamStartpoints.Length; i++)
                    {
                        Transform[] children = redTeamStartpoints[i].transform.GetComponentsInChildren<Transform>();

                        foreach (Transform child in children)
                        {
                            if (child.tag == "Toggle")
                            {
                                numberOfToggles++;
                            }
                            else if (child.tag == "EndPoint")
                            {
                                numberOfEndPoints++;
                            }
                        }
                    }
                }
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Purple")
            {
                numberOfToggles = 0;
                numberOfEndPoints = 0;

                purpleTeamStartpoints = GameObject.FindGameObjectsWithTag("Purple Team StartPoint");
                if (purpleTeamStartpoints.Length > 0)
                {
                    for (int i = 0; i < purpleTeamStartpoints.Length; i++)
                    {
                        Transform[] children = purpleTeamStartpoints[i].transform.GetComponentsInChildren<Transform>();

                        foreach (Transform child in children)
                        {
                            if (child.tag == "Toggle")
                            {
                                numberOfToggles++;
                            }
                            else if (child.tag == "EndPoint")
                            {
                                numberOfEndPoints++;
                            }
                        }
                    }
                }
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Orange")
            {
                numberOfToggles = 0;
                numberOfEndPoints = 0;

                orangeTeamStartpoints = GameObject.FindGameObjectsWithTag("Orange Team StartPoint");
                if (orangeTeamStartpoints.Length > 0)
                {
                    for (int i = 0; i < orangeTeamStartpoints.Length; i++)
                    {
                        Transform[] children = orangeTeamStartpoints[i].transform.GetComponentsInChildren<Transform>();

                        foreach (Transform child in children)
                        {
                            if (child.tag == "Toggle")
                            {
                                numberOfToggles++;
                            }
                            else if (child.tag == "EndPoint")
                            {
                                numberOfEndPoints++;
                            }
                        }
                    }
                }
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Green")
            {
                numberOfToggles = 0;
                numberOfEndPoints = 0;

                greenTeamStartpoints = GameObject.FindGameObjectsWithTag("Green Team StartPoint");
                if (greenTeamStartpoints.Length > 0)
                {
                    for (int i = 0; i < greenTeamStartpoints.Length; i++)
                    {
                        Transform[] children = greenTeamStartpoints[i].transform.GetComponentsInChildren<Transform>();

                        foreach (Transform child in children)
                        {
                            if (child.tag == "Toggle")
                            {
                                numberOfToggles++;
                            }
                            else if (child.tag == "EndPoint")
                            {
                                numberOfEndPoints++;
                            }
                        }
                    }
                }
            }

            // Calculate the score.
            _score = (numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint);

            // Update the score for each team.
            photonView.RPC("UpdateTeamScores", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.GetPhotonTeam().Name, _score);

            // Update the UI.
            NumberOfStealsRemainingText.text = NumberOfStealsRemaining.ToString();
            ScoreText.text = _score.ToString();
            _movesRemainingText.text = MovesInRoundRemaining.ToString();

            if (MovesInRoundRemaining == 0)
            {
                if (_roundNumber == TotalNumberOfRounds)
                {
                    Debug.Log("End of game");
                    photonView.RPC("MasterClientEndGame", RpcTarget.MasterClient);
                }
                else
                {
                    photonView.RPC("NextRound", RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    void DecreaseTimeRemaining(float masterClientSecondsInRoundRemaining)    
    {
        SecondsInRoundRemaining = masterClientSecondsInRoundRemaining;

        float roundedSecondsInRoundRemaining = UnityEngine.Mathf.Round(SecondsInRoundRemaining);
        _timeRemainingText.text = roundedSecondsInRoundRemaining.ToString();       
    }

    [PunRPC]
    void NextRound()
    {
        if (_roundNumber < TotalNumberOfRounds)
        {
            _roundNumber++;
            _roundNumberText.text = _roundNumber.ToString();

          //  _totalScore = _totalScore + RoundScore;
          //  TotalScoreText.text = _totalScore.ToString();

            // Reset the parameters for the next round.
          //  RoundScore = 0;
         //   RoundScoreText.text = RoundScore.ToString();
            MovesInRoundRemaining = (int)GameAndMapSettings.roomOptions.CustomRoomProperties["MovesPerRound"];
            SecondsInRoundRemaining = (float)GameAndMapSettings.roomOptions.CustomRoomProperties["SecondsPerRound"];
        }
    }

    [PunRPC]
    void MasterClientEndGame()
    {
        // need to RPC all clients to get latest scores,
      //  photonView.RPC("GetAllEndGameScores", RpcTarget.All);

        // then fire the below.
        photonView.RPC("EndGame", RpcTarget.All,
                            _yellowTeamScore, _blueTeamScore, _redTeamScore, _purpleTeamScore, _orangeTeamScore, _greenTeamScore);
    }

    //[PunRPC]
    //void MasterClientReceiveEndGameScores()
    //{

    //}


    [PunRPC]
    void EndGame(int yellowTeamScore, int blueTeamScore, int redTeamScore, int purpleTeamScore, int orangeTeamScore, int greenTeamScore)
    {   
        ScoreText.text = _score.ToString();

        IsGameEnded = true;
        _gameCompletePanelCanvasGroup.alpha = 1;

        // Create the end of game text.     
        string gameCompletedString = "Game Finished! \n";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetPhotonTeam().Name == "Yellow")
            {
                gameCompletedString = gameCompletedString + "Yellow Team: " + yellowTeamScore + "\n";
            }
            else if (player.GetPhotonTeam().Name == "Blue")
            {
                gameCompletedString = gameCompletedString + "Blue Team: " + blueTeamScore + "\n";
            }
            else if (player.GetPhotonTeam().Name == "Red")
            {
                gameCompletedString = gameCompletedString + "Red Team: " + redTeamScore + "\n";
            }
            else if (player.GetPhotonTeam().Name == "Purple")
            {
                gameCompletedString = gameCompletedString + "Purple Team: " + purpleTeamScore + "\n";
            }
            else if (player.GetPhotonTeam().Name == "Orange")
            {
                gameCompletedString = gameCompletedString + "Orange Team: " + orangeTeamScore + "\n";
            }
            else if (player.GetPhotonTeam().Name == "Green")
            {
                gameCompletedString = gameCompletedString + "Green Team: " + greenTeamScore + "\n";
            }           
        }

        GameCompleteText.text = gameCompletedString;
    }

    public void CheckIfEndPointTouchingAnyToggle()
    {
        photonView.RPC("CheckIfEndPointTouchingAnyToggleRPC", RpcTarget.All);
    }

    [PunRPC]
    public void CheckIfEndPointTouchingAnyToggleRPC()
    {
        endPoints = GameObject.FindGameObjectsWithTag("EndPoint"); 
        toggles = GameObject.FindGameObjectsWithTag("Toggle");

        //   bool isConnected;
        for (int i = 0; i < endPoints.Length; i++)
        {
            var endPoint = endPoints[i].GetComponent<BoxCollider2D>();

            bool isConnected = false;

            for (int j = 0; j < toggles.Length; j++)
            {
                Bounds toggle = toggles[j].transform.GetChild(0).GetComponent<BoxCollider2D>().bounds;

                if (endPoint.bounds.Intersects(toggle))
                {
                    if (toggles[j].transform.root.tag == "Yellow Team StartPoint")
                    {                       
                        isConnected = true;
                        break;
                    }
                    else if (toggles[j].transform.root.tag == "Blue Team StartPoint")
                    {                        
                        isConnected = true;
                        break;
                    }
                    else if (toggles[j].transform.root.tag == "Red Team StartPoint")
                    {
                        isConnected = true;
                        break;
                    }
                    else if (toggles[j].transform.root.tag == "Purple Team StartPoint")
                    {
                        isConnected = true;
                        break;
                    }
                    else if (toggles[j].transform.root.tag == "Orange Team StartPoint")
                    {
                        isConnected = true;
                        break;
                    }
                    else if (toggles[j].transform.root.tag == "Green Team StartPoint")
                    {
                        isConnected = true;
                        break;
                    }
                }               
            }
            if (isConnected == false)
            {
                endPoint.transform.parent = null;
                endPoint.transform.GetComponent<SpriteRenderer>().color = _disconnectedColor;               
            }           
        }       
    }

    // Only on the master client.
    [PunRPC]
    void UpdateTeamScores(string teamName, int score)
    {
        Debug.Log(teamName);
        Debug.Log(score);

        if (teamName == "Yellow")
        {
            _yellowTeamScore = score;

           // Debug.Log(yellowTeamScore);
        }
        else if (teamName == "Blue")
        {
            _blueTeamScore = score;
        }
        else if (teamName == "Red")
        {
            _redTeamScore = score;
        }
        else if (teamName == "Purple")
        {
            _purpleTeamScore = score;
        }
        else if (teamName == "Orange")
        {
            _orangeTeamScore = score;
        }
        else if (teamName == "Green")
        {
            _greenTeamScore = score;
        }       
    }
}
