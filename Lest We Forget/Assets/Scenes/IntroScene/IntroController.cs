// Author: Devon Wayman - December 2020
using System.Collections;
using UnityEngine;
using LWF.Interaction.LevelManagement;
using UnityEngine.UI;

public class IntroController : MonoBehaviour {

    [SerializeField] private Text introText;
    private WaitForSeconds displayDuration = new WaitForSeconds(4);

    private float moveSpeed = 0.6f;
    private int introTextIndex = 0;
    private string[] introductionTexts = new string[] { "Split Box Studios\npresents", "In association with\nJW Indie" };

    void Start() {
        StartCoroutine(FadeText(1f));
    }

    private void Update() {
        transform.position -= transform.forward * Time.deltaTime * moveSpeed;
    }

    private IEnumerator FadeText(float fadeSpeed) {

        while (introTextIndex < introductionTexts.Length) {
            introText.text = introductionTexts[introTextIndex];
            introText.color = new Color(introText.color.r, introText.color.g, introText.color.b, 0);

            while (introText.color.a < 0.8f) {
                introText.color = new Color(introText.color.r, introText.color.g, introText.color.b, introText.color.a + (Time.deltaTime / fadeSpeed));
                yield return null;
            }

            introText.color = new Color(introText.color.r, introText.color.g, introText.color.b, 0.8f);
            yield return displayDuration;

            while (introText.color.a > 0) {
                introText.color = new Color(introText.color.r, introText.color.g, introText.color.b, introText.color.a - (Time.deltaTime / fadeSpeed));
                yield return null;
            }

            introTextIndex++;
        }

        LevelChanger.Current.FadeToLevel("MainMenu");
    }
}