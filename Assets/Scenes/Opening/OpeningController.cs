// Author: Devon Wayman - December 2020
using System.Collections;
using UnityEngine;
using LWF.Interaction.LevelManagement;
using UnityEngine.UI;

public class OpeningController : MonoBehaviour {

    public Font[] fonts;
    public Text openerText = null;
    private WaitForSeconds displayDuration = new WaitForSeconds(4);

    public float moveDistance = 5f;
    public int moveDuration = 13;

    void Start() {
        float requestedForwardPosition = transform.position.z - moveDistance;
        openerText.font = fonts[0];
        openerText.text = "Split Box Studios";

        StartCoroutine(FadeText(1f, openerText.GetComponent<Text>()));
        StartCoroutine(MoveTitle(transform.position, new Vector3(transform.position.x, transform.position.y, requestedForwardPosition), moveDuration));
    }

    // Move text towards player
    private IEnumerator MoveTitle(Vector3 startPos, Vector3 desiredLocation, float moveTime){
         float t = 0f; 
         while (t < 1.0f) {
             t += Time.deltaTime / moveTime;
             transform.position = Vector3.Lerp(startPos, desiredLocation, t); 
             yield return null;         
         }
    }

    // Change text over time
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
        openerText.font = fonts[1];
        openerText.text = "Lest We Forget";

        while (text.color.a < 0.8f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / fadeDuration));
            yield return null;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.8f);

        yield return displayDuration;

        LevelChanger.Current.FadeToLevel("MainMenu");
    }
}