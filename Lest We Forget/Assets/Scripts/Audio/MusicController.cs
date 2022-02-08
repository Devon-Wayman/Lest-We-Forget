// Author: Devon Wayman - December 2020
using UnityEngine;
using UnityEngine.Audio;

namespace LWF.Audio {
    public class MusicController : MonoBehaviour {

        [Header("Audio Setup")]
        [SerializeField] AudioClip[] songs;
        [SerializeField] AudioSource audioSource;

        public enum Scene { MainMenu, VictimList };
        public Scene scene;

        [Header("Scene Specific Settings")]
        [SerializeField] bool canLoop = false;
        [SerializeField] AudioMixer sceneAudioMixer;

        void Awake() {
            audioSource.loop = canLoop;

            switch (scene) {
                case Scene.MainMenu:
                    songs = Resources.LoadAll<AudioClip>("Audio/RadioSongs");
                    InvokeRepeating("ChooseNextRadioSong", 3, 8);
                    sceneAudioMixer = Resources.Load<AudioMixer>("Audio/RadioSongs/RadioMixer");
                    break;
                case Scene.VictimList:
                    songs = Resources.LoadAll<AudioClip>("Audio/VictimsList");
                    sceneAudioMixer = Resources.Load<AudioMixer>("Audio/VictimsList/VLMixer");
                    PlayVictimListSong();
                    break;
            }

            audioSource.outputAudioMixerGroup = sceneAudioMixer.FindMatchingGroups("Master")[0]; ;
        }

        void PlayVictimListSong() {
            audioSource.clip = songs[0];
            audioSource.Play();
        }

        void ChooseNextRadioSong() {
            if (!audioSource.isPlaying) {
                //audioSource.clip = ChooseRadioSong();
                audioSource.clip = songs.RandomListSelection();
                audioSource.Play();
            }
        }
        private AudioClip ChooseRadioSong() {
            return songs[Random.Range(0, songs.Length)];
        }
    }
}