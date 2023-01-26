// Author: Devon Wayman - December 2020
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LWF.Interaction {
    public class VictimMemorialController : MonoBehaviour {

        [Header("Slideshow Items")]
        [SerializeField] private CanvasGroup photosCanvas; // Reference to text content of victims rect
        [SerializeField] private Image victimImage; // Display's selected prisoner sprite
        [SerializeField] private TMP_Text victimName;

        [SerializeField] private List<Sprite> victimsSprites = new();
        private int spriteIndex = 0;
        private int imageDisplayDuration = 5;
        private int imageBreakDuration = 1;
        [SerializeField] private int maxImagesToDisplay = 0;


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
            };

        private string[] closingQuotesArray = {
            "'We honor lost loved ones by sharing the lessons of their past to ensure a better future for all' - Judy and Mark Mucasey",
            "'It is our responsibility to inspire future generations to stand up against hatred, prejudice, and evil' - Kisha and Jason Itkin",
            "'The lessons of the Holocaust are not Jewish, but universal. And unfortunately, the lessons remain relevant today' - Sunni and Gary Markowitz",
            "'It is important to understand that big changes, the kind that transforms the way human beings handle being human, start with small changes' - Naomi Warrn",
            "'How wonderful it is that nobody need wait a single moment before starting to improve the world' - Anne Frank",
            "'Forgetting extermination is part of extermination' - Jean Baudrillard",
            "'No matter your race, religion, belief system or gender, humans have to be treated equally' - Eddie Jaku",
            "'It will not be long before there will be no first-hand survivors alive. And it is important to record this testimony as evidence for future generations' - Manfred Goldberg",
            "'It is up to us, Jews and non-Jews, to stop antisemitism' - Allen Firestone",
            "'Why did I survive while my parents and my brother didn't?' - Jannie Webber",
            "'It all happened so fast. The ghetto. The deportation. The sealed cattle car. The fiery altar upon which the history of our people and the future of mankind were meant to be sacrificed' - Elie Weisel",
            "'Thou shalt not be a victim, thou shalt not be a perpetrator, but, above all, thou shalt not be a bystander' - Yehuda Bauer",
            "'Because I remember, I despair. Because I remember, I have the duty to reject despair' - Elie Wiesel",
            "'To forget the Holocaust is to kill twice' - Elie Wiesel",
            "'Despite everything, I believe that people are really good at heart' - Anne Frank",
            "'My number is 174517. We have been baptized, we will carry the tattoo on our left arm until we die' - Primo Levi",
            "'You have to have faith, fantasy, hope, drive, determination, and the belief that tomorrow will be better' - Magda Brown",
            "'Never shall I forget the little faces of the children, whose bodies turned into wreaths of smoke beneath a silent blue sky' - Elie Wisel",
            "'As the generation of Holocaust survivors and liberators dwindles, the torch of remembrance, of bearing witness, and of education must continue forward' - Dan Gillerman"
        };
        private string quoteSelection = string.Empty; // Saves the random selection for closing quotes above


        private List<string> namesFromList = new List<string>();
        private List<string> countryFromList = new List<string>();

        private void Awake() {
            LoadAllSprites();

            List<string> quotes = closingQuotesArray.ToList();
            quoteSelection = quotes.RandomListSelection();

            photosCanvas.alpha = 0;

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

            string stringOfNames = string.Join("\n", namesFromList);
            nameAndDateText.text = stringOfNames;

            string stringOfCountries = string.Join("\n", countryFromList);
            countryText.text = stringOfCountries;
        }

        private void LoadAllSprites() {
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites/VictimsSS")) {
                victimsSprites.Add(sprite);
            }
        }

        private void Start() {
            maxImagesToDisplay = Random.Range(10, victimsSprites.Count - 4);
            Debug.Log($"Will only display {maxImagesToDisplay + 1} images in slideshow");
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
                        ShowEndQuote();
                    }
                });
            });
        }

        private void ShowEndQuote() {
            openingText.text = quoteSelection;

            LeanTween.alphaCanvas(openersCanvas, 0.8f, 1).setDelay(textChangeDelay).setOnComplete(() => {
                LeanTween.alphaCanvas(openersCanvas, 0, 1).setDelay(textDisplayDuration).setOnComplete(() => {
                    StartSlideshow();
                });
            });
        }

        private void StartSlideshow() {
            victimImage.sprite = victimsSprites.RandomListSelection();
            spriteIndex = victimsSprites.IndexOf(victimImage.sprite);
            victimName.text = VictimName(victimImage.sprite.name);

            LeanTween.alphaCanvas(photosCanvas, 1, 2).setDelay(imageBreakDuration).setOnComplete(() => {
                LeanTween.alphaCanvas(photosCanvas, 0, 2).setDelay(imageDisplayDuration).setOnComplete(() => {
                    victimsSprites.RemoveAt(spriteIndex);

                    // If no more sprites after removing most recent, start scrolling list
                    if (maxImagesToDisplay == 0) {
                        DisplayScrollingList();
                        return;
                    }

                    maxImagesToDisplay--;
                    StartSlideshow();
                });
            });
        }

        private void DisplayScrollingList() {
            openingText.text = "And many more...";

            LeanTween.alphaCanvas(openersCanvas, 0.8f, 1).setDelay(textChangeDelay).setOnComplete(() => {
                LeanTween.alphaCanvas(openersCanvas, 0, 1).setDelay(textDisplayDuration).setOnComplete(() => {
                    ScrollNamesList();
                });
            });
        }

        private void ScrollNamesList() {
            listCanvas.gameObject.SetActive(true);
            LeanTween.alphaCanvas(listCanvas, 0.8f, 1).setDelay(textChangeDelay).setOnComplete(() => {
                LeanTween.moveLocalY(listCanvas.gameObject, 400, 500);
            });
        }


        /// <summary>
        /// Returns properly formated string for victims image display along
        /// with their age and if they survived when available.
        /// </summary>
        /// <param name="tempName"></param>
        /// <returns></returns>
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

                    // If under 1 year of age when killed
                    if (temp[3].ToLower().Contains("u")) {
                        temp[3] = temp[3].Remove(0, 1);
                        return $"{temp[0]} {temp[1]} {temp[2]}\nMurdered at less than {temp[3]} year old";
                    }

                    return $"{temp[0]} {temp[1]} {temp[2]}\nMurdered at age {temp[3]}";
                }
            }
            // If first and last name only
            else if (temp.Length == 2) {
                return $"{temp[0]} {temp[1]}. Age/survival status: Unknown";
            }
            // Name and age given
            else {
                if (temp[2] == "survived") {
                    return $"{temp[0]} {temp[1]}\nSurvived";
                } else {

                    // If under 1 year of age when killed
                    if (temp[2].ToLower().Contains("u")) {
                        temp[2] = temp[2].Remove(0, 1);
                        return $"{temp[0]} {temp[1]}\nMurdered at less than {temp[2]} year old";
                    }

                    return $"{temp[0]} {temp[1]}\nMurdered at age {temp[2]}";
                }
            }
        }
    }
}