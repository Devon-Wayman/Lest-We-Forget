// Author: Devon Wayman
// Sept 17, 2020
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace WWIIVR.Interaction.Player {
    public class InputHandler : MonoBehaviour {

        [SerializeField] private XRController controller;
        [SerializeField] private bool menuPressed = false;

        public bool GetApplicaitonMenuDown() {
            controller.inputDevice.TryGetFeatureValue(CommonUsages.menuButton, out menuPressed);
            return menuPressed;
        }
    }

}