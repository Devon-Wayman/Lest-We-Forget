using UnityEngine;
using UnityEngine.SceneManagement;

namespace LWF.Managers {
    [DefaultExecutionOrder(-1)]
    public static class GameManager {

        public static int currentSceneIndex { get; private set; }
        public static bool ScenePrepped { get; private set; }

        public static Camera playerCamera;
        private static GUIManager guiManager;

        [RuntimeInitializeOnLoadMethod]
        public static void Init() {
            Debug.Log("Game manager initializing");
            guiManager = GUIManager.Instance;

            if (guiManager == null) {
                Debug.LogWarning("Unable to grab GUI Manager instance");
                return;
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene((int)SceneEnums.MENU);
        }

        public static void CloseApplication() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
            // Do all AI initializations, camera setup, procedural generation, etc here
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;


            GUIManager.Instance.levelFadeCanvas.gameObject.SetActive(true);
            GUIManager.Instance.FadeLevel(0, () => {
                ScenePrepped = true;
                GUIManager.Instance.levelFadeCanvas.gameObject.SetActive(false);
            });
        }

        public static void LoadToScene(int levelIndex) {
            ScenePrepped = false;
            GUIManager.Instance.levelFadeCanvas.gameObject.SetActive(true);
            GUIManager.Instance.FadeLevel(1, () => SceneManager.LoadScene(levelIndex));
        }

        public static void QuitGame() {
            ScenePrepped = false;
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
