// Author: Devon Wayman - December 2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LWF.Interaction {
    public class VictimsListSlideshowController : MonoBehaviour {

        [SerializeField] CanvasGroup photoAlpha; // Reference to text content of victims rect
        [SerializeField] Image victimImage; // Display's selected prisoner sprite
        [SerializeField] TMP_Text victimName;

        List<Sprite> victimsSprites = new List<Sprite>();

        static VictimsListSlideshowController Instance;
        public static VictimsListSlideshowController Current {
            get {
                if (Instance == null) Instance = FindObjectOfType<VictimsListSlideshowController>();
                return Instance;
            }
        }

        WaitForSeconds nextImageDelay = new WaitForSeconds(1);
        WaitForSeconds imageDisplayDuration = new WaitForSeconds(10);

        int spriteIndex = 0;

        void Awake() {
            photoAlpha.alpha = 0;
            LoadAllSprites();
        }

        void LoadAllSprites() {
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites/VictimsSS")) {
                victimsSprites.Add(sprite);
            }
        }

        public void StartSlideshow() {
            StartCoroutine(SlideshowControl());
        }

        IEnumerator SlideshowControl() {
            while (victimsSprites.Count != 0) {
                victimImage.sprite = GrabRandomImage();
                yield return nextImageDelay;
                // LeanTween.alphaCanvas(photoAlpha, 0.8f, 1);
                photoAlpha.FadeCanvasGroup(0.8f, 1);
                yield return imageDisplayDuration;
                // LeanTween.alphaCanvas(photoAlpha, 0, 1);
                photoAlpha.FadeCanvasGroup(0, 1);
                yield return nextImageDelay;
                victimsSprites.RemoveAt(spriteIndex);
            }

            VictimsListTextController.Current.BeginListScroll();
        }


        // TODO: See if possible to get the sprite via extension within the slideshowcontrol enuemerator then just call to format the name string

        Sprite GrabRandomImage() {
            var tempSprite = victimsSprites.RandomListSelection();

            if (tempSprite.name.ToLower().Contains("unknown")) {
                victimName.text = "";
            } else {
                victimName.text = SetNameForPhotograph(tempSprite.name);
            }

            spriteIndex = victimsSprites.IndexOf(tempSprite);
            return tempSprite;
        }

        string SetNameForPhotograph(string tempName) {
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
                return $"{temp[0]} {temp[1]}. Age and survival status: Unknown";
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
