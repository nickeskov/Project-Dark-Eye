using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

class GameStatsControl : MonoBehaviour
{
    #region FireSlider variables
    public static bool IsFired = true;

    public Slider FireSlider;
    public GameObject FireInHand;
    private float _timer = 0f;

    public ParticleSystem FireLight;
    public static float FireValue = 0.1f;
    #endregion

    #region Running Variables
    public static bool IsTired = false;

    public PlayerMovement SpeedRun;
    public Slider AccelerationSlider;

    private float _timerForRun = 0f;
    private Image _myRunStripe;
    private MouseLooker _playerLooker;
    #endregion

    #region Brain Variables
    public Image BrainIcon;
    public Sprite[] BrainIconStages = new Sprite[4];

    private float _timerForBrain;
    public enum BrainStages
    {
        Warning1, Warning2, Warning3
    }
    public Image ScreenImage;
    public string currentSatge;
    #endregion
    public static bool OnFired = false;
    #region AudioVariables
    public AudioMixerSnapshot Calm;
    public AudioMixerSnapshot Dang1;
    public AudioMixerSnapshot Dang2;
    public AudioMixerSnapshot Dang3;
    public AudioMixerSnapshot Death;

    private const float Bmp = 128;
    private float _transitionIn;
    private float _transitionOut;
    #endregion

    public Text ScaryTextFeel;
    public Canvas NextLevelCanvas;


    void Start()
    {
        _timerForBrain = 0f;
        StartCoroutine(CalmState());
        _playerLooker = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLooker>();
    }

    void Update()
    {
        // if (!IsFired) { if (OnFired) { IsFired = true; } }

        FireSliderControl(FireValue);
        RunSliderControl();
        IsCheatActive();
    }

    void IsCheatActive()
    {
        if (Input.GetKeyDown("space"))
        {
            ChangeLevel(3);
        }
        else if (Input.GetKeyDown("backspace"))
        {
            ChangeLevel(2);
        }
    }

    void FireSliderControl(float fireValue)
    {
        if (IsFired)
        {
            if (_timer >= 1.5f)
            {
                FireSlider.value -= fireValue;
                _timer = 0f;
            }
        }
        else
        {
            if (FireSlider.value >= 95f)
            {
                Animator anim = FireInHand.GetComponent<Animator>();
                int p = Animator.StringToHash("FireOn");
                anim.SetTrigger("FireOn");
                IsFired = true;
                Debug.Log(IsFired.ToString() + " IsFired" + p.ToString());
                Invoke("FireHandActivity", 2f);
            }
        }
        if (FireSlider.value <= 0f)
        {
            Animator anim = FireInHand.GetComponent<Animator>();
            int p = Animator.StringToHash("FireOff");
            anim.Play(p);
            Invoke("FireHandActivity", 2f);
            IsFired = false;
            Debug.Log(IsFired.ToString() + " IsFired" + p.ToString());

        }
        _timer += Time.deltaTime;
    }

    void RunSliderControl()
    {
        _timerForRun += Time.deltaTime;

        if (SpeedRun.CurrentSpeed >= 7f && !IsTired)
        {
            AccelerationSlider.value -= 4f;
            _timerForRun = 0f;
        }

        if (AccelerationSlider.value <= 0f)
        {
            IsTired = true;
            Debug.Log(SpeedRun.CurrentSpeed.ToString());
            _myRunStripe = AccelerationSlider.GetComponentInChildren<Image>();
            _myRunStripe.color = new Color(1f, 0f, 0f, 255f);
        }

        if (_timerForRun > 2f)
        {
            if (IsTired)
            {
                _myRunStripe.color = Color.clear;
                IsTired = false;
            }
            AccelerationSlider.value += .8f;
        }
    }

    void BrainControl()
    {
        if (_timerForBrain >= 4f && currentSatge == BrainStages.Warning1.ToString())
        {
            BrainIcon.sprite = BrainIconStages[0];
            ScreenImage.color = Color.Lerp(ScreenImage.color, new Color(1f, 0f, 0f, 0.3f), 4.5f * Time.deltaTime);
            currentSatge = BrainStages.Warning2.ToString();
            Invoke("ColorReset", 0.3f);
            _timerForBrain = 0f;
        }

        if (_timerForBrain > 8f && currentSatge == BrainStages.Warning2.ToString())
        {
            currentSatge = BrainStages.Warning3.ToString();
            BrainIcon.sprite = BrainIconStages[1];
            _timerForBrain = 0f;
        }

        if (_timerForBrain > 12f && currentSatge == BrainStages.Warning3.ToString())
        {

            BrainIcon.sprite = BrainIconStages[2];
            _timerForBrain = 0f;
        }
    }

    Color ColorReset()
    {
        return ScreenImage.color = Color.Lerp(ScreenImage.color, Color.clear, 1f);
    }

    public IEnumerator CalmState()
    {
        Calm.TransitionTo(60 / Bmp);
        Debug.Log("CalmStage");
        bool spec = true;

        while (spec)
        {
            if (IsFired)
                yield return null;
            else
            {
                _timerForBrain += Time.deltaTime;
                if (_timerForBrain >= 7f) { spec = false; }
                yield return null;
            }
        }

        if (_timerForBrain >= 7f && !IsFired)
        {
            _timerForBrain = 0f;
            BrainIcon.sprite = BrainIconStages[0];
            ScreenImage.color = Color.Lerp(ScreenImage.color, new Color(1f, 0f, 0f, 0.3f), 4.5f * Time.deltaTime);
            Invoke("ColorReset", 0.3f);
            Debug.Log("DangerousStage");
            StartCoroutine(StageDangerous1());
            yield break;
        }
    }

    public IEnumerator StageDangerous1()
    {
        Dang1.TransitionTo(60 / Bmp);
        ScaryTextFeel.gameObject.SetActive(true);
        Invoke("FadeText", 2f);
        float timeForCalming = 0f;
        bool spec = true;
        while (spec)
        {
            if (IsFired)
            {
                _timerForBrain = 0f;
                if (_timerForBrain == 0f)
                {
                    timeForCalming += Time.deltaTime;
                    if (timeForCalming >= 5f)
                    {
                        StartCoroutine(CalmState());
                        yield break;
                    }
                }
                yield return null;
            }
            else
            {
                _timerForBrain += Time.deltaTime;
                if (_timerForBrain >= 7f)
                {
                    spec = false;
                }
                yield return null;
            }
        }

        _timerForBrain = 0f;
        BrainIcon.sprite = BrainIconStages[1];
        ScreenImage.color = Color.Lerp(ScreenImage.color, new Color(1f, 0f, 0f, 0.3f), 4.5f * Time.deltaTime);
        Invoke("ColorReset", 0.3f);
        StartCoroutine(StageDangerous2());
        yield break;
    }

    public IEnumerator StageDangerous2()
    {
        Dang2.TransitionTo(60 / Bmp);
        ScaryTextFeel.gameObject.SetActive(true);
        Invoke("FadeText", 2f);
        bool spec = true;
        float timeForCalming = 0f;

        while (spec)
        {
            if (IsFired)
            {
                _timerForBrain = 0f;
                if (_timerForBrain == 0f)
                {
                    timeForCalming += Time.deltaTime;
                    if (timeForCalming >= 5f)
                    {
                        StartCoroutine(StageDangerous1());
                        yield break;
                    }
                }
                yield return null;
            }
            else
            {
                _timerForBrain += Time.deltaTime;
                if (_timerForBrain >= 8f)
                {
                    spec = false;
                }
                yield return null;
            }
        }
        _timerForBrain = 0f;
        BrainIcon.sprite = BrainIconStages[2];
        ScreenImage.color = Color.Lerp(ScreenImage.color, new Color(1f, 0f, 0f, 0.3f), 4.5f * Time.deltaTime);
        Invoke("ColorReset", 0.3f);
        StartCoroutine(StageDangerousCritical());
    }

    public IEnumerator StageDangerousCritical()
    {
        Dang3.TransitionTo(60 / Bmp);
        ScaryTextFeel.gameObject.SetActive(true);
        Invoke("FadeText", 2f);
        bool spec = true;
        float timeForCalming = 0f;

        while (spec)
        {
            if (IsFired)
            {
                _timerForBrain = 0f;
                if (_timerForBrain == 0f)
                {
                    timeForCalming += Time.deltaTime;
                    if (timeForCalming >= 5f)
                    {
                        StartCoroutine(StageDangerous2());
                        yield break;
                    }
                }
                yield return null;
            }
            else
            {
                _timerForBrain += Time.deltaTime;
                if (_timerForBrain >= 7f)
                {
                    spec = false;
                }
                yield return null;
            }
        }

        _timerForBrain = 0f;
        BrainIcon.sprite = BrainIconStages[3];
        ScreenImage.color = Color.Lerp(ScreenImage.color, new Color(1f, 0f, 0f, 0.3f), 4.5f * Time.deltaTime);
        Invoke("ColorReset", 0.5f);
        StartCoroutine(StageMadnessDeath());
    }

    public IEnumerator StageMadnessDeath()
    {
        Death.TransitionTo(60 / Bmp);
        _timerForBrain += Time.deltaTime;
        bool spec = true;
        float timeForCalming = 0f;

        while (spec)
        {
            if (IsFired)
            {
                _timerForBrain = 0f;
                if (_timerForBrain == 0f)
                {
                    timeForCalming += Time.deltaTime;
                    if (timeForCalming >= 5f)
                    {
                        StartCoroutine(StageDangerousCritical());
                        yield break;
                    }
                }
                yield return null;
            }
            else
            {
                _timerForBrain += Time.deltaTime;
                if (_timerForBrain >= 7f)
                {
                    spec = false;
                }
                yield return null;
            }
        }
        ScreenImage.color = Color.Lerp(ScreenImage.color, new Color(1f, 0f, 0f, 0.3f), 4.5f * Time.deltaTime);
    }


    void FadeText()
    {
        ScaryTextFeel.gameObject.SetActive(false);
    }


    public void ChangeLevel(int lvl)
    {

       // int lvl = Application.loadedLevel(); этот метод-obsolete, нужно заменить потом
       // if(lvl == 2) lvl = 3;
        SceneManager.LoadScene(lvl);
    }


    public void BlockActivity()
    {
        NextLevelCanvas.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SpeedRun.enabled = false;
        _playerLooker.enabled = false;
        gameObject.SetActive(false);
    }

    public void FireHandActivity()
    {
        if (!IsFired)
        {
            FireLight.Stop(true);
            FireLight.gameObject.SetActive(false);
        }
        else
        {
            FireLight.Play(true);
        }
    }
}
