// Author: Devon Wayman - December 2020
using System.Collections;
using UnityEngine;
using LWF.Interaction.LevelManagement;
using UnityEngine.UI;
using UnityEngine.Localization;

public class OpeningController : MonoBehaviour {

    public LocalizedString gameOpenerTranslator = new LocalizedString();
    public Text openingText = null;
    private WaitForSeconds displayDuration = new WaitForSeconds(6);

    public float moveDistance = 5f;
    public int moveDuration = 13;
    private int textsDisplayed = 1;

    private void Awake() {
        gameOpenerTranslator.StringChanged += OnStringChange;
    }

    void Start() {
        gameOpenerTranslator.TableEntryReference = $"line{textsDisplayed}";

        float requestedForwardPosition = transform.position.z - moveDistance;
        StartCoroutine(FadeText(1f, openingText.GetComponent<Text>()));
        StartCoroutine(MoveTitle(transform.position, new Vector3(transform.position.x, transform.position.y, requestedForwardPosition), moveDuration));
    }

    private void OnStringChange(string value) {
        openingText.text = value;
    }

    private IEnumerator FadeText(float fadeDuration, Text text) {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

        while (text.color.a < 0.8f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / fadeDuration));
            yield return null;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.8f);

        yield return displayDuration;

        while (text.color.a > 0) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / fadeDuration));
            yield return null;
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

        if (textsDisplayed == 1) {
            textsDisplayed++;
            gameOpenerTranslator.TableEntryReference = $"line{textsDisplayed}";
            StartCoroutine(FadeText(1f, openingText.GetComponent<Text>()));
        } else {
            LevelChanger.Current.FadeToLevel("MainMenu");
        }
    }

    private IEnumerator MoveTitle(Vector3 startPos, Vector3 desiredLocation, float moveTime){
         float t = 0f; 
         while (t < 1.0f) {
             t += Time.deltaTime / moveTime;
             transform.position = Vector3.Lerp(startPos, desiredLocation, t); 
             yield return null;         
         }
    }
}