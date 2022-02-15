using System.Collections.Generic;
using UnityEngine;

namespace LWF.Audio {
    public class JukeboxController : MonoBehaviour {

        [Header("Transforms")]
        public Transform vinylRestTransform;
        public Transform needleTransform;
        public Transform restHead;
        public GameObject vinylDiscCopy;

        [Header("Audio Items")]
        [SerializeField] private AudioSource audioSource;

        [Header("Debugging")]
        public Transform currentSongVinyl;
        public List<Transform> vinylHolders = new List<Transform>();
        public bool discParented = false;

        private float selectedVinylStartHeight;
        private Vector3 selectedVinylPosition;
        private int songLengthSeconds;
        private int songIndex;
        private List<AudioClip> songs = new List<AudioClip>();

        private void Awake() {
            var songsTemp = Resources.LoadAll<AudioClip>("Audio/RadioSongs");

            foreach (AudioClip clip in songsTemp) {
                songs.Add(clip);
            }

            vinylHolders.Add(vinylDiscCopy.transform);

            for (int i = 1; i < songs.Count; i++) {
                if (i >= 13) break;

                GameObject copy = Instantiate(vinylDiscCopy);
                copy.transform.parent = this.gameObject.transform;
                copy.transform.position = new Vector3(vinylDiscCopy.transform.position.x, vinylDiscCopy.transform.position.y - 0.01f * i, vinylDiscCopy.transform.position.z);
                copy.transform.localRotation = vinylDiscCopy.transform.localRotation;
                copy.name = $"Disc{i}";
                vinylHolders.Add(copy.transform);
            }
        }

        private void Start() {
            PickNewSong();
        }

        private void PickNewSong() {
            songIndex = songs.IndexOf(songs.RandomListSelection());

            audioSource.clip = songs[songIndex];
            //songIndex = songs.IndexOf(audioSource.clip);

            songLengthSeconds = Mathf.RoundToInt(songs[songIndex].length + 3);
            currentSongVinyl = vinylHolders[songIndex].transform;
            SetupDisc();
        }

        private void SetupDisc() {
            LeanTween.rotateLocal(vinylHolders[songIndex].gameObject, new Vector3(0, -86, 0), 5).setEaseOutQuad().setOnComplete(() => {

                // Set the position at which this vinyl is at on y axis 
                selectedVinylStartHeight = vinylHolders[songIndex].transform.position.y;
                selectedVinylPosition = vinylHolders[songIndex].GetChild(0).transform.position;

                // move the vinyl rest
                LeanTween.moveLocalY(vinylRestTransform.gameObject, 1.375f, 9).setEaseOutExpo().setOnComplete(() => {
                    PlayRecord();
                });
            });
        }
        private void PlayRecord() {
            LeanTween.rotateY(vinylRestTransform.gameObject, 360 * 10, songLengthSeconds);

            audioSource.Play();

            LeanTween.rotateLocal(needleTransform.gameObject, new Vector3(0, 9.5f, 0), songLengthSeconds).setOnComplete(() => {
                ReturnDisc();
            });
        }

        private void ReturnDisc() {
            LeanTween.rotateLocal(needleTransform.gameObject, new Vector3(0, 0, 0), 6).setEaseOutSine();

            LeanTween.moveLocalY(vinylRestTransform.gameObject, 1.168f, 10).setOnComplete(() => {
                LeanTween.rotateLocal(vinylHolders[songIndex].gameObject, new Vector3(0, 0, 0), 7).setOnComplete(() => {
                    PickNewSong();
                });
            });
        }

        private void FixedUpdate() {
            if (restHead.position.y >= currentSongVinyl.position.y && !discParented) {
                Debug.Log("Parenting vinyl disc to holder");
                vinylHolders[songIndex].GetChild(0).SetParent(restHead);
                discParented = true;
            }

            if (restHead.position.y <= selectedVinylStartHeight && discParented) {
                Debug.Log("Vinyl placed back in holder");
                restHead.GetChild(0).transform.SetParent(vinylHolders[songIndex], true);
                vinylHolders[songIndex].GetChild(0).position = selectedVinylPosition;
                discParented = false;
            }
        }
    }
}
