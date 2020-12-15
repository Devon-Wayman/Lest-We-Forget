// Author: Devon Wayman
using UnityEngine;
using UnityEngine.SceneManagement;
using WWIIVR.Interaction.LevelManagement;

namespace WWIIVR.Interaction.Player {
    [RequireComponent(typeof(InputHandler))]
    public class GameController : MonoBehaviour {

        private MenuManager menuManager = null; // Used at Main Menu scene
        private string currentSceneName; // Get current scene name
        
        [SerializeField] private InputHandler inputHandler;

        public static bool changingScenes = false;

        private void Awake() {

            // Ensure changing scenes is false on awake when loading to a new scene
            changingScenes = false; 

            LevelChanger.Instance.GetComponent<Animator>().speed = 1f; // Make sure on start that the level changer animation speed is normal
            currentSceneName = SceneManager.GetActiveScene().name;

            if(currentSceneName == "MainMenu") {
                if (menuManager == null) {
                    menuManager = FindObjectOfType<MenuManager>();
                }
            } else {
                // Keep menu manager null if not in main menu scene
                menuManager = null;
            }

            if (inputHandler == null)
                inputHandler = GetComponent<InputHandler>();
        }

        private void Update() {
            // If menu button is ever pressed we want to either reset the scene or go home
            if (changingScenes) return;

            // Check if menu button has been pressed
            if (inputHandler.GetApplicationMenuDown()) {
                if (Time.timeScale != 1)
                    Time.timeScale = 1;
                LevelChanger.Instance.FadeToLevel("MainMenu");
                changingScenes = true;
            }

            #region Main Menu only
            // Nothing passed the next line is required unless at main menu
            if (currentSceneName != "MainMenu") return;
            #endregion
        }
    }
}