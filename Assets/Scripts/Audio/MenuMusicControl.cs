// Author: Devon Wayman - December 2020
using UnityEngine;

namespace LWF.Audio {

    /// <summary>
    /// Auto plays music files depending on the scene Scene enum is set to
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class MenuMusicControl : MonoBehaviour {

        public enum Scene { MainMenu, VictimList };

        public Scene scene;

        public AudioClip[] songs; // Array of radio songs (populated from resources on awake)
        private AudioSource audioSource; // Attatched gameobjects audio source

        public int songsPlayed = 0; // Amount of songs that have been played (keeps track for some scenes)

        private void Awake() {
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
                    InvokeRepeating("ChooseVictimListSong", 1, 5);
                    break;
            }
        }

        private void ChooseNextRadioSong() {
            if (!audioSource.isPlaying) {
                audioSource.clip = ChooseRadioSong();
                audioSource.Play();
            }
        }


        private void ChooseVictimListSong() {
            if (songsPlayed == songs.Length) {
                CancelInvoke();
                gameObject.SetActive(false);
            }
            if (!audioSource.isPlaying) {
                audioSource.clip = songs[songsPlayed];
                audioSource.Play();
                songsPlayed += 1;
            }
        }

        private AudioClip ChooseRadioSong() { 
            return songs[Random.Range(0, songs.Length)];
        }
    }
}
