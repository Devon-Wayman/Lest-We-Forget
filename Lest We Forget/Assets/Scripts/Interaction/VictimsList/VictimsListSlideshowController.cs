// Author: Devon Wayman - December 2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LWF.Interaction {
    public class VictimsListSlideshowController : MonoBehaviour {

        [SerializeField] private CanvasGroup photoAlpha; // Reference to text content of victims rect
        [SerializeField] private Image victimImage; // Display's selected prisoner sprite
        [SerializeField] private Text victimName = null;
        private List<Sprite> victimsSprites = new List<Sprite>();
        private static VictimsListSlideshowController Instance;
        public static VictimsListSlideshowController Current {
            get {
                if (Instance == null) Instance = FindObjectOfType<VictimsListSlideshowController>();
                return Instance;
            }
        }
        private WaitForSeconds nextImageDelay = new WaitForSeconds(1);
        private WaitForSeconds imageDisplayDuration = new WaitForSeconds(10);

        private int spriteIndex = 0;

        private void Awake() {
            photoAlpha.alpha = 0;
            GetVictimSprites();
        }
        private void GetVictimSprites() {
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites/VictimsSS")) {
                victimsSprites.Add(sprite);
            }
        }

        public void StartSlideshow() {
            StartCoroutine(SlideshowControl());
        }

        private IEnumerator SlideshowControl() {
            while (victimsSprites.Count != 0) {
                victimImage.sprite = GrabRandomImage();
                yield return nextImageDelay;
                LeanTween.alphaCanvas(photoAlpha, 0.8f, 1);
                yield return imageDisplayDuration;
                LeanTween.alphaCanvas(photoAlpha, 0, 1);
                yield return nextImageDelay;
                victimsSprites.RemoveAt(spriteIndex);
            }
            VictimsListTextController.Current.ListToCenter();
        }

        private Sprite GrabRandomImage() {
            spriteIndex = Random.Range(0, victimsSprites.Count);
            string tempName = victimsSprites[spriteIndex].name;

            if (tempName.ToLower().Contains("unknown")) {
                victimName.text = "";
            } else {
                victimName.text = SetNameForPhotograph(tempName);
            }
            return victimsSprites[spriteIndex];
        }

        private string SetNameForPhotograph(string tempName) {
            string[] temp = tempName.Split('_');

            // If middle name included
            if (temp.Length == 4) {
                // If "survived" is where age would be
                if (temp[3] == "survived") {
                    return $"{temp[0]} {temp[1]} {temp[2]}\nSurvived";
                } else {
                    return $"{temp[0]} {temp[1]} {temp[2]}\nMurdered at age {temp[3]}";
                }
            }
            // If only first and last name given
            else if (temp.Length == 2) {
                return $"{temp[0]} {temp[1]}. Age and whether survived: Unknown";
            }
            // If name and age given
            else {
                if (temp[2] == "survived") {
                    return $"{temp[0]} {temp[1]}\nSurvived";
                } else {
                    return $"{temp[0]} {temp[1]}\nMurdered at age {temp[2]}";
                }
            }
        }
    }
}
