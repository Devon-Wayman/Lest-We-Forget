// Author: Devon Wayman - January 2021
using LWF.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace LWF.Interaction {
    public class TheaterControl : MonoBehaviour {

        [SerializeField] VideoPlayer videoPlayer = null; // Video player 
        WaitForSeconds waitTime = new WaitForSeconds(2);

        private void Awake() {
            string requestedMovie = PlayerManager.Instance.playerSettings.requestedMovie;

            if (requestedMovie == string.Empty || requestedMovie == null) {
                Debug.LogWarning("Movie string from settings empty. Playing random");
                requestedMovie = "camps";
            }

            Debug.Log($"Requested movie: {requestedMovie}");
            videoPlayer.clip = Resources.Load<VideoClip>($"TheaterClips/{requestedMovie}");
            StartCoroutine(PrepareVideo());
        }

        private IEnumerator PrepareVideo() {
            videoPlayer.Prepare();

            while (!videoPlayer.isPrepared) {
                yield return waitTime;

                if (videoPlayer.isPrepared) break;
            }

            Debug.Log("Starting video");
            videoPlayer.Play();
        }
    }
}
