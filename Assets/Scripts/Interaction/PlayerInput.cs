// Author: Devon Wayman
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using WWIIVR.Interaction;

// Main player input manager
namespace WWIIVR.Player {
    public class PlayerInput : MonoBehaviour {

        private MenuControl menuControl = null;
        private LevelChanger levelChanger = null; // Reference to level changer object
        [SerializeField] private List<InputDevice> leftHandDevices = new List<InputDevice> (); // Left hand VR items
        [SerializeField] private List<InputDevice> rightHandDevices = new List<InputDevice> (); // Right hand VR items
        private string currentSceneName; // Get current scene name

        [Header ("Scene Specific Features")]
        private XRRayInteractor rayInteractor = null;
        private LineRenderer lineRenderer = null;
        private XRInteractorLineVisual lineVisual = null;

        [Header ("User input variables")]
        public bool menuPressedLastFrame = false;

        private void Awake () {
            levelChanger = FindObjectOfType<LevelChanger> (); // Set reference to level changer
            levelChanger.GetComponent<Animator> ().speed = 1f; // Make sure on start that the level changer animation speed is normal

            currentSceneName = SceneManager.GetActiveScene ().name;

            rayInteractor = FindObjectOfType<XRRayInteractor> ();
            lineRenderer = FindObjectOfType<LineRenderer> ();
            lineVisual = FindObjectOfType<XRInteractorLineVisual> ();

            if (currentSceneName == "MainMenu") {
                rayInteractor.enabled = lineRenderer.enabled = lineVisual.enabled = true; // Activate line interactions
                menuControl = FindObjectOfType<MenuControl> ();
            } else {
                rayInteractor.enabled = lineRenderer.enabled = lineVisual.enabled = false; // Deactivate point interactions
            }
        }

        private void Start () {
            InputDevices.GetDevicesAtXRNode (XRNode.LeftHand, leftHandDevices);
            InputDevices.GetDevicesAtXRNode (XRNode.RightHand, rightHandDevices);
        }

        private void Update () {
            if (leftHandDevices.Count <= 0 && leftHandDevices.Count <= 0) return;

            bool menuPressed;

            #region Controller Inputs
            if (leftHandDevices[0].TryGetFeatureValue (CommonUsages.menuButton, out menuPressed) && menuPressed && !menuPressedLastFrame) {
                menuPressedLastFrame = true;

                if (currentSceneName != "MainMenu" && menuPressed) {
                    if (Time.timeScale != 1)
                        Time.timeScale = 1;
                    Debug.Log ("Going home!");
                    levelChanger.FadeToLevel ("MainMenu");

                } else if (currentSceneName == "MainMenu" && menuPressed && menuControl != null) {
                    menuControl.ResetMenu.Invoke ();
                }
            } else if (leftHandDevices[0].TryGetFeatureValue (CommonUsages.menuButton, out menuPressed) && !menuPressed && menuPressedLastFrame) {
                menuPressedLastFrame = false; // Reset menu pressed last frame back to false once menu button is no longer pressed
            }
            #endregion
        }
    }
}