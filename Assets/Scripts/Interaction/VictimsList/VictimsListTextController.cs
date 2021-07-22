// Author: Devon Wayman - March 2021
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace LWF.Interaction {
    public class VictimsListTextController : MonoBehaviour {

        [Header("Opening Remarks")]
        public CanvasGroup openingRemarksAlpha = null;
        public Text openingRemarksText = null;

        public CanvasGroup victimsList = null;
        public GameObject listTextHolder;


        [Header("Person Info")]
        public Text personsNameAndDates = null;
        public Text personsCountryOfOrigin = null;

        private static VictimsListTextController Instance;
        public static VictimsListTextController Current {
            get {
                if (Instance == null) Instance = FindObjectOfType<VictimsListTextController>();
                return Instance;
            }
        }

        private WaitForSeconds textDelay = new WaitForSeconds(1);
        private WaitForSeconds textDisplayDuration = new WaitForSeconds(7);

        private bool canScroll = false;
        private int textIndex;
        private string[] openingRemarksArray = new string[] { "Hello", "These opening remakrs will be\ndifferent in the release version" };

        private List<string> namesFromList = new List<string>();
        private List<string> countryFromList = new List<string>();

        private void Awake() {
            victimsList.alpha = openingRemarksAlpha.alpha = textIndex = 0;
            victimsList.gameObject.SetActive(false);
            var textFile = Resources.Load<TextAsset>("Text/victims");
            string rawText = textFile.ToString();

            // Split string after "," to different arrays
            using (StringReader reader = new StringReader(rawText)) {
                string line; // individual line from the raw text file
                while ((line = reader.ReadLine()) != null) {
                    string[] lineTemp = line.Split(',');
                    namesFromList.Add(lineTemp[0]);
                    countryFromList.Add(lineTemp[1]);
                }
            }

            // Set names and birth/death dates to single text in UI
            var arrayOfNames = namesFromList.ToArray();
            string stringOfNames = string.Join("\n", namesFromList);
            personsNameAndDates.text = stringOfNames;

            // Set names and birth/death dates to single text in UI
            var arrayOfCountries = countryFromList.ToArray();
            string stringOfCountries = string.Join("\n", countryFromList);
            personsCountryOfOrigin.text = stringOfCountries;
        }
        private void Start() {
            StartCoroutine(OpeningTextControl());
        }
        private void Update() {
            if (!canScroll) return;
            listTextHolder.transform.position += transform.up * Time.deltaTime * 0.3f;
        }
        public void ListToCenter() {
            LeanTween.moveX(listTextHolder, 0, 9).setEaseOutSine();
            LeanTween.rotateY(listTextHolder, 0, 9).setEaseOutSine();
        }
        private IEnumerator OpeningTextControl() {
            while (textIndex < openingRemarksArray.Length) {
                openingRemarksText.text = openingRemarksArray[textIndex];
                yield return textDelay;
                LeanTween.alphaCanvas(openingRemarksAlpha, 0.8f, 1);
                yield return textDisplayDuration;
                LeanTween.alphaCanvas(openingRemarksAlpha, 0, 1);
                yield return textDelay;
                textIndex++;
            }

            VictimsListSlideshowController.Current.StartSlideshow();
            canScroll = true;
            victimsList.gameObject.SetActive(true);
            openingRemarksText.gameObject.SetActive(false);
            LeanTween.alphaCanvas(victimsList, 0.9f, 1);
        }
    }
}
