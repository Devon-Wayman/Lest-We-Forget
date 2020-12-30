// Author: Devon Wayman - December 2020
using LWF.Interaction.LevelManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LWF.Interaction.Player {
    public class ControllerInput : MonoBehaviour {

        [SerializeField] private InputActionAsset actionsSet;

        private InputAction homePressed;

        public bool canGoHome = true;


        private void Awake() {
            if (SceneManager.GetActiveScene().name != "MainMenu")
                canGoHome = true;
            else
                canGoHome = false;

            
            var leftHandMap = actionsSet.FindActionMap("XRI LeftHand");
            homePressed = leftHandMap.FindAction("Home");
            homePressed.performed += OnHomePressed;
        }

        private void OnHomePressed(InputAction.CallbackContext obj) {
            if (!canGoHome) {
                Debug.Log("Go home function disabled");
                return;
            }
            LevelChanger.Current.FadeToLevel("MainMenu");
        }

        private void OnEnable() {
            homePressed.Enable();
        }
        private void OnDisable() {
            homePressed.Disable();
        }
    }
}