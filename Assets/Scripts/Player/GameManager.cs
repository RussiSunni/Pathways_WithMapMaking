using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private CanvasGroup _infoPanelCanvasGroup;  

    private int _score = 0;
    // private int _roundScore;
  //  public static int RoundScore;
    private int _roundNumber = 1;
    public static int TotalNumberOfRounds;
    public static int MovesInRoundRemaining;
    public static float SecondsInRoundRemaining;
    public static int PointsPerToggle;
    public static int PointsPerEndpoint;
    public static int NumberOfStealsRemaining;
    private bool _isGameStarted = false;
    private bool _isGameEnded;

    private Color _yellowTeamConnectedColor = new Color(1, 1, 0, 1);
    private Color _blueTeamConnectedColor = new Color(0, 0, 1, 1);
    private Color _redTeamConnectedColor = new Color(1, 0, 0, 1);
    private Color _purpleTeamConnectedColor = new Color(1, 0, 1, 1);
    private Color _orangeTeamConnectedColor = new Color(1, 0.65f, 0, 1);
    private Color _greenTeamConnectedColor = new Color(0, 1, 0, 1);  


    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _infoPanelCanvasGroup.alpha = 1;
        _isGameStarted = true;
        ScoreText.text = _score.ToString();     
    }

    void Start()
    {      
        SceneManager.sceneLoaded += OnSceneLoaded;      

        MovesInRoundRemaining = (int)CreateAndJoinRooms.roomOptions.CustomRoomProperties["MovesPerRound"];
        SecondsInRoundRemaining = (float)CreateAndJoinRooms.roomOptions.CustomRoomProperties["SecondsPerRound"];
        TotalNumberOfRounds = (int)CreateAndJoinRooms.roomOptions.CustomRoomProperties["TotalNumberOfRounds"];
        PointsPerToggle = (int)CreateAndJoinRooms.roomOptions.CustomRoomProperties["PointsPerToggle"];
        PointsPerEndpoint = (int)CreateAndJoinRooms.roomOptions.CustomRoomProperties["PointsPerEndpoint"];
        NumberOfStealsRemaining = (int)CreateAndJoinRooms.roomOptions.CustomRoomProperties["TotalNumberOfSteals"];

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
        if (_isGameStarted && !_isGameEnded)
        {         

            // Time remaining
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                SecondsInRoundRemaining -= Time.deltaTime;
                if (SecondsInRoundRemaining > 0)
                {
                    photonView.RPC("DecreaseTimeRemaining", RpcTarget.All, SecondsInRoundRemaining);
                }

                if (SecondsInRoundRemaining < 0)
                {
                    if (_roundNumber == TotalNumberOfRounds)
                    {
                        Debug.Log("End of game");
                        photonView.RPC("EndGame", RpcTarget.All);
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
        if (_isGameStarted && !_isGameEnded)
        { 
            photonView.RPC("UpdateScoreRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    void UpdateScoreRPC()
    {
        Debug.Log("update score");

        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name != "Teacher")
        {
            if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Yellow")
            {
                if (GameObject.FindGameObjectsWithTag("Yellow Team StartPoint").Length > 0)
                {
                    Transform[] children = GameObject.FindGameObjectsWithTag("Yellow Team StartPoint")[0].transform.GetComponentsInChildren<Transform>();
                    int numberOfToggles = 0;
                    int numberOfEndPoints = 0;

                  //  Debug.Log("children.Length: " + children.Length);

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

                    _score = (numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint);
                    //  RoundScore = ((numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint)) - _totalScore;
                    
                    Debug.Log("_totalScore: " + _score);
                }
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Blue")
            {
                if (GameObject.FindGameObjectsWithTag("Blue Team StartPoint").Length > 0)
                {
                    Transform[] children = GameObject.FindGameObjectsWithTag("Blue Team StartPoint")[0].transform.GetComponentsInChildren<Transform>();
                    int numberOfToggles = 0;
                    int numberOfEndPoints = 0;
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
                    //RoundScore = ((numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint)) - _totalScore;
                    _score = (numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint);
                }
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Red")
            {
                if (GameObject.FindGameObjectsWithTag("Red Team StartPoint").Length > 0)
                {
                    Transform[] children = GameObject.FindGameObjectsWithTag("Red Team StartPoint")[0].transform.GetComponentsInChildren<Transform>();
                    int numberOfToggles = 0;
                    int numberOfEndPoints = 0;
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
                    //    RoundScore = ((numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint)) - _totalScore;
                    _score = (numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint);
                }
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Purple")
            {
                if (GameObject.FindGameObjectsWithTag("Purple Team StartPoint").Length > 0)
                {
                    Transform[] children = GameObject.FindGameObjectsWithTag("Purple Team StartPoint")[0].transform.GetComponentsInChildren<Transform>();
                    int numberOfToggles = 0;
                    int numberOfEndPoints = 0;
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
                    //  RoundScore = ((numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint)) - _totalScore;
                    _score = (numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint);
                }
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Orange")
            {
                if (GameObject.FindGameObjectsWithTag("Orange Team StartPoint").Length > 0)
                {
                    Transform[] children = GameObject.FindGameObjectsWithTag("Orange Team StartPoint")[0].transform.GetComponentsInChildren<Transform>();
                    int numberOfToggles = 0;
                    int numberOfEndPoints = 0;
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
                    // RoundScore = ((numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint)) - _totalScore;
                    _score = (numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint);
                }
            }
            else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == "Green")
            {
                if (GameObject.FindGameObjectsWithTag("Green Team StartPoint").Length > 0)
                {
                    Transform[] children = GameObject.FindGameObjectsWithTag("Green Team StartPoint")[0].transform.GetComponentsInChildren<Transform>();
                    int numberOfToggles = 0;
                    int numberOfEndPoints = 0;
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
                    //   RoundScore = ((numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint)) - _totalScore;
                    _score = (numberOfToggles * PointsPerToggle) + (numberOfEndPoints * PointsPerEndpoint);
                }
            }

            NumberOfStealsRemainingText.text = NumberOfStealsRemaining.ToString();
            //RoundScoreText.text = RoundScore.ToString();

            ScoreText.text = _score.ToString();

            _movesRemainingText.text = MovesInRoundRemaining.ToString();
            if (MovesInRoundRemaining == 0)
            {
                if (_roundNumber == TotalNumberOfRounds)
                {
                    Debug.Log("End of game");
                    photonView.RPC("EndGame", RpcTarget.All);
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
            MovesInRoundRemaining = (int)CreateAndJoinRooms.roomOptions.CustomRoomProperties["MovesPerRound"];
            SecondsInRoundRemaining = (float)CreateAndJoinRooms.roomOptions.CustomRoomProperties["SecondsPerRound"];
        }
    }

    [PunRPC]
    void EndGame()
    {
      //  _totalScore = _totalScore + RoundScore;
        ScoreText.text = _score.ToString();

     //   RoundScore = 0;
     //   RoundScoreText.text = RoundScore.ToString();

        _isGameEnded = true;
    }
}
