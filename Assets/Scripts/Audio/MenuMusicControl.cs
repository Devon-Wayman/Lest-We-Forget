// Author: Devon Wayman
using UnityEngine;

namespace WWIIVR.Audio {
    // Require an audio source to be attatched to same gameobject
    [RequireComponent(typeof(AudioSource))]
    public class MenuMusicControl : MonoBehaviour {

        private AudioClip[] radioSongs; // Array of radio songs (populated from resources on awake)
        private AudioSource radioAudioSource; // Attatched gameobjects audio source

        private void Awake() {
            // Retrieve in all radio songs from resources folder
            radioSongs = Resources.LoadAll<AudioClip>("Audio/RadioSongs"); 
            
            // Set reference to this object's audio source
            radioAudioSource = GetComponent<AudioSource>();
        }

        private void Start(){   
            // Start first song 3 seconds after start, then after every 8 seconds, check
            // if music is still playing. If not, another track will be selected
            InvokeRepeating("ChooseNextSong", 3, 8);
        }

        private void ChooseNextSong() { 
            // If audio source is not playing, select and play the next track
            if (!radioAudioSource.isPlaying) {
                radioAudioSource.clip = ChooseRandomSong();
                radioAudioSource.Play();
            }
        }

        private AudioClip ChooseRandomSong(){
            return radioSongs[Random.Range(0, radioSongs.Length)];
        }
    }
}
