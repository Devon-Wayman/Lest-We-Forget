// Author: Devon Wayman - June 2021
using LWF.Interaction.LevelManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LWF.Interaction {
    public class ReturnHome : MonoBehaviour {

        public InputActionReference homeClickedActionReference;

        void Start() {
            homeClickedActionReference.action.performed += GoBackHome;
        }

        void GoBackHome(InputAction.CallbackContext obj) {
            Debug.Log("Application home button pressed");
            LevelChanger.Instance.FadeToLevel((int)SceneEnums.MAINMENU);
            AudioListener.volume.ChangeValueOverTime(AudioListener.volume, 0);
        }
    }
}
