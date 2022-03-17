// Author: Devon Wayman - January 2021
using LWF.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace LWF.Interaction {
    public class TheaterControl : MonoBehaviour {

        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private AudioSource videoAudio;
        private readonly WaitForSeconds waitTime = new(2);

        private void Awake() {
            videoPlayer.clip = Resources.Load<VideoClip>($"TheaterClips/{GameManager.RequestedFilm}");
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
