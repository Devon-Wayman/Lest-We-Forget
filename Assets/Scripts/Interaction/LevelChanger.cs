// Author: Devon Wayman
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Fades current scene out and fades in requested scene
namespace WWIIVR.Interaction {
    public class LevelChanger : MonoBehaviour {

        // Animator for fade animation
        private Animator levelChangeAnimator;
        // Requested level to transition to
        private string levelToLoad;

        // Stores all audio sources in scene
        private List<AudioSource> allAudio = new List<AudioSource>(); 
        // Amount of time to fade audio our (default 2 second)
        private int audioFadeTime = 2; 
        // Determine if application is being exited by user
        private bool quittingGame = false; 

        private void Awake() {
            // Set reference to animator object
            levelChangeAnimator = GetComponent<Animator>();

            // Collect and add all audio sources in scene to the list
            foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
                allAudio.Add(audioSource);
        }

        // Load level passed in
        public void FadeToLevel(string requestedLevel) {
            Debug.Log($"Fading to: {requestedLevel}");
            levelToLoad = requestedLevel; // Set levelToLoad to index passed in
            StartCoroutine(LowerAudio());   // Fade out audio
            levelChangeAnimator.SetTrigger("FadeOut"); // Trigger the fade out animation
        }
        public void QuitGame() {
            quittingGame = true;
            StartCoroutine(LowerAudio());
            levelChangeAnimator.SetTrigger("FadeOut");
        }

        // Fade audio of current scene out
        private IEnumerator LowerAudio() {
            float t = audioFadeTime;
            while (t > 0) {
                t -= Time.deltaTime; // Decrease value of "t" over time; set by value of audioFadeTime

                for (int i = 0; i < allAudio.Count; i++)
                    allAudio[i].volume = t / audioFadeTime;

                yield return null;
            }
            yield break;
        }
        
        // Load scene after fade out is completed
        public void OnFadeComplete() {
            if (quittingGame) {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
                return;
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
