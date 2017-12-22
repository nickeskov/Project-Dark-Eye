using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class TextFading : MonoBehaviour
{
    private Animator _anim;
    private float _timer;

    public GameObject NextText;

    // Use this for initialization
    void Start()
    {
        _timer = 0f;
        _anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        MakeText();
    }

    void MakeText()
    {
        _anim.SetTrigger("TimeToChangeText");
        if (_timer >= 6f)
        {
            DestroyImmediate(gameObject);

            if (NextText != null)
            {
                NextText.SetActive(true);

            }

        }
        _timer += Time.deltaTime;
    }
}
