// Author: Devon Wayman
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LWF.Interaction.LevelManagement {
    public class LevelChanger : MonoBehaviour {

        public static LevelChanger Instance;

        [SerializeField] private CanvasGroup levelFadeGroup = null;

        private bool quittingGame = false;
        private float audioFadeTime = 2f;
        private string levelToLoad;

        public static LevelChanger Current {
            get {
                if (!Instance) Instance = FindObjectOfType<LevelChanger>();
                return Instance;
            }
        }


        private void Awake() {
            levelFadeGroup.alpha = 1; // ensure we can see the black screen on start
        }

        private void Start() {
            LeanTween.alphaCanvas(levelFadeGroup, 0, 4);

            if (AudioListener.volume != 1)
                AudioListener.volume = 1;
        }

        public void FadeToLevel(string requestedLevel) {
            levelToLoad = requestedLevel; // Set levelToLoad to index passed in
            StartCoroutine(LowerAudio());   // Fade out audio
            LeanTween.alphaCanvas(levelFadeGroup, 1, 4).setOnComplete(() => { OnFadeComplete(); });
        }
        public void QuitGame() {
            quittingGame = true;
            StartCoroutine(LowerAudio());
            LeanTween.alphaCanvas(levelFadeGroup, 1, 4).setOnComplete(() => { OnFadeComplete(); });
        }

        private IEnumerator LowerAudio() {
            float t = audioFadeTime;
            while (t > 0) {
                t -= Time.deltaTime; // Decrease value of "t" over time; set by value of audioFadeTime
                AudioListener.volume = t / audioFadeTime;
                yield return null;
            }
            yield break;
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
