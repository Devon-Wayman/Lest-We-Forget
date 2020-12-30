// Author: Devon Wayman
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LWF.Interaction.LevelManagement {
    public class LevelChanger : MonoBehaviour {

        public Animator levelChangeAnimator;
        public static LevelChanger Instance;

        private bool quittingGame = false;
        private float audioFadeTime = 2f;
        private string levelToLoad;

        public static LevelChanger Current {
            get {
                if (!Instance) Instance = FindObjectOfType<LevelChanger>();
                return Instance;
            }
        }

        private void Start() {
            levelChangeAnimator.Play("FadeScene_In");
            AudioListener.volume = 1;
        }

        public void FadeToLevel(string requestedLevel) {
            Debug.Log($"Fading to: {requestedLevel}");
            levelToLoad = requestedLevel; // Set levelToLoad to index passed in
            StartCoroutine(LowerAudio());   // Fade out audio
            levelChangeAnimator.Play("FadeScene_Out"); // Trigger the fade out animation
        }
        public void QuitGame() {
            quittingGame = true;
            StartCoroutine(LowerAudio());
            levelChangeAnimator.Play("FadeScene_Out");
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
                Debug.LogWarning($"{levelToLoad} does not exist. Reloading current level");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads current scene if requested scene couldnt be found
            }
        }
    }
}
