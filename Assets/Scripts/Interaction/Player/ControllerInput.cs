// Author: Devon Wayman - December 2020
using LWF.Interaction.LevelManagement;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LWF.Interaction.Player {
    public class ControllerInput : MonoBehaviour {

        private InputAction homePressed;
        private InputAction userPresence;

        [SerializeField] private InputActionAsset actionsSet;


        private void Awake() {
            var leftHandMap = actionsSet.FindActionMap("XRI LeftHand");
            var headMap = actionsSet.FindActionMap("XRI HMD");

            homePressed = leftHandMap.FindAction("Home");
            userPresence = headMap.FindAction("UserPresence");

            userPresence.performed += OnUserPresenceChanged;
            homePressed.performed += OnHomePressed;
        }

        private void OnUserPresenceChanged(InputAction.CallbackContext obj) {
            Debug.Log($"User presence set to {obj.phase}");
        }

        

        private void OnHomePressed(InputAction.CallbackContext obj) {
            if (SceneManager.GetActiveScene().name != "MainMenu") return;
            Debug.Log("Going home");
            LevelChanger.Current.FadeToLevel("MainMenu");
        }


        private void OnEnable() {
            homePressed.Enable();
            userPresence.Enable();
        }
        private void OnDisable() {
            homePressed.Disable();
            userPresence.Disable();
        }
    }
}