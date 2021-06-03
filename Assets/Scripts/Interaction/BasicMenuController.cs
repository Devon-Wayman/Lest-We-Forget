// Author: Devon Wayman - March 2021
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace WWIIVR {
    public class BasicMenuController : MonoBehaviour {

        [SerializeField] private CanvasGroup gameTitleGroup = null;
        [SerializeField] private CanvasGroup mainMenuGroup = null;

        [Header("Rolling Credits Items")]
        [SerializeField] private CanvasGroup creditsGroup = null;
        [SerializeField] private RectTransform creditsRectTransform = null;

        [Header("Donate to WWP items")]
        [SerializeField] private Text donateButtonText = null;
        [SerializeField] private Button donateButton = null;

        private WaitForSeconds fadeInDelay = new WaitForSeconds(3);
        private bool rollingCredits = false;

        private void Awake() {
            mainMenuGroup.alpha = gameTitleGroup.alpha = creditsGroup.alpha = 0;
            mainMenuGroup.interactable = creditsGroup.interactable = gameTitleGroup.interactable = false;
        }

        private void Start() {
            StartCoroutine(MenuFadeIns());
        }

        private void Update() {
            if (!rollingCredits) return;

            creditsRectTransform.transform.Translate(Vector3.up * Time.deltaTime * 0.6f);

            if (creditsRectTransform.transform.localPosition.y > 10) EndCredits();
        }

        private IEnumerator MenuFadeIns() {
            yield return fadeInDelay;
            LeanTween.alphaCanvas(gameTitleGroup, 1, 2);
            yield return fadeInDelay;
            LeanTween.alphaCanvas(mainMenuGroup, 1, 2);
            mainMenuGroup.interactable = true;
        }

        public void DonateToWWP() {
            System.Diagnostics.Process.Start("https://support.woundedwarriorproject.org/Default.aspx?tsid=10043");
            donateButtonText.text = "Check computer";
            donateButton.interactable = false;
        }

        #region Project credits
        /// <summary>
        /// Called when the Credits button from Extras menu is selected.
        /// Can be cancled when user presses any button on controller
        /// </summary>
        public void RollCredits() {
            LeanTween.alphaCanvas(gameTitleGroup, 0, 1);
            LeanTween.alphaCanvas(mainMenuGroup, 0, 1);
            LeanTween.alphaCanvas(creditsGroup, 1, 1);
            mainMenuGroup.interactable = gameTitleGroup.interactable = creditsGroup.interactable = false;
            rollingCredits = true;
        }

        /// <summary>
        /// Called when the credits have been fully displayed
        /// </summary>
        public void EndCredits() {
            rollingCredits = false;
            LeanTween.alphaCanvas(gameTitleGroup, 1, 1);
            LeanTween.alphaCanvas(mainMenuGroup, 1, 1);
            LeanTween.alphaCanvas(creditsGroup, 0, 1);
            mainMenuGroup.interactable = true;
        }
        #endregion
    }
}
