// Copyright Devon Wayman 2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Controls state of MainMenu scene canvases. DO NOT place game settings controls in here, Devon! Future you is watching you!
namespace WWIIVR.Interaction {
    public class MenuControl : MonoBehaviour {

        // Reference to LevelChanger
        private LevelChanger levelChanger = null;
        // CanvasGroup of main screen 
        public CanvasGroup mainMenuScreen;
        // Child objects/menus of the main screen
        private List<GameObject> childMenus = new List<GameObject>();
        // UnityEvent to run functions to reset menu view
        public UnityEvent ResetMenu;

        private void Awake() {
            // Set reference to LevelChanger
            levelChanger = FindObjectOfType<LevelChanger>();
            
            // Get child objects and add to childMenus list
            foreach (Transform obj in transform)
                childMenus.Add(obj.gameObject);
        }
        private void Start() {
            // Ensure timescale is default
            Time.timeScale = 1; 
            // Make sure we cant interact with the main menu yet
            mainMenuScreen.interactable = false;
            // Make screen invisible
            mainMenuScreen.alpha = 0;
            // Ensure all screens are disabled except main
            ResetMainMenu();
        }
        public void ResetMainMenu() {
            for (int i = 0; i < childMenus.Count; i++)
                childMenus[i].SetActive(true);
           
            for (int i = 2; i < childMenus.Count; i++)
                childMenus[i].SetActive(false);
        }

        #region Fade in main menu
        public void EnableMainMenu() {
            StartCoroutine(FadeInMenu(mainMenuScreen));
        }
        private IEnumerator FadeInMenu(CanvasGroup canvas) {
            while (canvas.alpha < 0.8f) {
                canvas.alpha += 0.5f * Time.deltaTime;
                yield return null;
            }
            canvas.alpha = 0.8f;
            mainMenuScreen.interactable = true;
        }
        #endregion

        #region Scene Loading
        public void LoadTheater(string clipName) {
            PlayerPrefs.SetString("Movie", clipName);
            levelChanger.FadeToLevel("Theater");
        }
        public void LoadPlaneFlight(string planeSelection) {
            PlayerPrefs.SetString("DesiredPlane", planeSelection);
            levelChanger.FadeToLevel("PlaneFlight");
        }
        public void LoadDDay() {
            levelChanger.FadeToLevel("DDay");
        }
        public void LoadVictimsList() {
            levelChanger.FadeToLevel("VictimsList");
        }
        #endregion
        
        public void QuitApplication() {
            levelChanger.QuitGame();
        }
    }
}
