// Author: Devon Wayman - December 2020
using LWF.Managers;
using UnityEngine;

public class IntroController : MonoBehaviour {

    [SerializeField] TMPro.TMP_Text introText;
    [SerializeField] CanvasGroup introCanvas;

    private string[] introductionTexts = new string[] { "Split Box Studios\npresents", "In association with\nJW Indie", "Lest We Forget" };
    private int textsDisplayed = 0;

    void Start() {
        introCanvas.alpha = 0;
        LeanTween.moveZ(gameObject, 7, 30);
        DisplayText();
    }

    private void DisplayText() {
        introText.text = introductionTexts[textsDisplayed];

        LeanTween.alphaCanvas(introCanvas, 1, 1).setOnComplete(() => {
            LeanTween.alphaCanvas(introCanvas, 0, 1).setDelay(4).setOnComplete(() => {

                if (textsDisplayed >= introductionTexts.Length - 1) {
                    Debug.Log("All texts displayed. Starting menu load timer");
                    GameManager.LoadToScene((int)SceneEnums.MENU);
                    return;
                }

                textsDisplayed++;
                DisplayText();
            });
        });
    }
}