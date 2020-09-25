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

        // Singletone for this class to reference in the LoadingScreen
        // scene if the theater is being loaded
        public TheaterControl current;

        private void Awake() {
            current = this;

            videoPlayer = GetComponent<VideoPlayer>(); // Get video player component
            videoPlayer.playOnAwake = false; 
            videoPlayer.Pause(); 

            projectorAudio.Stop(); 

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
                    videoToPlay = videos[2];
                    break;
            }
        }

        private void Start() {  
            StartCoroutine(PrepareVideo());
        }

        private IEnumerator PrepareVideo(){
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = videoToPlay;
            videoPlayer.Prepare();

            WaitForSeconds waitTime = new WaitForSeconds(2f);

            while (!videoPlayer.isPrepared) {
                Debug.Log("Video still needs to be prepared");
                yield return waitTime;
                break;
            }
            videoPlayer.Play();
            projectorAudio.Play();
        }
    }
}
