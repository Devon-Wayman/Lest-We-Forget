// Author: Devon Wayman - January 2021
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace LWF.Interaction {
    public class TheaterControl : MonoBehaviour {

        [SerializeField] private AudioSource projectorAudio = null;
        [SerializeField] private AudioSource videoAudio = null;
        [SerializeField] private VideoPlayer videoPlayer = null; // Video player 
        private WaitForSeconds waitTime = new WaitForSeconds(1);

        private string requestedMovie;

        private void Awake() {
            requestedMovie = PlayerPrefs.GetString("Movie");

            if (requestedMovie == String.Empty || requestedMovie == "") {
                requestedMovie = "survivor";
            }

            // if survior video with audio, get audio source and set it
            if (requestedMovie == "survivor") {
                videoPlayer.SetTargetAudioSource(0, videoAudio);
            }

            videoPlayer.clip = Resources.Load<VideoClip>($"TheaterClips/{requestedMovie}") as VideoClip;
        }

        private void Start() {
            StartCoroutine(PrepareVideo());
        }

        private IEnumerator PrepareVideo() {
            videoPlayer.Prepare();

            while (!videoPlayer.isPrepared) {
                yield return waitTime;
                break;
            }
            videoPlayer.Play();
            projectorAudio.Play();
        }
    }
}
