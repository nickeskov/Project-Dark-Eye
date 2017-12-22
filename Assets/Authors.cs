using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Authors : MonoBehaviour
{
    public int SceneID = 4;

    public void SeeAuthors()
    {
        float TimeForBegin = 0f;
        while (TimeForBegin <= 3f) TimeForBegin += Time.deltaTime;

        SceneManager.LoadSceneAsync(SceneID);
    }

    public void ReturnToMenu()
    {
        float Times = 0f;
        while (Times <= 1.5f) Times += Time.deltaTime;
        SceneManager.LoadSceneAsync(sceneBuildIndex: 0);
    }
}
