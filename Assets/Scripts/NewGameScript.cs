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
        float TimeForBegin = 0f;
        while (TimeForBegin <= 3f) TimeForBegin += Time.deltaTime;

        SceneManager.LoadSceneAsync(SceneID);
    }

    public void OutFromGame()
    {
        float Times = 0f;
        while (Times <= 1.5f) Times += Time.deltaTime;
        Application.Quit();
    }
}
