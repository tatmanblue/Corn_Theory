using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
        private Transform mainCamera;                  // A reference to the main camera in the scenes transform
        private Vector3 cameraForwardDirection;             // The current forward direction of the camera
        private Vector3 movementVector;
        private bool isJumping;                      // the world-relative desired move direction, calculated from the camForward and user input.

        
        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                mainCamera = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!isJumping)
            {
                isJumping = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (mainCamera != null)
            {
                // calculate camera relative direction to move:
                cameraForwardDirection = Vector3.Scale(mainCamera.forward, new Vector3(1, 0, 1)).normalized;
                movementVector = v * cameraForwardDirection + h * mainCamera.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                movementVector = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) movementVector *= 0.5f;
#endif

            // pass all parameters to the character control script
            character.Move(movementVector, crouch, isJumping);
            isJumping = false;

            
        }
    }
}
