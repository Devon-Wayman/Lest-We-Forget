// Author: Devon Wayman
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Sets up the salute lesson video and generates the lock code to prevent user from 
/// entering the application for a full day
/// </summary>
namespace WWIIVR.Interaction {
    public class SaluteLessonController : MonoBehaviour {

        public VideoClip videoToPlay; // Video clip to play
        private VideoPlayer videoPlayer = null; // Video player 

        private void Awake() {
            videoPlayer = GetComponent<VideoPlayer>(); // Get video player component

            videoPlayer.playOnAwake = false; // Ensure video does not begin playing
            videoPlayer.Pause(); // Pause video if autoplay is set to true

            videoPlayer.loopPointReached += LockAndExit; // Execute LockAndExit when video has reached end point
        }

        // Generate lock file to prevent user from entering until next day
        private void LockAndExit(VideoPlayer source) {
            Debug.Log ("Attempting to generate lock file.");

            Debug.Log($"Current date: {GetCurrentDate()}. Unlock date: {UnlockDate()}");
        }

        private string GetCurrentDate() {
            return DateTime.Today.ToShortDateString();
        }
        private string UnlockDate(){
            return DateTime.Today.AddDays(1).ToShortDateString();
        }

        private void Start() {  
            StartCoroutine(PlayVideo());
        }

        private IEnumerator PlayVideo(){
            yield return new WaitForSeconds(1);
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = videoToPlay;
            videoPlayer.Prepare();

            // Wait two seconds before checking if video is ready for playback if the status returns false
            WaitForSeconds waitTime = new WaitForSeconds(2);
            while (!videoPlayer.isPrepared) {
                yield return waitTime;
                break;
            }
            videoPlayer.Play(); // Play the video
        }
    }
}
