// Author: Devon Wayman
// Sept 17, 2020
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace WWIIVR.Interaction.Player {
    public class InputHandler : MonoBehaviour {

        [SerializeField] private XRController leftController = null;
        [SerializeField] private XRController rightController = null;
        
        public bool MenuPressed { get; private set; } = false;
        public bool SceneLoadCalled { get; private set; } = false;

        private bool leftTriggerPressed;
        private bool rightTriggerPressed;
        private bool leftGripSqueezed;
        private bool rightGripSqueezed;

        public bool GetApplicaitonMenuDown() {
            leftController.inputDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool pressed);

            if (pressed)
                MenuPressed = true;
            else
                MenuPressed = false;

            return pressed;
        }  
        public bool TutorialRequested() {
            leftController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out leftTriggerPressed);
            rightController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out rightTriggerPressed);

            if (leftTriggerPressed && rightTriggerPressed) {
                return true;
            } else {
                return false;
            }
        }

        // Check if grip button is squeezed on either controller at main meun
        public void CheckGripSqueezed() {
            if (SceneLoadCalled) return;

            leftController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out leftGripSqueezed);
            rightController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out rightGripSqueezed);

            if (!leftGripSqueezed || !rightGripSqueezed) return;

            RaycastHit hit;

            // Check left hand for holding object to activate scene change
            if(Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hit, 0.5f)) {
                hit.transform.gameObject.TryGetComponent(out LevelObject levelObject);
                if (levelObject != null) {
                    SceneLoadCalled = true;
                    levelObject.LoadLevel();
                }
            }

            // Check right hand for holding object to activate scene change
            if (Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hit, 0.5f)) {
                hit.transform.gameObject.TryGetComponent(out LevelObject levelObject);
                if (levelObject != null) {
                    SceneLoadCalled = true;
                    levelObject.LoadLevel();
                }
            }
        }
    }
}