// Author: Devon Wayman
using UnityEngine;
using UnityEngine.SceneManagement;
using WWIIVR.Interaction.LevelManagement;

namespace WWIIVR.Interaction.Player {
    [RequireComponent(typeof(InputHandler))]
    public class GameController : MonoBehaviour {

        private LevelChanger levelChanger = null; // Reference to level changer object
        private MenuManager menuManager = null; // Used at Main Menu scene
        private string currentSceneName; // Get current scene name
        
        [SerializeField] private InputHandler inputHandler;

        private bool menuWasPressed = false;

        private void Awake() {
            levelChanger = FindObjectOfType<LevelChanger>(); // Set reference to level changer
            levelChanger.GetComponent<Animator>().speed = 1f; // Make sure on start that the level changer animation speed is normal
            currentSceneName = SceneManager.GetActiveScene().name;

            if(currentSceneName == "MainMenu") {
                if (menuManager == null) {
                    menuManager = FindObjectOfType<MenuManager>();
                }
            }

            if (inputHandler == null)
                inputHandler = GetComponent<InputHandler>();
        }

        private void Update() {
            // If menu button is ever pressed we want to either reset the scene or go home
            if (menuWasPressed) return;

            // Check if menu button has been pressed
            if (inputHandler.GetApplicaitonMenuDown()) {
                if (Time.timeScale != 1)
                    Time.timeScale = 1;
                Debug.Log("Going home!");
                levelChanger.FadeToLevel("MainMenu");
                menuWasPressed = true;
            }

            // Nothing passed the next line is required unless at main menu
            if (currentSceneName != "MainMenu") return;

            // Check if both trigger buttons have been pressed at main menu to play the tutorial
            if (inputHandler.TutorialRequested()) {
                if (menuManager.TutorialPlaying) {
                    Debug.Log("Tutorial currently playing!");
                    return;
                }
                menuManager.PlayTutorial();
            }

            // If grip is squeezed, check if held object has a LoadScene item and run its function
            inputHandler.CheckGripSqueezed();
        }
    }
}