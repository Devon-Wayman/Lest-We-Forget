// Author: Devon Wayman
using System.Collections;
using UnityEngine;
using WWIIVR.Interaction.LevelManagement;

/// <summary>
/// Used to load main menu after studio intro delay
/// </summary>
public class StudioOpening : MonoBehaviour {

    public float menuDelay = 6f;
    public float moveDistance = 5f;


    void Start() {
        StartCoroutine(FadeToMain());

        float requestedForwardPosition = transform.position.z - moveDistance;

        StartCoroutine(MoveTitle(transform.position, new Vector3(transform.position.x, transform.position.y, requestedForwardPosition ), 9f));
    }

    // Smoothly move the opening title text forward towards the player
    private IEnumerator MoveTitle(Vector3 startPos, Vector3 desiredLocation, float moveTime){
         float t = 0f; // Start time value
         while (t < 1.0f) {
             t += Time.deltaTime / moveTime;
             transform.position = Vector3.Lerp(startPos, desiredLocation, t); 
             yield return null;         
         }
    }

    // Wait given time before fading out to transition to the main menu
    private IEnumerator FadeToMain() {
        yield return new WaitForSeconds(menuDelay);

        LevelChanger.Instance.FadeToLevel("MainMenu");
    }
}