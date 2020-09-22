// Author: Devon Wayman
// Date: Sept 21 2020
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Takes input from the VR controller and allows player to move around the scene freely 
/// (if canMove bool is true)
/// </summary>
namespace WWIIVR.Interaction.Player {
    public class MovmentProvider : LocomotionProvider {

        public float playerSpeed = 1.7f;
        public float gravityMultiplier = 1f;
        [SerializeField] private bool canMove = true;

        // List of XR controllers
        public List<XRController> controllers = null;

        private CharacterController characterController = null;
        private GameObject head = null;


        protected override void Awake() {
            characterController = GetComponent<CharacterController>();
            head = GetComponent<XRRig>().cameraGameObject;
        }

        private void Start() {
            PositionController();
        }

        private void FixedUpdate() {
            PositionController();

            if (!canMove) return;
            CheckForInput();
            ApplyGravity();
        }

        private void PositionController() {
            // Get head in local playspace to ground
            float headHeight = Mathf.Clamp(head.transform.localPosition.y, 1, 2);
            characterController.height = headHeight;

            // Cut in half and add skin
            Vector3 newCenter = Vector3.zero;
            newCenter.y = characterController.height / 2;
            newCenter.y += characterController.skinWidth;

            // Move capsule to local space
            newCenter.x = head.transform.localPosition.x;
            newCenter.z = head.transform.localPosition.z;

            // Apply adjustments
            characterController.center = newCenter;
        }
        
        private void CheckForInput() {
            foreach (XRController controller in controllers) {
                // If input is redily available
                if (controller.enableInputActions) {
                    CheckForMovement(controller.inputDevice);
                }
            }
        }

        private void CheckForMovement(InputDevice device) {
            // Get joystick input
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position)) {
                StartMove(position);
            }
        }

        private void StartMove(Vector2 position) {
            // Apply touch position to heads forward vector
            Vector3 direction = new Vector3(position.x, 0, position.y);
            Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

            // Rotate input direction by horizontal head rotation
            direction = Quaternion.Euler(headRotation) * direction;

            // Apply speed and move
            Vector3 movement = direction * playerSpeed;
            characterController.Move(movement * Time.deltaTime);
        }
        private void ApplyGravity() {
            Vector3 gravity = new Vector3(0, Physics.gravity.y * gravityMultiplier, 0);
            gravity.y *= Time.deltaTime;
            characterController.Move(gravity * Time.deltaTime);
        }

    }
}
