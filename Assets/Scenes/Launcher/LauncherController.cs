// Author: Devon Wayman - June 2021
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

namespace WWIIVR {
    public class LauncherController : MonoBehaviour {

        [SerializeField] private GameObject creditsOverlay = null;
        [SerializeField] private RectTransform creditTextContent = null;
        private bool creditsEnabled = false;

        private void Awake() {
            creditsOverlay.SetActive(false);
        }

        public void LaunchGame() {
            SceneManager.LoadScene("IntroScene"); // Load the intro scene immediatly if in editor
        }

        public void ToggleCredits() {
            creditsEnabled = !creditsEnabled;
            creditsOverlay.SetActive(creditsEnabled);

            if (creditsEnabled) {
                creditTextContent.anchoredPosition = new Vector2(0, 0);
            }
        }

        public void QuitApp() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
