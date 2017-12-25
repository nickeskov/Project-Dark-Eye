using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class PauseGame : MonoBehaviour
{
    private bool _pause;
    private float _timer;
    public Canvas CanV;
    public PlayerMovement PlayerMovement;
    public MouseLooker mouselook;

    public AudioSource aud;

    // Use this for initialization
    void Start()
    {
        _pause = false;
        _timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetButton("Cancel") || GameStatsControl._dead)
        {
            return;
        }

        PlayerMovement.enabled = false;

        CanV.gameObject.SetActive(true);
        mouselook.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        /*if (GameStatsTranslator.FireSlider == null)
        {
            return;
        }

        if (GameStatsTranslator._timerForBrain > 0f)
        {
            GameStatsTranslator._timerForBrain -= Time.deltaTime;
        }

        if (GameStatsControl.IsFired)
        {
            if (_timer >= 1.5f)
            {
                GameStatsTranslator.FireSlider.value += GameStatsControl.FireValue;
                _timer = 0f;
            }
            _timer += Time.deltaTime;
        }*/
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
