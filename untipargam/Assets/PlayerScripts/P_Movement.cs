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
        private float camX, camXMin = -0.40f, camXMax = 0.60f;
        private float jumpPower = 10, slidePower;
        [SerializeField] private GameObject Head;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private bool grounded;
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
            forwardVector = transform.TransformVector(0, 0, 1 + slidePower)* verticalInput * moveSpeed;
            sideVector = transform.TransformVector(1 + slidePower,0,0) * horizontalInput * moveSpeed;
            
            #region Slide
            if (Input.GetKeyDown(KeyCode.LeftShift) && slidePower <= 1f)
            {
                slidePower = 6f;
                StartCoroutine(nameof(Slide));
            }
            #endregion

            #region Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopCoroutine(nameof(Slide));
                upVector = Vector3.zero;
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                StartCoroutine(nameof(Slide));
            }
            #endregion
            #endregion

            #region CameraControl
            //rotates player based on camera
            rb.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime, 0, Space.World);
        
            //camera verticality
            
            Head.transform.Rotate(camX, 0, 0, Space.Self);
            

            #endregion
        }
    
        void FixedUpdate()
        {
            upVector = new Vector3(0, rb.velocity.y, 0);
            rb.velocity = forwardVector + sideVector + upVector;
        }

        #region JumpReset
        private void OnCollisionExit(Collision other)
        {
            if(other.gameObject.CompareTag("Ground"))
                grounded = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Ground")) return;
            
            grounded = true;
        }
        #endregion
        
        private IEnumerator Slide()
        {
            while (slidePower > 0)
            {
                slidePower -= 0.4f;
                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }
    }
}

