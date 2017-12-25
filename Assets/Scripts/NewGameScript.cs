using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameScript : MonoBehaviour
{
    public int SceneID = 2;

    public void StartNewGame()
    {     
        SceneManager.LoadScene(SceneID);
    }

    public void OutFromGame()
    {
        GameStatsControl._dead = false;
        float Times = 0f;
        while (Times <= 1.5f) Times += Time.deltaTime;
        Application.Quit();
    }
}
