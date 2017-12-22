using UnityEngine;
using System.Collections;
public class MouseLooker : MonoBehaviour
{
	// Use this for initialization
	public float XSensitivity = 2f;
	public float YSensitivity = 2f;
	public bool clampVerticalRotation = true;
	public float MinimumX = -90F;
	public float MaximumX = 90F;
	public bool smooth;
	public float smoothTime = 5f;
	
	// internal private variables
	private Quaternion _mCharacterTargetRot;
	private Quaternion _mCameraTargetRot;
	private Transform _character;
	private Transform _cameraTransform;

	void Start() {
		// start the game with the cursor locked
		LockCursor (true);

		// get a reference to the character's transform (which this script should be attached to)
		_character = gameObject.transform;

		// get a reference to the main camera's transform
		_cameraTransform = Camera.main.transform;

		// get the location rotation of the character and the camera
		_mCharacterTargetRot = _character.localRotation;
		_mCameraTargetRot = _cameraTransform.localRotation;
	}
	
	void Update() {
		// rotate stuff based on the mouse
		LookRotation ();

		// if ESCAPE key is pressed, then unlock the cursor
		if(Input.GetButtonDown("Jump"))
        {
			LockCursor (false);
		}

		// if the player fires, then relock the cursor
		if(Input.GetButtonDown("Fire1"))
        {
			LockCursor (true);
		}
	}
	
	private void LockCursor(bool isLocked)
	{
		if (isLocked) 
		{
			// make the mouse pointer invisible
			Cursor.visible = false;

			// lock the mouse pointer within the game area
			Cursor.lockState = CursorLockMode.Locked;
		}
        else
        {
			// make the mouse pointer visible
			Cursor.visible = true;

			// unlock the mouse pointer so player can click on other windows
			Cursor.lockState = CursorLockMode.None;
		}
	}

	public void LookRotation()
	{
		//get the y and x rotation based on the Input manager
		float yRot = Input.GetAxis("Mouse X") * XSensitivity;
		float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

		// calculate the rotation
		_mCharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
		_mCameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

		// clamp the vertical rotation if specified
		if(clampVerticalRotation)
			_mCameraTargetRot = ClampRotationAroundXAxis (_mCameraTargetRot);

		// update the character and camera based on calculations
		if(smooth) // if smooth, then slerp over time
		{
			_character.localRotation = Quaternion.Slerp (_character.localRotation, _mCharacterTargetRot,
			                                            smoothTime * Time.deltaTime);
			_cameraTransform.localRotation = Quaternion.Slerp (_cameraTransform.localRotation, _mCameraTargetRot,
			                                         smoothTime * Time.deltaTime);
		}
		else // not smooth, so just jump
		{
			_character.localRotation = _mCharacterTargetRot;
			_cameraTransform.localRotation = _mCameraTargetRot;
		}
	}
	
	// Some math ... eeck!
	Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;
		
		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);
		
		angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);
		
		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);
		
		return q;
	}
}
