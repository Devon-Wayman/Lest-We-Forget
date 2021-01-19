// Author: Devon Wayman - December 2020
using UnityEngine;

namespace LWF.Audio {
    public class MusicController : MonoBehaviour {


        public AudioClip[] songs; // Array of radio songs (populated from resources on awake)
        public AudioSource audioSource; // Attatched gameobjects audio source
        public enum Scene { MainMenu, VictimList };
        public Scene scene;

        public int songsPlayed = 0; // Amount of songs that have been played (keeps track for some scenes)

        private void Awake() {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }
        private void Start() {
            switch (scene) {
                case Scene.MainMenu:
                    songs = Resources.LoadAll<AudioClip>("Audio/RadioSongs");
                    InvokeRepeating("ChooseNextRadioSong", 3, 8);
                    break;
                case Scene.VictimList:
                    songs = Resources.LoadAll<AudioClip>("Audio/VictimsList");
                    PlayVictimListSong();
                    break;
            }
        }

        private void ChooseNextRadioSong() {
            if (!audioSource.isPlaying) {
                audioSource.clip = ChooseRadioSong();
                audioSource.Play();
            }
        }
        private AudioClip ChooseRadioSong() {
            return songs[Random.Range(0, songs.Length)];
        }
        private void PlayVictimListSong() {
            audioSource.clip = songs[songsPlayed];
            audioSource.Play();
        }
    }
}
