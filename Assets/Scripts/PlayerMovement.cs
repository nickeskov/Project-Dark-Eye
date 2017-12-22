using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using UnityStandardAssets.CrossPlatformInput;
class PlayerMovement : MonoBehaviour
{
    public float Speed = 5f;

    private float _speedForRunning;
    private Vector3 _movement;

    // private Rigidbody rigbod;
    public float CurrentSpeed;
    CharacterController charT;
    public Slider SpeedSlider;

    // Use this for initialization
    void Awake()
    {
        charT = GetComponent<CharacterController>();
        // rigbod = GetComponent<Rigidbody>();
        CurrentSpeed = Speed;
        _speedForRunning = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        RunControl(GameStatsControl.IsTired);
        float inputUp = Input.GetAxisRaw("Vertical");

        float inputLeft = Input.GetAxisRaw("Horizontal");
        Move(inputUp, inputLeft);
    }

    void Move(float up, float left)
    {
        Vector3 movementX = up * CurrentSpeed * Vector3.forward * Time.deltaTime;
        Vector3 movementY = left * CurrentSpeed * Vector3.right * Time.deltaTime;
        //  Vector3 NewPosition = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), 0f, CrossPlatformInputManager.GetAxis("Vertical"));
        //  movement = transform.TransformDirection(NewPosition);
        //  movement = movement.normalized * currentSpeed * Time.deltaTime;
        _movement = transform.TransformDirection(movementX + movementY);
        _movement = _movement.normalized * CurrentSpeed * Time.deltaTime;
        //rigbod.MovePosition(transform.position + movement);
        charT.Move(_movement);
    }

    public void RunControl(bool isTired)
    {
        if (SpeedSlider.value <= 0f) return;

        if (isTired)
        {
            CurrentSpeed = 5f;
        }
        CurrentSpeed = Input.GetButton("Fire3") ? 10f : 5f;
    }

}


