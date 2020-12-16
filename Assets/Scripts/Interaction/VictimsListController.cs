// Author: Devon Wayman
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used to control UI for the victims list scene
namespace WWIIVR.ApplicationSettings {
    public class VictimsListController : MonoBehaviour {        
        public GameObject textHolder;
        private int openerTextIndex; 

        public Text openerText = null;
        public Text scrollingVictimNames = null;
        public Text imageVictimName = null;

        public CanvasGroup openerCG = null;
        public CanvasGroup victimsListCG = null;
        public CanvasGroup victimsDisplayCG = null; // Reference to text content of victims rect

        private Image victimImageDisplay; // Display's selected prisoner sprite
        
        private List<Sprite> victimsSprites = new List<Sprite>(); // List of the prisoner images from spritesheet
        
        private bool allowScroll; // Enables/disabled scrolling of the victims list

        // Cached wait for seconds used for text fade and display time delays
        private WaitForSeconds textChangeDelay = new WaitForSeconds(1);
        private WaitForSeconds textDisplayDuration = new WaitForSeconds(7);
        private WaitForSeconds imageDisplayDuration = new WaitForSeconds(10);

        private void Awake() {
            victimImageDisplay = victimsDisplayCG.GetComponent<Image>();
            openerText = openerCG.GetComponent<Text>();
            scrollingVictimNames = victimsListCG.GetComponent<Text>();
            victimsListCG.alpha = openerCG.alpha = victimsDisplayCG.alpha = openerTextIndex = 0;
            victimsListCG.gameObject.SetActive(false);
            victimsDisplayCG.gameObject.SetActive(false);
            var textFile = Resources.Load<TextAsset>("victims"); // Load victims text file
            GetVictimSprites();
            string rawText = textFile.ToString();
            scrollingVictimNames.text = rawText;
        }
        private void Start() {
            UpdateOpenerText();
            StartCoroutine(FadeOpenerText()); // Check opener index if we are in the Victims scene after all references are create
        }
        private void GetVictimSprites() {
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites/VictimsSS"))
                victimsSprites.Add(sprite);
        }

        private void Update() {
            if (!allowScroll) return;
                
            textHolder.transform.position += transform.up * Time.deltaTime * 0.3f;
        }

        private void UpdateOpenerText() {
            switch (openerTextIndex) {
                case 0:
                    openerText.text = "In the year 1941, Nazi-occupied regions\nbegan murdering Jews";
                    break;
                case 1:
                    openerText.text = "Over 6 million; two-thirds of Europes Jewish population";
                    break;
                case 2:
                    openerText.text = "These mass murders did not end until 1943";
                    break;
                case 3:
                    openerText.text = "Within this two year span, entire innocent\nfamilies were wiped from the face of the Earth";
                    break;
                case 4:
                    openerText.text = "Listed are just some of the names of the innocent\n people who had their lives taken away too soon";
                    break;
                case 5:
                    openerText.text = "\"To forget the dead would be akin to killing\nthem a second time\" - Elie Wiesel Night ";
                    break;
            }
        }
        private IEnumerator FadeOpenerText() {
            yield return textChangeDelay;

            while (openerCG.alpha < 0.8f) {
                openerCG.alpha += 0.8f * Time.deltaTime;
                yield return null;
            }
            openerCG.alpha = 0.8f;

            yield return textDisplayDuration;

            while (openerCG.alpha > 0f) {
                openerCG.alpha -= 0.8f * Time.deltaTime;
                yield return null;
            }

            openerCG.alpha = 0;
            openerTextIndex++;

            if (!(openerTextIndex > 5)) {
                UpdateOpenerText();
                StartCoroutine(FadeOpenerText());
            } else {
                StartCoroutine(SlideshowControl());
                StartCoroutine(FadeInList());
                allowScroll = true;
            }
        }


        // Used to change person image and name as well as fade in and out
        #region Slideshow and Scroll Text
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
            StartCoroutine(SlideshowControl()); 
        }


        private Sprite GrabRandomImage() {
            int index = Random.Range(0, victimsSprites.Count);

            string tempName = victimsSprites[index].name;

            if (tempName.ToLower().Contains("unknown")) {
                imageVictimName.text = "";
            } else {
                string[] tempArray = tempName.Split('_');

                // If middle name included
                if (tempArray.Length == 4) {
                    // If survived is where age would be
                    if (tempArray[3] == "survived") {
                        imageVictimName.text = $"{tempArray[0]} {tempArray[1]} {tempArray[2]}.\nSurvived";
                    } else {
                        imageVictimName.text = $"{tempArray[0]} {tempArray[1]} {tempArray[2]}.\nDied age: {tempArray[3]}";
                    }
                } 
                // If only first and last name given
                else if (tempArray.Length == 2) {
                    imageVictimName.text = $"{tempArray[0]} {tempArray[1]}. Age/whether or not they survived unknown";
                } 
                // If name and age given
                else {
                    if (tempArray[2] == "survived") {
                        imageVictimName.text = $"{tempArray[0]} {tempArray[1]}.\nSurvived";
                    } else {
                        imageVictimName.text = $"{tempArray[0]} {tempArray[1]}.\nDied age: {tempArray[2]}";
                    }
                    
                }
            }
                return victimsSprites[index];
        }

        // Fade in the long list of names (runs once)
        private IEnumerator FadeInList() {
            openerText.gameObject.SetActive(false);

            victimsListCG.gameObject.SetActive(true);
            victimsDisplayCG.gameObject.SetActive(true);

            while (victimsListCG.alpha < 0.9f) {
                victimsListCG.alpha += 0.8f * Time.deltaTime;
                yield return null;
            }
            victimsListCG.alpha = 0.9f;
        }
        #endregion
    }
}
