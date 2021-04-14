// Author: Devon Wayman - March 2021
using UnityEngine;
using UnityEngine.UI;

namespace WWIIVR {
    public class BasicMenuController : MonoBehaviour {

        public Button[] subMenuButtons;
        public GameObject[] contentPanels;

        private void OnEnable() {
            // Add event listeners to switch panels via content buttons
            for (int i = 0; i < subMenuButtons.Length; i++) {
                subMenuButtons[i].onClick.AddListener(() => SwitchToContentPanel(i));
            }

            // Disable all panels except the first
            for (int i = 0; i < contentPanels.Length; i++) {
                if (i == 1) {
                    contentPanels[i].SetActive(true);
                } else {
                    contentPanels[i].SetActive(false);
                }
            }
        }
        void OnDisable() {
            for (int i = 0; i < subMenuButtons.Length; i++) {
                subMenuButtons[i].onClick.RemoveListener(() => SwitchToContentPanel(i));
            }
        }

        private void SwitchToContentPanel(int menuIndex) {
            for (int i = 0; i < contentPanels.Length; i++) {
                if (i == menuIndex) {
                    contentPanels[i].SetActive(true);
                } else {
                    contentPanels[i].SetActive(false);
                }
            }
        }
    }
}
