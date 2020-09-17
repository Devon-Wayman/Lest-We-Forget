using System;
// Author: Devon Wayman
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Set up and play requested video
/// </summary>
namespace WWIIVR.Interaction {
    public class TheaterControl : MonoBehaviour {
        
        public VideoClip[] videos; // Array of available video clips
        public string requestedMovie; // String value of RequestedMovie
        private VideoClip videoToPlay; // Video clip to play (selected depending on user input)
        private VideoPlayer videoPlayer = null; // Video player 

        public AudioSource projectorAudio = null;

        private void Awake() {
            videoPlayer = GetComponent<VideoPlayer>(); // Get video player component

            videoPlayer.playOnAwake = false; // Ensure video does not begin playing
            videoPlayer.Pause(); // Pause video if autoplay is set to true

            projectorAudio.Stop(); // Prevent projector audio from playing

            // Set up requested video to play
            requestedMovie = PlayerPrefs.GetString("Movie");

            switch (requestedMovie) {
                case "assorted":
                    videoToPlay = videos[0];
                    break;
                case "bombing":
                    videoToPlay = videos[1];
                    break;
                case "camps":
                    videoToPlay = videos[2]; // Select third video in videos array to play
                    break;
            }
        }

        private void Start() {  
            StartCoroutine(PlayVideo());
        }

        private IEnumerator PlayVideo(){
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = videoToPlay;
            videoPlayer.Prepare();

            // Wait two seconds before checking if video is ready for playback if the status returns false
            WaitForSeconds waitTime = new WaitForSeconds(1);

            while (!videoPlayer.isPrepared) {
                yield return waitTime;
                break;
            }
            videoPlayer.Play(); // Play the video
            projectorAudio.Play();
        }
    }
}
