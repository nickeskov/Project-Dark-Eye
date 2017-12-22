using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FireController : MonoBehaviour
{
    //  bool IsFired;
    Animator anim;
    float Timer;
    void Start()
    {
        anim = GetComponent<Animator>();

        Timer = 0f;
    }
    private void Update()
    {

    }

    public void FireOnOff()
    {
        if (GameStatsControl.IsFired)
        {
            anim.SetTrigger("FireOff");
            while (Timer <= 3.5f)
            {
                Timer += Time.deltaTime;
            }
            gameObject.SetActive(false);
            GameStatsControl.IsFired = false;
            GameStatsControl.FireValue = 0f;
        }
        else
        {
            gameObject.SetActive(true);
            anim.SetTrigger("FireOn");
            GameStatsControl.IsFired = true;
            GameStatsControl.FireValue = 5f;

        }
    }
}
