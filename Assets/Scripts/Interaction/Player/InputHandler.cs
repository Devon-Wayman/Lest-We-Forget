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

        private bool leftTriggerPressed;
        private bool rightTriggerPressed;

        private void Awake() {
            // Ensure MenuPressed is false on awake of a new scene
            MenuPressed = false;
        }


        public bool GetApplicationMenuDown() {
            leftController.inputDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool pressed);

            if (pressed)
                MenuPressed = true;
            else
                MenuPressed = false;

            return pressed;
        }
        public bool TutorialRequested() {
            leftController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out leftTriggerPressed);
            rightController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out rightTriggerPressed);

            if (leftTriggerPressed && rightTriggerPressed) {
                return true;
            } else {
                return false;
            }
        }
    }
}