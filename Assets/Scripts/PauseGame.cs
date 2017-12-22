using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class PauseGame : MonoBehaviour
{
    private bool _pause;
    public Canvas CanV;
    public PlayerMovement PlayerMovement;
    public MouseLooker mouselook;

    public AudioSource aud;

    // Use this for initialization
    void Start()
    {
        _pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetButton("Cancel")) return;

        PlayerMovement.enabled = false;

        CanV.gameObject.SetActive(true);
        mouselook.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnContinue()
    {
        if (!CanV.gameObject.active) return;

        PlayerMovement.enabled = true;
        mouselook.enabled = true;
        CanV.gameObject.SetActive(false);
    }
    public void OnOut()
    {
        SceneManager.LoadScene(0);
    }
}
