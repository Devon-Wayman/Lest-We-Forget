// Author: Devon Wayman - June 2021
using LWF.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LWF.Interaction {
    public class ReturnHome : MonoBehaviour {

        public InputActionReference homeClickedActionReference;

        void Start() {
            if (GameManager.currentSceneIndex == (int)SceneEnums.INTRO || GameManager.currentSceneIndex == (int)SceneEnums.MENU) return;

            homeClickedActionReference.action.performed += GoBackHome;
        }

        private void GoBackHome(InputAction.CallbackContext obj) {
            Debug.Log("Application home button pressed");
            GUIManager.Instance.FadeLevel(1, () => SceneManager.LoadScene((int)SceneEnums.MENU));
            AudioListener.volume.ChangeValueOverTime(AudioListener.volume, 0);
        }
    }
}
