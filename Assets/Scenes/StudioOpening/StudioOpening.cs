// Author: Devon Wayman
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using WWIIVR.Interaction;

/// <summary>
/// Used to load main menu after studio intro delay
/// </summary>
public class StudioOpening : MonoBehaviour {

    public float menuDelay = 6f;
    public float moveDistance = 5f; // Distance to move canvas forward for opener
    LevelChanger levelChanger = null; // Reference to LevelChanger

    void Awake() {
        levelChanger = FindObjectOfType<LevelChanger>(); // Find and store reference to level changer object
    }

    void Start() {
        StartCoroutine(FadeToMain()); // Start coroutine to fade to main menu

        float requestedForwardPosition = this.transform.position.z - moveDistance;

        StartCoroutine(MoveTitle(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, requestedForwardPosition ), 9f));
    }

    // Smoothly move the opening title text forward towards the player
    private IEnumerator MoveTitle(Vector3 startPos, Vector3 desiredLocation, float moveTime){
         float t = 0f; // Start time value
         while (t < 1.0f) {
             t += Time.deltaTime / moveTime; // Sweeps from 0 to 1 in time seconds
             transform.position = Vector3.Lerp(startPos, desiredLocation, t); // Set position proportional to t
             yield return null;         // Leave the routine and return here in the next frame
         }
    }

    // Wait given time before fading out to transition to the main menu
    private IEnumerator FadeToMain() {
        yield return new WaitForSeconds(menuDelay);
        if (levelChanger != null)
            levelChanger.FadeToLevel("MainMenu");
        else
            SceneManager.LoadScene("MainMenu"); // If level changer can not be found, abruptly switch to main menu scene
    }
}