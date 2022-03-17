// Author: Devon Wayman - June 2021
using LWF.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LWF.Interaction {
    public class ReturnHome : MonoBehaviour {

        public InputActionReference homeClickedActionReference;

        private void OnEnable() {
            Debug.Log("Initializing ReturnHome system");
            if (GameManager.CurrentSceneIndex == 1 || GameManager.CurrentSceneIndex == 2) return;

            homeClickedActionReference.action.performed += GoBackHome;
        }

        private void OnDisable() {
            homeClickedActionReference.action.performed -= GoBackHome;
        }

        private void GoBackHome(InputAction.CallbackContext obj) {
            Debug.Log("Application home button pressed");
            GUIManager.Instance.FadeLevel(1, () => SceneManager.LoadScene((int)SceneEnums.MENU));
            AudioListener.volume.ChangeValueOverTime(AudioListener.volume, 0);
        }
    }
}
