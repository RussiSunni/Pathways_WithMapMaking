using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadMapMaker()
    {
        SceneManager.LoadScene("MapMaker");
    }

    public void LoadPlayer()
    {
        SceneManager.LoadScene("Player");
    }
}
