using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace LWF.Audio {
    public class JukeboxController : MonoBehaviour {

        [Header("Transforms")]
        public Transform vinylRestTransform;
        public Transform needleTransform;
        public GameObject vinylDiscCopy;
        public Transform restHead;

        [Header("Audio Items")]
        [SerializeField] private List<AudioClip> songs = new List<AudioClip>();
        [SerializeField] private List<Transform> allDiscs = new List<Transform>();
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioMixer sceneAudioMixer;

        [Header("Debugging")]
        public Transform currentSongVinyl;
        public bool discParented = false;
        public bool musicPlaying = false;
        public bool isLowering = false;
        private float songLengthSeconds;
        private float distanceToRecord;
        private int songIndex;


        private void Awake() {
            var songsTemp = Resources.LoadAll<AudioClip>("Audio/RadioSongs");

            for (int i = 0; i < songsTemp.Length; i++) {
                songs.Add(songsTemp[i]);
            }

            sceneAudioMixer = Resources.Load<AudioMixer>("Audio/RadioSongs/RadioMixer");
            audioSource.outputAudioMixerGroup = sceneAudioMixer.FindMatchingGroups("Master")[0]; ;
            allDiscs.Add(vinylDiscCopy.transform);

            for (int i = 1; i < songs.Count; i++) {
                if (i >= 13) break;

                GameObject copy = Instantiate(vinylDiscCopy);
                copy.transform.parent = this.gameObject.transform;
                copy.transform.position = new Vector3(vinylDiscCopy.transform.position.x, vinylDiscCopy.transform.position.y - 0.01f * i, vinylDiscCopy.transform.position.z);
                copy.transform.localRotation = vinylDiscCopy.transform.localRotation;
                copy.name = $"Disc{i}";
                allDiscs.Add(copy.transform);
            }
        }
        private void Start() {
            PickNewSong();
        }

        private void SetupDisc(System.Action callback) {
            isLowering = false;

            LeanTween.rotateLocal(allDiscs[songIndex].gameObject, new Vector3(0, -86, 0), 5).setEaseInOutExpo().setOnComplete(() => {
                // move the vinyl rest
                LeanTween.moveLocalY(vinylRestTransform.gameObject, 1.375f, 9).setEaseOutExpo().setOnComplete(() => {
                    // begin moving needle and start playback of record
                    callback?.Invoke();
                });
            });
        }
        private void PlayRecord() {
            audioSource.Play();
            musicPlaying = true;

            // begin spinning the correct record
            LeanTween.rotateY(vinylRestTransform.gameObject, 360 * 10, songLengthSeconds);

            // start moving the play needle
            LeanTween.rotateLocal(needleTransform.gameObject, new Vector3(0, 10, 0), songLengthSeconds + 1).setOnComplete(() => {
                ReturnDisc(PickNewSong);
            });
        }
        private void ReturnDisc(System.Action callback) {
            isLowering = true;
            musicPlaying = false;
            LeanTween.rotateLocal(needleTransform.gameObject, new Vector3(0, 0, 0), 6).setEaseOutSine();

            // bring vinyl rest back down to place record in holder
            LeanTween.moveLocalY(vinylRestTransform.gameObject, 1.168f, 8).setEaseOutExpo().setOnComplete(() => {
                LeanTween.rotateLocal(allDiscs[songIndex].gameObject, new Vector3(0, 0, 0), 7).setEaseInOutExpo().setOnComplete(() => {
                    callback?.Invoke();
                });
            });
        }

        private void PickNewSong() {
            audioSource.clip = songs.RandomListSelection();
            songIndex = songs.IndexOf(audioSource.clip);
            songLengthSeconds = songs[songIndex].length;
            currentSongVinyl = allDiscs[songIndex].transform;
            SetupDisc(PlayRecord);
        }

        private void FixedUpdate() {
            if (musicPlaying && discParented) return;

            distanceToRecord = Vector3.Distance(restHead.position, currentSongVinyl.position);

            if (distanceToRecord < 0.31f && !discParented && !isLowering) {
                Debug.Log("PARENTING!!!!!");
                allDiscs[songIndex].GetChild(0).SetParent(restHead);
                discParented = true;
            }

            if (distanceToRecord < 0.31f && discParented && isLowering) {
                Debug.Log("SETTING DISC BACK IN HOLDER");
                discParented = false;
            }
        }
    }
}
