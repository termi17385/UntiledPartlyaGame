using System;
using UnityEngine;

namespace UtiledPartlyaGame.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        #region Main Variables
        /// <summary> The speed of character movement </summary>
        [Header("Main Variables"), SerializeField] private float speed = 10f;
        [SerializeField] private float vSpeed = 0;
        /// <summary> Speed of the jump </summary>
        [SerializeField] private float jumpSpeed = 15f;
        /// <summary> The force of gravity applied to the character </summary>
        [SerializeField] private float gravityModifier = 1f;
        [SerializeField] private Joystick mobileLeftJoystick;
        [SerializeField] private InputMethod inputMethod = InputMethod.MouseAndKeyboard;
        #endregion
        #region private Variables 
        /// <summary> The character controller attached to the player character's </summary>
        [Space] private CharacterController playerChar;
        private bool grounded;
        private bool resetGravity;
        private float gravity = 9.8f;
        #endregion
        #region public Variables
        public bool isDead;
        public GameObject[] hideObjects;
        #endregion
        #region Start Update
        private void Start()
        {
            FirstInputMethodCheck();
            // Sets playerChar to the CharacterController attached to the player
            playerChar = GetComponent<CharacterController>();
        }
        private void Update()
        {
            CheckInputPlatform();
            if(!isDead) PlayerMovement();
            else if (isDead) PlayerSpectatorMode();
        }
        #endregion
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

            /// <summary> Checks if player on a mobile platform </summary>
            private void CheckInputPlatform()
            {
                InputMethod oldUsingPlatform = inputMethod;
            #if UNITY_IOS || UNITY_ANDROID
			    inputMethod = InputMethod.Mobile;
            #else
                inputMethod = InputMethod.MouseAndKeyboard;
            #endif
			
                if(oldUsingPlatform != inputMethod)
                {
                    if(inputMethod == InputMethod.MouseAndKeyboard)
                        TurnOnMouseAndKeyboardInput();
                    if(inputMethod == InputMethod.Mobile)
                        TurnOnMobileInput();
                    else
                        Debug.Log("No input system found!!");
                }
            }

            private void TurnOnMobileInput()
            {
                // Turn on mobile joystick.
                mobileLeftJoystick.gameObject.SetActive(true);
            }

            private void TurnOnMouseAndKeyboardInput()
            {
                // Turn off Mobile Joystick.
                mobileLeftJoystick.gameObject.SetActive(false);
            }
        #endregion
            
        #region Movement Types
        /// <summary> Basic spectator flight
        /// when the player dies </summary>
        private void PlayerSpectatorMode()
        {
            Debug.Log("You are dead -" + gameObject.name);
            // Turning this off because we will just turn off their player controller and rigidbody reset them, and turn them on again. ~ Kieran 10:13 Monday 29/11/21.
            // var h = Input.GetAxis("Horizontal") * (speed * 100) * Time.deltaTime;
            // var v = Input.GetAxis("Vertical") * (speed * 100) * Time.deltaTime;
            //
            // transform.position += (transform.forward * v * Time.deltaTime);
            // transform.position += (transform.right * h * Time.deltaTime);
        }
        /// <summary> Handles the movement
        /// and jumping of the player </summary>
        private void PlayerMovement()
        {
            float horizontalMovement = 0 * speed;
            float verticalMovement = 0 * speed;

            if(inputMethod == InputMethod.MouseAndKeyboard)
            {
               horizontalMovement = Input.GetAxis("Horizontal") * speed;
               verticalMovement = Input.GetAxis("Vertical") * speed;
            }
            else if(inputMethod == InputMethod.Mobile)
            {
                if(mobileLeftJoystick.gameObject != null)
                {
                    if(mobileLeftJoystick.gameObject.active)
                    {
                        // these handle the input axis's
                        horizontalMovement = mobileLeftJoystick.Horizontal * speed;
                        verticalMovement = mobileLeftJoystick.Vertical * speed;
                    }
                }
            }
            else
            {
                Debug.Log("No input system found!!", gameObject);
            }

            // handles the movement and rotation
            //Vector3 vel = Quaternion.Euler(0, playerChar.transform.eulerAngles.y, 0) * new Vector3(horizontalMovement, 0, verticalMovement);
            Vector3 vel = Quaternion.Euler(0, playerChar.transform.eulerAngles.y, 0) * new Vector3(horizontalMovement, 0, verticalMovement);
            if(grounded) // checking if grounded
            {
                //todo set a 1 time velocity check thing 
                if(resetGravity) // making sure gravity is only reset once
                {
                    vSpeed = 0; // stop the character moving down when grounded
                    resetGravity = false; 
                }
                // Turning off Jumping ~ Kieran 10:08 Monday 29/11/21.
                // if(Input.GetKeyDown(KeyCode.Space)) // checking if the player has jumped
                // {
                //     Debug.Log("jump pressed");
                //     vSpeed = jumpSpeed; // sets the upwards velocity
                //     //el.y = vSpeed;
                //     //layerChar.Move(vel * Time.deltaTime);
                // }
                
                //Debug.Log("Controller" + playerChar.isGrounded);
                //Debug.Log("Collider" + grounded);
            }
            // if the player has left the ground
            if(!grounded)
            {
                // activates gravity
                vSpeed -= gravity * gravityModifier * Time.deltaTime;
                resetGravity = true; // primes the reset
            }
            vel.y = vSpeed;
            playerChar.Move(vel * Time.deltaTime);
        }
        #endregion
        #region Basic Ground Check
        private void OnTriggerEnter(Collider _other) { if(!_other.gameObject.CompareTag("Player")) grounded = true; }
        private void OnTriggerExit(Collider _other) { if(!_other.gameObject.CompareTag("Player")) grounded = false; }
        #endregion
    }
}
