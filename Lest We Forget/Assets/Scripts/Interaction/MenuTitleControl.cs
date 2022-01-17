using LWF.Managers;
using LWF.Settings;
using UnityEngine;

namespace LWF.Interaction {
    public class MenuTitleControl : MonoBehaviour {

        [SerializeField] private CanvasGroup canvas;

        private void Start() {
            Debug.Log("Checking if this is the first time menu has been visited since launch");

            if (PlayerManager.Instance.playerSettings.firstLaunch) {
                canvas.alpha = 0;
                canvas.FadeCanvasGroup(0.9f, 5, FadeMenuOut, 3f);
                return;
            }

            Debug.Log("Menu has already been shown. We will not do the fade animation");
            gameObject.SetActive(false);
        }

        private void FadeMenuOut() {
            canvas.FadeCanvasGroup(0, 5, DisableMenuCanvas, 4);
        }

        private void DisableMenuCanvas() {
            SaveLoadManager.Save(new PlayerSettings { firstLaunch = false }); ;
            gameObject.SetActive(false);
        }
    }
}
