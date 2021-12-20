// Author: Devon Wayman - December 2020
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour {

    [SerializeField] TMPro.TMP_Text introText;
    [SerializeField] CanvasGroup introCanvas;

    WaitForSeconds displayDuration = new WaitForSeconds(4);
    WaitForSeconds breakDuration = new WaitForSeconds(1);

    string[] introductionTexts = new string[] { "Split Box Studios\npresents", "In association with\nJW Indie" };

    void Start() {
        introCanvas.alpha = 0;
        introText.text = introductionTexts[0];
        StartCoroutine(FadeText(1f));
        LeanTween.moveZ(gameObject, 10, 20);
    }

    IEnumerator FadeText(float fadeSpeed) {
        yield return breakDuration;
        LeanTween.alphaCanvas(introCanvas, 0.9f, 0.5f);
        yield return displayDuration;
        LeanTween.alphaCanvas(introCanvas, 0, 0.5f);
        yield return breakDuration;
        introText.text = introductionTexts[1];
        LeanTween.alphaCanvas(introCanvas, 0.9f, 0.5f);
        yield return displayDuration;
        LeanTween.alphaCanvas(introCanvas, 0, 0.5f);
        yield return breakDuration;
        SceneManager.LoadScene(1); // Load main menu scene (whichever version it may be at index 1)
    }
}