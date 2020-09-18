// Author: Devon Wayman
using UnityEngine;
using UnityEngine.SceneManagement;

// Main player input manager
namespace WWIIVR.Interaction.Player {

    [RequireComponent(typeof(InputHandler))]
    public class GameController : MonoBehaviour {

        private LevelChanger levelChanger = null; // Reference to level changer object
        private string currentSceneName; // Get current scene name

        //[SerializeField] private List<InputDevice> leftHandDevices = new List<InputDevice> (); // Left hand VR items
        //[SerializeField] private List<InputDevice> rightHandDevices = new List<InputDevice> (); // Right hand VR items
        [SerializeField] private InputHandler inputHandler;

        private bool menuWasPressed = false;

        private void Awake() {
            levelChanger = FindObjectOfType<LevelChanger>(); // Set reference to level changer
            levelChanger.GetComponent<Animator>().speed = 1f; // Make sure on start that the level changer animation speed is normal
            currentSceneName = SceneManager.GetActiveScene().name;

            if (inputHandler == null)
                inputHandler = GetComponent<InputHandler>();
        }

        //private void Start () {
        //    InputDevices.GetDevicesAtXRNode (XRNode.LeftHand, leftHandDevices);
        //    InputDevices.GetDevicesAtXRNode (XRNode.RightHand, rightHandDevices);
        //}

        private void Update() {
            if (currentSceneName == "MainMenu") return;
            if (menuWasPressed) return;
            // if (leftHandDevices.Count <= 0 && leftHandDevices.Count <= 0) return;

            //if (leftHandDevices[0].TryGetFeatureValue (CommonUsages.menuButton, out menuPressed) && menuPressed && !menuPressedLastFrame) {
            if (inputHandler.GetApplicaitonMenuDown()) {
                if (Time.timeScale != 1)
                    Time.timeScale = 1;
                Debug.Log("Going home!");
                levelChanger.FadeToLevel("MainMenu");
                menuWasPressed = true;
            }
        }
    }
}