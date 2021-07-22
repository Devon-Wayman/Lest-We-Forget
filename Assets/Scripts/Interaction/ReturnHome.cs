// Author: Devon Wayman - June 2021
using LWF.Interaction.LevelManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WWIIVR.Interaction {
    public class ReturnHome : MonoBehaviour {
        //public GameObject baseControllerGameobject;
        public InputActionReference homeClickedActionReference;
        //public UnityEvent onHomePressed;

        private void Start() {
            homeClickedActionReference.action.performed += GoBackHome;
        }

        //private void GoBackHome(InputAction.CallbackContext obj) => onHomePressed.Invoke();
        private void GoBackHome(InputAction.CallbackContext obj) => LevelChanger.Current.FadeToLevel("MainMenu");
    }
}
