using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Exit")
        {

            int i = SceneManager.GetActiveScene().buildIndex;
            if(i==2) SceneManager.LoadScene(7);
            else if (i == 3) SceneManager.LoadScene(8);
            else if (i == 5) SceneManager.LoadScene(9);
        }
    }
}

