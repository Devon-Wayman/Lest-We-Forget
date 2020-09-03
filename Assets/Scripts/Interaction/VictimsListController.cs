// Author: Devon Wayman
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used to control UI for the victims list scene
namespace WWIIVR.ApplicationSettings {
    public class VictimsListController : MonoBehaviour {

        public GameObject textHolder;
        private int openerTextIndex; // Index number to determine what the opener text should read for the VictimsList scene
        Text openerText, victimNames;
        public CanvasGroup openerCG, victimsListCG, victimsDisplayCG; // Reference to text content of victims rect
        private Image victimImageDisplay; // Image holder to pass in images from sprite sheet 
        private List<Sprite> victimsSprites = new List<Sprite>(); // List of the survivor images pulled from a sprite sheet
        private bool allowAutoScroll; // Allow autoscrolling of the victims list

        private void Awake() {
            victimImageDisplay = victimsDisplayCG.GetComponent<Image>();
            openerText = openerCG.GetComponent<Text>();
            victimNames = victimsListCG.GetComponent<Text>();
            victimsListCG.alpha = openerCG.alpha = victimsDisplayCG.alpha = openerTextIndex = 0;

            foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites/VictimsSS"))
                victimsSprites.Append(sprite);
        }
        private void Start() {
            var textFile = Resources.Load<TextAsset>("victims"); // Load victims text file
            string rawText = textFile.ToString();
            victimNames.text = rawText;
            UpdateOpenerText();
            StartCoroutine(FadeOpenerText()); // Check opener index if we are in the Victims scene after all references are create
        }
        private void Update() {
            if (!allowAutoScroll) return;
                
            textHolder.transform.position += transform.up * Time.deltaTime * 0.3f;
        }

        #region Opening statements
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
            yield return new WaitForSeconds(2);

            while (openerCG.alpha < 0.8f) {
                openerCG.alpha += 0.8f * Time.deltaTime;
                yield return null;
            }
            openerCG.alpha = 0.8f;
            yield return new WaitForSeconds(5);
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
                // BEGIN SLIDESHOW
                StartCoroutine(SlideshowControl());
                StartCoroutine(FadeInList());
                allowAutoScroll = true;
            }
        }
        #endregion

        #region Slideshow and Scroll Text
        // Fade slideshow images in and out and change randomly
        private IEnumerator SlideshowControl() {
            victimImageDisplay.sprite = GrabRandomImage();

            yield return new WaitForSeconds(3);
            while (victimsDisplayCG.alpha < .8f) {
                victimsDisplayCG.alpha += 0.8f * Time.deltaTime;
                yield return null;
            }
            victimsDisplayCG.alpha = 0.8f;
            yield return new WaitForSeconds(8);
            while (victimsDisplayCG.alpha > 0) {
                victimsDisplayCG.alpha -= 0.8f * Time.deltaTime;
                yield return null;
            }
            victimsDisplayCG.alpha = 0;
            StartCoroutine(SlideshowControl()); // Pick another image and start again
        }
        // GRAB RANDOM IMAGE FROM VICTIMS SPRITE SHEET
        private Sprite GrabRandomImage() {
            int index = Random.Range(0, victimsSprites.Count);
            return victimsSprites[index];
        }

        private IEnumerator FadeInList() {
            while (victimsListCG.alpha < 0.9f) {
                victimsListCG.alpha += 0.8f * Time.deltaTime;
                yield return null;
            }
            victimsListCG.alpha = 0.9f;
            yield break;
        }
        #endregion
    }
}
