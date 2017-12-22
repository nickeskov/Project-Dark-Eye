using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class BrainStages : MonoBehaviour
{
    private Image _brainStageIcon;
    private float _timer;

    public Image CanX;
    public Sprite[] BrainStage=new Sprite[5];
    public string CurrentStage;

    public enum Stages {
        Stage1, 
        Stage2,
        Stage3
    }

	// Use this for initialization
	void Start ()
    {
        _brainStageIcon = GetComponent<Image>();
     //   CanX = gameObject.GetComponentInParent<Image>();
            _timer = 0f;
        CurrentStage = Stages.Stage1.ToString();
	}
	
	// Update is called once per frame
	void Update ()
    {
        _timer += Time.deltaTime;

        if (_timer >= 3f && CurrentStage== Stages.Stage1.ToString())
        {
            CanX.color = Color.Lerp(CanX.color, new Color(1f, 0f, 0f, .7f), 4.5f*Time.deltaTime);
            _brainStageIcon.sprite = BrainStage[0];
            CurrentStage = Stages.Stage2.ToString();
            Invoke("ColorReset", 0.2f);

            _timer = 0f;
        }

        if (_timer >= 7f && CurrentStage == Stages.Stage2.ToString())
        {
            _brainStageIcon.sprite = BrainStage[1];
            CurrentStage = Stages.Stage3.ToString();
           
            _timer = 0f;

        }

        if (_timer >= 12f && CurrentStage == Stages.Stage3.ToString())
        {
            _brainStageIcon.sprite = BrainStage[1];
          
            _timer = 0f;
        }
    }

    Color ColorReset()
    {
        return CanX.color = Color.Lerp(CanX.color, Color.clear, 1f);
    }
}
