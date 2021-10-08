using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class P_Movement : MonoBehaviour
    {
        public float moveSpeed = 9f, sensitivity = 100f;
        private float horizontalInput, verticalInput;
        private Vector3 forwardVector, sideVector, upVector;
        //up is negative down is positive?
        private float camX, camXMin = 0.60f, camXMax = -0.40f;
        private float jumpPower = 10, slidePower = 1;
        [SerializeField] private GameObject Head, Player;
        [SerializeField] private Rigidbody rb;

        //private Vector3 vectorSum; will be used to limit max speed of natural horizontal movement
        
        private float slideEnd;
        private int fastLerp = 4;


        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            #region Initialisation
            //inputs
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");


            var camRotation = Camera.main.transform.rotation;
            camX = -Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
           
            #endregion   
        
            #region MovementControl
            //cardinal directions
            forwardVector = Player.transform.forward * slidePower * verticalInput * moveSpeed;
            sideVector = Player.transform.right * slidePower * horizontalInput * moveSpeed;
            
            #region Slide
            if (Input.GetKeyDown(KeyCode.LeftShift) && slidePower <= 1f)
            {
                slidePower = 6f;
                StartCoroutine(nameof(Slide));
            }
            #endregion

            #region Jump
            if (Input.GetKeyDown(KeyCode.Space) && P_JumpPing.grounded)
            {
                //this attempts to get the jump some directional oomf
                StopCoroutine(nameof(Slide));
                
                // attempts to cancel downward momentum
                upVector = Vector3.zero;
                
                //up force
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                StartCoroutine(nameof(Slide));
                P_JumpPing.grounded = false;
            }
            #endregion
            #endregion

            #region CameraControl
            //rotates player based on camera
            rb.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime, 0);
        
            //camera verticality
            
            Head.transform.Rotate(camX, 0, 0);
            //if(Head.transform.rotation < camXMax)

            #endregion
        }
    
        void FixedUpdate()
        {
            // here to be used as some sort of magnitude limiter
            //vectorSum = forwardVector + sideVector;
            
            
            upVector = new Vector3(0, rb.velocity.y, 0);
            rb.velocity = forwardVector + sideVector + upVector;
        }

        private IEnumerator Slide()
        {
            while (slidePower > 1)
            {
                slidePower -= 0.4f;
                if (slidePower < 1)
                    slidePower = 1;
                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }
    }
}

