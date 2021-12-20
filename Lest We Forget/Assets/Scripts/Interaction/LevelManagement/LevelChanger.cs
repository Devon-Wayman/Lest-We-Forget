// Author: Devon Wayman
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LWF.Interaction.LevelManagement {
    public class LevelChanger : DevSingleton<LevelChanger> {

        [SerializeField] CanvasGroup levelChangeFade = null;

        public Action onFadeCompleteAction;
        [SerializeField] GameObject locomotionManager;
        bool quittingGame = false;
        int levelToLoad;


        void Awake() {
            levelChangeFade.alpha = 1;
            onFadeCompleteAction = OnFadeComplete;
        }

        void Start() {
            levelChangeFade.FadeCanvasGroup(0, 4);
            if (AudioListener.volume != 1) AudioListener.volume = 1;

            if (SceneManager.GetActiveScene().buildIndex == (int)SceneEnums.MAINMENU) {
                locomotionManager.SetActive(true);
            } else {
                locomotionManager.SetActive(false);
            }
        }

        public void FadeToLevel(int levelIndex) {
            Debug.Log("Fading to new level");
            levelToLoad = levelIndex;

            // TODO: Make a coroutine in the Helpers class that does this!)
            AudioListener.volume.ChangeValueOverTime(AudioListener.volume, 0);

            levelChangeFade.SceneChangeFade(1, 4);
        }
        public void QuitGame() {
            quittingGame = true;
            AudioListener.volume.ChangeValueOverTime(AudioListener.volume, 0);
            levelChangeFade.SceneChangeFade(1, 4);
        }

        public void OnFadeComplete() {
            if (quittingGame) {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }

            try {
                SceneManager.LoadScene(levelToLoad);
            } catch {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads current scene if requested scene couldnt be found
            }
        }
    }
}
