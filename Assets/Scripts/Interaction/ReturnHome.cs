// Author: Devon Wayman - June 2021
using LWF.Interaction.LevelManagement;
using UnityEngine;

namespace WWIIVR.Interaction {
    public class ReturnHome : MonoBehaviour {

        private bool homeWasPressed = false; // prevents home from being pressed multiple times to call for a main menu call

        void Update() {
            if (homeWasPressed) return;

            // Return home when Start button (menu button on left controller) is pressed
            if (OVRInput.GetDown(OVRInput.RawButton.Start)) {
                LevelChanger.Current.FadeToLevel("MainMenu");
                homeWasPressed = true;
            }
        }
    }
}
