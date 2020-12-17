// Author: Devon Wayman
// Sept 17, 2020
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace WWIIVR.Interaction.Player {
    public class InputHandler : MonoBehaviour {

        public static InputHandler Instance;

        [SerializeField] private XRController leftController = null;

        public bool MenuPressed { get; private set; } = false;


        private void Awake() {
            if (Instance == null)
                Instance = this;
            else if (Instance != null)
                Destroy(this);

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
    }
}