// Author: Devon Wayman
// Sept 17, 2020
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace WWIIVR.Interaction.Player {
    public class InputHandler : MonoBehaviour {

        [SerializeField] private XRController leftController = null;
        [SerializeField] private XRController rightController = null;
        
        private bool menuPressed = false;

        private bool leftTriggerPressed;
        private bool rightTriggerPressed;

        public bool GetApplicaitonMenuDown() {
            leftController.inputDevice.TryGetFeatureValue(CommonUsages.menuButton, out menuPressed);
            return menuPressed;
        }

        public bool TutorialRequested() {
            leftController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out leftTriggerPressed);
            rightController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out rightTriggerPressed);

            if (leftTriggerPressed && rightTriggerPressed)
                return true;
            else
                return false;
        }
    }

}