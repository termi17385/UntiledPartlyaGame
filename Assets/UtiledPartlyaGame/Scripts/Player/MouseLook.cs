using System;
using UnityEngine;

namespace UtiledPartlyaGame.Player
{
	public class MouseLook : MonoBehaviour
	{
		[SerializeField] private Transform head;
		[SerializeField] private Joystick mobileRightJoystick;
		[SerializeField, Range(0, 500)] public float sensitivity = 300f;
		[SerializeField, Range(0, 500)] public float arrowSensitivity = 200f;
		[SerializeField, Range(0, 500)] private float joystickSensitivity = 250f;
		[SerializeField] private float minY = -60, maxY = 60;

		[SerializeField] private InputMethod inputMethod = InputMethod.MouseAndKeyboard;
		[SerializeField] private bool MouseUnlocked;
	
		private float rotY;
		private bool isUsingMenu = false;
		private PlayerController player;

		private void Start()
		{
			MouseUnlocked = true;
			FirstInputMethodCheck();
			player = GetComponent<PlayerController>();
		}


		private void Update()
		{
			if (!isUsingMenu)
			{
				if(!player.isDead) MouseMovementStandard();
				else if (player.isDead) FlightCamMouse();
			
				//Unlocks the mouse and stops camera rotation so you can use menus
				if(inputMethod == InputMethod.MouseAndKeyboard)
				{
					if(Cursor.visible)
					{
						MouseUnlocked = true;
						Cursor.lockState = CursorLockMode.None;
						if(Input.GetKeyDown(KeyCode.Mouse0))
						{
							Cursor.visible = false;
						}
					}
					else
					{
						MouseUnlocked = false;
						Cursor.lockState = CursorLockMode.Confined;
					}

				}
				else if(inputMethod == InputMethod.Mobile)
				{
					MouseUnlocked = true;
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = false;
				}

			}
			else
			{
				// Locks cursor again so you can use mouse to look around
				//////if (Input.GetKeyDown(KeyCode.Escape))
				//////{
				//////	Cursor.lockState = CursorLockMode.Locked;
				//////	Cursor.visible = false;
				//////	isUsingMenu = false;
				//////}
			}
			
		}

	#region InputMethodChecks

		private void FirstInputMethodCheck()
		{
		#if UNITY_IOS || UNITY_ANDROID
			inputMethod = InputMethod.Mobile;
			TurnOnMobileInput();
		#else
			inputMethod = InputMethod.MouseAndKeyboard;
			TurnOnMouseAndKeyboardInput();
		#endif
		}

		private void TurnOnMobileInput()
		{
			// Turn on mobile joystick.
			mobileRightJoystick.gameObject.SetActive(true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = false;
		}

		private void TurnOnMouseAndKeyboardInput()
		{
			// Turn off Mobile Joystick.
			mobileRightJoystick.gameObject.SetActive(false);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	#endregion

	#region Controls
		/// <summary> Alters the mouse controls slightly
		/// so that it works with the flight controls </summary>
		private void FlightCamMouse()
		{
			Debug.Log("You are dead -" + gameObject.name);
			// Turning this off because we will just turn off their player controller and rigidbody reset them, and turn them on again. ~ Kieran 10:17 Monday 29/11/21.
			// var rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
			// var newRotY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
			// transform.localEulerAngles = new Vector3(newRotY, rotX, 0f);
		}

		/// <summary> Standard mouse
		/// look controls </summary>
		private void MouseMovementStandard()
		{
			float xRotation = 0;
			
			// handles the characters movement
			if(inputMethod == InputMethod.Mobile)
			{
				if(mobileRightJoystick.gameObject != null)
				{
					if(mobileRightJoystick.gameObject.active)
					{
						float TurnWithJoystick = mobileRightJoystick.Horizontal * joystickSensitivity;
						xRotation = TurnWithJoystick;
					}
				}
			}
			else if(inputMethod == InputMethod.MouseAndKeyboard)
			{
				if(!MouseUnlocked)
				{
					float mouseX = Input.GetAxis("Mouse X") * sensitivity;
					// We can add the Turning with Arrow To on screen arrows.
					float TurnWithArrows = Input.GetAxis($"ArrowsTurning") * arrowSensitivity;
					xRotation = mouseX * mouseX > TurnWithArrows * TurnWithArrows ? mouseX : TurnWithArrows;
				}
			}
			else
			{
				Debug.Log("No input system found!!", gameObject);
			}
			//xRotation = TurnWithJoystick;
			//Debug.Log($"Turn w/ joystick = {xRotation} --- mobileRightJoystick.Horizontal = {mobileRightJoystick.Horizontal}");
			//xRotation = TurnWithArrows;
			//float keyboardX = Input.GetAxis() * sensitivity;
			
			// We are only moving in the X Rotation now ~ Kieran 10:17 Monday 29/11/21.
			//float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
			
			transform.Rotate(0, xRotation * Time.deltaTime, 0);

			// handles the camera's movement
			//rotY -= mouseY * Time.deltaTime;
			rotY = Mathf.Clamp(rotY, minY, maxY);

			// the head of the player (camera /or actual head)
			head.localRotation = Quaternion.Euler(rotY, 0, 0);
		}
	#endregion
	}

	/// <summary> This is the input system the player is currently using. </summary>
}
	public enum InputMethod{ Mobile, MouseAndKeyboard };
