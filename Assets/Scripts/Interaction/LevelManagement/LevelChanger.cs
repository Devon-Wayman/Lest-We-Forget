// Author: Devon Wayman
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Fades current scene out and fades in requested scene
namespace WWIIVR.Interaction.LevelManagement {
    public class LevelChanger : MonoBehaviour {

        public static LevelChanger Instance;

        private Animator levelChangeAnimator;

        private string levelToLoad;

        private List<AudioSource> allAudio = new List<AudioSource>(); 

        private float audioFadeTime = 2f; 

        private bool quittingGame = false;

        public static LevelChanger Current {
            get {
                if (!Instance) Instance = FindObjectOfType<LevelChanger>();

                return Instance;
            }
        }

        private void Awake() {
            if (Current != null && Current != this) 
                DestroyImmediate(this);
            else 
                name = "SceneLoadManager";
            

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
