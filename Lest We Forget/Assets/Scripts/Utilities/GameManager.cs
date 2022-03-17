using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LWF.Managers {
    [DefaultExecutionOrder(-1)]
    public static class GameManager {

        public static int CurrentSceneIndex { get; private set; }
        public static bool ScenePrepped { get; private set; }


        public static string RequestedFilm { get; set; }
        public static bool FirstLaunch { get; private set; } = true;

        [RuntimeInitializeOnLoadMethod]
        public static void Init() {
            Debug.Log("Game manager initializing");
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene((int)SceneEnums.MENU);
        }

        /// <summary>
        /// Called whenever the player collides with the reset plane in a scene
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public static void ResetScene() {
            GUIManager.Instance.ResetLevel(1, () => {
                ScenePrepped = true;
            });
        }

        private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
            // Do all AI initializations, camera setup, procedural generation, etc here
            CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (CurrentSceneIndex == (int)SceneEnums.MENU && !FirstLaunch) {
                FirstLaunch = false;
                Debug.Log("First launch has been set to false as we have loaded into the main menu scene");
            }

            GUIManager.Instance.FadeLevel(0, () => {
                ScenePrepped = true;
            });
        }

        public static void LoadToScene(int levelIndex) {
            ScenePrepped = false;

            if (GUIManager.Instance == null) {
                Debug.LogWarning("<color=yellow>GuiManager not found</color>. Loading directly to scene without transition");
                SceneManager.LoadScene(levelIndex);
                return;
            }

            GUIManager.Instance.FadeLevel(1, () => SceneManager.LoadScene(levelIndex));
        }

        public static void QuitGame() {
            ScenePrepped = false;
            SceneManager.sceneLoaded -= OnSceneLoaded;

            GUIManager.Instance.FadeLevel(1, () => {

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
        }
    }
}
