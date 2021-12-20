// Author: Devon Wayman - January 2021
using System.Collections;
using LWF.Settings;
using UnityEngine;
using UnityEngine.Video;

namespace LWF.Interaction {
    public class TheaterControl : MonoBehaviour {

        [SerializeField] VideoPlayer videoPlayer = null; // Video player 
        WaitForSeconds waitTime = new WaitForSeconds(1);

        void Awake() {
            string requestedMovie = PlayerSaveLoadManager.Instance.playerSettings.requestedMovie;
            videoPlayer.clip = Resources.Load<VideoClip>($"TheaterClips/{requestedMovie}") as VideoClip;
            StartCoroutine(PrepareVideo());
        }

        IEnumerator PrepareVideo() {
            videoPlayer.Prepare();

            while (!videoPlayer.isPrepared) {
                Debug.LogWarning("Video is not yet ready for playback.");
                yield return waitTime;

                if (videoPlayer.isPrepared) break;
            }

            Debug.Log("Video player ready. Starting now");
            videoPlayer.Play();
        }
    }
}
