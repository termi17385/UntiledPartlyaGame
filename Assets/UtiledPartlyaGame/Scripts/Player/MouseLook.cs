using UnityEngine;

namespace UtiledPartlyaGame.Player
{
	public class MouseLook : MonoBehaviour
	{
		[SerializeField] private Transform head;
		[SerializeField, Range(0, 500)] public float sensitivity = 300;
		[SerializeField] private float minY = -60, maxY = 60;
	
		private float rotY;
		private bool isUsingMenu = false;
		private PlayerController player;

		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			player = GetComponent<PlayerController>();
		}

		private void Update()
		{
			if (!isUsingMenu)
			{
				if(!player.isDead) MouseMovementStandard();
				else if (player.isDead) FlightCamMouse();
			
				// Unlocks the mouse and stops camera rotation so you can use menus
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					isUsingMenu = true;
				}
			}
			else
			{
				// Locks cursor again so you can use mouse to look around
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
					isUsingMenu = false;
				}
			}
		}

	#region Controls
		/// <summary> Alters the mouse controls slightly
		/// so that it works with the flight controls </summary>
		private void FlightCamMouse()
		{
			var rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
			var newRotY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
			transform.localEulerAngles = new Vector3(newRotY, rotX, 0f);
		}

		/// <summary> Standard mouse
		/// look controls </summary>
		private void MouseMovementStandard()
		{
			// handles the characters movement
			var mouseX = Input.GetAxis("Mouse X") * sensitivity;
			var mouseY = Input.GetAxis("Mouse Y") * sensitivity;
			transform.Rotate(0, mouseX * Time.deltaTime, 0);

			// handles the camera's movement
			rotY -= mouseY * Time.deltaTime;
			rotY = Mathf.Clamp(rotY, minY, maxY);

			// the head of the player (camera /or actual head)
			head.localRotation = Quaternion.Euler(rotY, 0, 0);
		}
	#endregion
	}
}