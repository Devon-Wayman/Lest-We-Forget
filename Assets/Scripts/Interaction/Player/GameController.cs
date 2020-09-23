﻿// Author: Devon Wayman
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

        public static bool changingScenes = false;

        private void Awake() {

            // Ensure changing scenes is false on awake when loading to a new scene
            changingScenes = false; 

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
            if (changingScenes) return;

            // Check if menu button has been pressed
            if (inputHandler.GetApplicationMenuDown()) {
                if (Time.timeScale != 1)
                    Time.timeScale = 1;
                levelChanger.FadeToLevel("MainMenu");
                changingScenes = true;
            }

            #region Main Menu only
            // Nothing passed the next line is required unless at main menu
            if (currentSceneName != "MainMenu") return;

            // Check if both trigger buttons have been pressed at main menu to play the tutorial
            if (inputHandler.TutorialRequested()) {
                if (menuManager.TutorialPlaying) {
                    Debug.Log("Tutorial currently playing!");
                } else {
                    menuManager.PlayTutorial();
                }
            }
            #endregion
        }
    }
}