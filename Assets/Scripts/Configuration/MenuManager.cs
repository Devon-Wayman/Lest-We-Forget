// Author Devon Wayman
// Date Sept 21 2020
using System.Collections;
using UnityEngine;

/// <summary>
/// Used to play tutorial audio as well as perform other basic tasks for the Main Menu scene only
/// </summary>
public class MenuManager : MonoBehaviour {

    public bool TutorialPlaying { get; private set; } = false;

    [SerializeField] private AudioSource radioAudio = null;
    [SerializeField] private AudioSource tutorialAudio = null;

    private float radioOriginalVolume = 0f;
    private float radioTutorialVolume = 0.3f;

    private void Awake() {
        radioOriginalVolume = radioAudio.volume;
        tutorialAudio = GetComponent<AudioSource>();
    }

    #region Tutorial Setup
    public void PlayTutorial() {
        TutorialPlaying = true;
        Debug.Log("Beginning tutorial");
        StartCoroutine(FadeOutRadio());
    }

    private IEnumerator FadeOutRadio() {
        while (radioAudio.volume >= radioTutorialVolume) {
            radioAudio.volume -= 0.5f * Time.deltaTime;
            yield return null;
        }
        radioAudio.volume = radioTutorialVolume;
        PlayTutorialAudioClip();
    }

    private void PlayTutorialAudioClip() {
        tutorialAudio.Play();
        StartCoroutine(TutorialDelay());
    }

    private IEnumerator TutorialDelay() {
        // Wait for tutorial audio to finish
        yield return new WaitForSeconds(tutorialAudio.clip.length);

        while (radioAudio.volume <= radioOriginalVolume) {
            radioAudio.volume += 0.5f * Time.deltaTime;
            yield return null;
        }
        radioAudio.volume = radioOriginalVolume;
        TutorialPlaying = false;
    }
    #endregion
}
