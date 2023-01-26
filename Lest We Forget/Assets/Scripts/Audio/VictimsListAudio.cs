// Author: Devon Wayman - December 2020
using UnityEngine;

namespace LWF.Audio {
    public class VictimsListAudio : MonoBehaviour {

        [Header("Audio Setup")]
        [SerializeField] AudioSource audioSource;
        [SerializeField] bool canLoop = true;

        private AudioClip[] songs;

        private void Start() {
            audioSource.loop = canLoop;
            songs = Resources.LoadAll<AudioClip>("Audio/VictimsList");
            audioSource.clip = songs[0];
            audioSource.Play();
        }
    }
}