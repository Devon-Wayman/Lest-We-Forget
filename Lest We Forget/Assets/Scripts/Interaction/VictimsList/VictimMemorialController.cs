// Author: Devon Wayman - December 2020
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LWF.Interaction {
    public class VictimMemorialController : MonoBehaviour {

        [Header("Slideshow Items")]
        [SerializeField] private CanvasGroup photosCanvas; // Reference to text content of victims rect
        [SerializeField] private Image victimImage; // Display's selected prisoner sprite
        [SerializeField] private TMP_Text victimName;

        private List<Sprite> victimsSprites = new List<Sprite>();
        private int spriteIndex = 0;
        private int imageDisplayDuration = 5;
        private int imageBreakDuration = 1;


        [Header("Opening and List Items")]
        [SerializeField] private CanvasGroup openersCanvas;
        [SerializeField] private TMP_Text openingText;
        [SerializeField] private CanvasGroup listCanvas;
        [SerializeField] private TMP_Text nameAndDateText;
        [SerializeField] private TMP_Text countryText;

        private int textChangeDelay = 1;
        private int textDisplayDuration = 8;
        private int textIndex = 0;
        private string[] openingRemarksArray = new string[] {
            "In March 1933 after Adolf Hitler became Chancellor of Germany, the first concentration camps were built",
            "Over the years more camps would be established and used to bring death to many innocent people",
            "The last camp liberated by the Allies was Stutthof in Sztutowo Poland on May 9th, 1945" ,
            "Through the years of their operation, these camps were used to claim the lives of many people",
            "According to the United States Holocaust Memorial Museum approximately 2.7 million people's lives were claimed in these living hells",
            "Listed are just a small number of those who's lives were taken",
            "'Everybody, every human being has the obligation to contribute somehow to this world' - Edith Carter"
            };
        private List<string> namesFromList = new List<string>();
        private List<string> countryFromList = new List<string>();


        private void Awake() {
            #region Slideshow Setup
            photosCanvas.alpha = 0;
            LoadAllSprites();
            #endregion

            #region Names List Setup
            listCanvas.alpha = openersCanvas.alpha = textIndex = 0;
            listCanvas.gameObject.SetActive(false);

            var textFile = Resources.Load<TextAsset>("Text/victims");
            string rawText = textFile.ToString();

            using (StringReader reader = new StringReader(rawText)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    string[] lineTemp = line.Split(',');
                    namesFromList.Add(lineTemp[0]);
                    countryFromList.Add(lineTemp[1]);
                }
            }

            var arrayOfNames = namesFromList.ToArray();
            string stringOfNames = string.Join("\n", namesFromList);
            nameAndDateText.text = stringOfNames;

            var arrayOfCountries = countryFromList.ToArray();
            string stringOfCountries = string.Join("\n", countryFromList);
            countryText.text = stringOfCountries;
            #endregion
        }

        private void Start() {
            ShowOpeningRemarks();
        }

        private void ShowOpeningRemarks() {
            openingText.text = openingRemarksArray[textIndex];

            LeanTween.alphaCanvas(openersCanvas, 0.8f, 1).setDelay(textChangeDelay).setOnComplete(() => {
                LeanTween.alphaCanvas(openersCanvas, 0, 1).setDelay(textDisplayDuration).setOnComplete(() => {
                    textIndex++;
                    if (!(textIndex >= openingRemarksArray.Length)) {
                        ShowOpeningRemarks();
                    } else {
                        StartSlideshow();
                    }
                });
            });
        }

        #region Slideshow
        private void LoadAllSprites() {
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites/VictimsSS")) {
                victimsSprites.Add(sprite);
            }
        }

        private void StartSlideshow() {
            Debug.Log("Slideshow called");

            victimImage.sprite = victimsSprites.RandomListSelection();
            spriteIndex = victimsSprites.IndexOf(victimImage.sprite);
            victimName.text = VictimName(victimImage.sprite.name);

            LeanTween.alphaCanvas(photosCanvas, 1, 2).setDelay(imageBreakDuration).setOnComplete(() => {
                LeanTween.alphaCanvas(photosCanvas, 0, 2).setDelay(imageDisplayDuration).setOnComplete(() => {
                    if (victimsSprites.Count != 0) StartSlideshow();
                });
            });
        }

        private string VictimName(string tempName) {
            if (tempName.ToLower().Contains("unknown")) {
                victimName.text = "";
                return "";
            }

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
        #endregion
    }
}