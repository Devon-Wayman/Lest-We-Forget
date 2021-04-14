// Author: Devon Wayman - December 2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LWF.Interaction {
    public class VictimsListSlideshowController : MonoBehaviour {

        [SerializeField] private CanvasGroup victimsDisplayCG; // Reference to text content of victims rect
        [SerializeField] private Image victimImageDisplay; // Display's selected prisoner sprite
        [SerializeField] private Text imageVictimName = null;
        private List<Sprite> victimsSprites = new List<Sprite>(); // List of the prisoner images from spritesheet
        private static VictimsListSlideshowController Instance;
        public static VictimsListSlideshowController Current {
            get {
                if (Instance == null) Instance = FindObjectOfType<VictimsListSlideshowController>();
                return Instance;
            }
        }
        private WaitForSeconds textChangeDelay = new WaitForSeconds(1);
        private WaitForSeconds imageDisplayDuration = new WaitForSeconds(10);

        private int spriteIndex = 0; // Index of currently activated sprite selected from victimSprites

        private void Awake() {
            victimsDisplayCG.alpha = 0;
            GetVictimSprites();
        }
        private void GetVictimSprites() {
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites/VictimsSS")) {
                victimsSprites.Add(sprite);
            }
        }

        // Called from VictimsListTextController
        public void StartSlideshow() {
            StartCoroutine(SlideshowControl());
        }

        /// <summary>
        /// As long as all sprites havent been used, loops a fade in/out animation
        /// on the holocaust victim image display. Contains function calls to select images
        /// at random as well as to set up their names for the display.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SlideshowControl() {
            while (victimsSprites.Count != 0) {
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
            }
        }

        /// <summary>
        /// Grabs a random sprite image from Victims spritesheet and sets it to active in 
        /// the scene for user to view. A function call to setup the display name is also here
        /// </summary>
        private Sprite GrabRandomImage() {
            spriteIndex = Random.Range(0, victimsSprites.Count);
            string tempName = victimsSprites[spriteIndex].name;

            if (tempName.ToLower().Contains("unknown")) {
                imageVictimName.text = "";
            } else {
                imageVictimName.text = SetNameForPhotograph(tempName);
            }
            return victimsSprites[spriteIndex];
        }

        /// <summary>
        /// Returns the formatted name, age and status of person chosen
        /// </summary>
        private string SetNameForPhotograph(string tempName) {
            string[] tempNameArray = tempName.Split('_');

            // If middle name included
            if (tempNameArray.Length == 4) {
                // If "survived" is where age would be
                if (tempNameArray[3] == "survived") {
                    return $"{tempNameArray[0]} {tempNameArray[1]} {tempNameArray[2]}.\nSurvived";
                } else {
                    return $"{tempNameArray[0]} {tempNameArray[1]} {tempNameArray[2]}.\nDied age: {tempNameArray[3]}";
                }
            }
            // If only first and last name given
            else if (tempNameArray.Length == 2) {
                return $"{tempNameArray[0]} {tempNameArray[1]}. Age/whether or not they survived unknown";
            }
            // If name and age given
            else {
                if (tempNameArray[2] == "survived") {
                    return $"{tempNameArray[0]} {tempNameArray[1]}.\nSurvived";
                } else {
                    return $"{tempNameArray[0]} {tempNameArray[1]}.\nDied age: {tempNameArray[2]}";
                }
            }
        }
    }
}
