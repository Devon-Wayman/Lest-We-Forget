// Author: Devon Wayman - January 2021
using LWF.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace LWF.Interaction {
    public class TheaterControl : MonoBehaviour {

        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private AudioSource videoAudio;
        private WaitForSeconds waitTime = new WaitForSeconds(2);

        private void Awake() {
            string requestedMovie;

            videoPlayer.SetTargetAudioSource(0, videoAudio);

            if (PlayerManager.Instance != null) {
                requestedMovie = PlayerManager.Instance.playerSettings.requestedMovie;
            } else {
                Debug.LogWarning("Movie string from settings empty or PlayerManager is null. Playing random");
                requestedMovie = "assorted";
            }

            Debug.Log($"Requested movie: {requestedMovie}");
            videoPlayer.clip = Resources.Load<VideoClip>($"TheaterClips/{requestedMovie}");
        }

        private void Start() {
            StartCoroutine(PrepareVideo());
        }

        private IEnumerator PrepareVideo() {
            videoPlayer.Prepare();

            while (!videoPlayer.isPrepared) {
                Debug.Log("Movie not ready to play. We will wait a moment");
                yield return waitTime;

                if (videoPlayer.isPrepared) break;
            }

            Debug.Log("Starting video");
            videoPlayer.Play();
        }
    }
}
