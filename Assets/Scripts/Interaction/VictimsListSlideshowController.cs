﻿// Author: Devon Wayman - December 2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LWF.Interaction {
    public class VictimsListSlideshowController : MonoBehaviour {

        public CanvasGroup victimsDisplayCG = null; // Reference to text content of victims rect
        private Image victimImageDisplay; // Display's selected prisoner sprite
        private List<Sprite> victimsSprites = new List<Sprite>(); // List of the prisoner images from spritesheet
        public Text imageVictimName = null;
        private WaitForSeconds textChangeDelay = new WaitForSeconds(1);
        private WaitForSeconds imageDisplayDuration = new WaitForSeconds(10);

        private static VictimsListSlideshowController Instance;

        public int spriteIndex = 0; // Index of currently activated sprite selected from victimSprites

        public static VictimsListSlideshowController Current {
            get {
                if (Instance == null) Instance = FindObjectOfType<VictimsListSlideshowController>();
                return Instance;
            }
        }

        private void Awake() {
            victimImageDisplay = victimsDisplayCG.GetComponent<Image>();
            victimsDisplayCG.alpha = 0;
            GetVictimSprites();
        }

        private void GetVictimSprites() {
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites/VictimsSS"))
                victimsSprites.Add(sprite);
        }


        public void StartSlideshow() {
            StartCoroutine(SlideshowControl());
        }

        private IEnumerator SlideshowControl() {
            victimImageDisplay.sprite = GrabRandomImage();

            yield return textChangeDelay;

            while (victimsDisplayCG.alpha < .8f) {
                victimsDisplayCG.alpha += 0.8f * Time.deltaTime;
                yield return null;
            }

            victimsDisplayCG.alpha = 0.8f;

            yield return imageDisplayDuration;

            while (victimsDisplayCG.alpha > 0) {
                victimsDisplayCG.alpha -= 0.8f * Time.deltaTime;
                yield return null;
            }

            victimsDisplayCG.alpha = 0;

            victimsSprites.RemoveAt(spriteIndex);

            // If there are no more sprites to show from the sprite sheet, break out of coroutine
            if (victimsSprites.Count == 0) {
                Debug.Log("All sprites for slideshow have been shown. Exiting coroutine...");
                yield break;
            }

            StartCoroutine(SlideshowControl());
        }

        private Sprite GrabRandomImage() {
            spriteIndex = Random.Range(0, victimsSprites.Count);

            string tempName = victimsSprites[spriteIndex].name;

            if (tempName.ToLower().Contains("unknown")) {
                imageVictimName.text = "";
            } else {
                string[] tempNameArray = tempName.Split('_');

                // If middle name included
                if (tempNameArray.Length == 4) {
                    // If survived is where age would be
                    if (tempNameArray[3] == "survived") {
                        imageVictimName.text = $"{tempNameArray[0]} {tempNameArray[1]} {tempNameArray[2]}.\nSurvived";
                    } else {
                        imageVictimName.text = $"{tempNameArray[0]} {tempNameArray[1]} {tempNameArray[2]}.\nDied age: {tempNameArray[3]}";
                    }
                }
                // If only first and last name given
                else if (tempNameArray.Length == 2) {
                    imageVictimName.text = $"{tempNameArray[0]} {tempNameArray[1]}. Age/whether or not they survived unknown";
                }
                // If name and age given
                else {
                    if (tempNameArray[2] == "survived") {
                        imageVictimName.text = $"{tempNameArray[0]} {tempNameArray[1]}.\nSurvived";
                    } else {
                        imageVictimName.text = $"{tempNameArray[0]} {tempNameArray[1]}.\nDied age: {tempNameArray[2]}";
                    }
                }
            }
            return victimsSprites[spriteIndex];
        }
    }
}
