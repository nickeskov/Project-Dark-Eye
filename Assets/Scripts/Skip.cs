using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Skip : MonoBehaviour {

    public int SceneID = 2;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space")) SceneManager.LoadScene(SceneID);
	}
}
