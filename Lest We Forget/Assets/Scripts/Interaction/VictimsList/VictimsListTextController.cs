// Author: Devon Wayman - March 2021
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

namespace LWF.Interaction {
    public class VictimsListTextController : MonoBehaviour {

        [Header("Opening Remarks")]
        public CanvasGroup openingCanvasGroup = null;
        [SerializeField] TMP_Text openingRemarksText = null;

        public CanvasGroup listCanvasGroup = null;
        public GameObject listTextHolder;


        [Header("Person Info")]
        [SerializeField] TMP_Text nameAndDateText = null;
        [SerializeField] TMP_Text countryText = null;

        static VictimsListTextController Instance;
        public static VictimsListTextController Current {
            get {
                if (Instance == null) Instance = FindObjectOfType<VictimsListTextController>();
                return Instance;
            }
        }

        WaitForSeconds changeDelay = new WaitForSeconds(1);
        WaitForSeconds textDisplayDuration = new WaitForSeconds(8);

        bool canScroll = false;
        int textIndex;
        string[] openingRemarksArray = new string[] {
            "In March 1933 after Adolf Hitler became Chancellor of Germany, the first concentration camps were built",
            "Over the years more camps would be established and used to bring death to many innocent people",
            "The last camp liberated by the Allies was Stutthof in Sztutowo Poland on May 9th, 1945" ,
            "Through the years of their operation, these camps were used to claim the lives of many people",
            "According to the United States Holocaust Memorial Museum approximately 2.7 million people's lives were claimed in these living hells",
            "Listed are just a small number of those who's lives were taken",
            "'Everybody, every human being has the obligation to contribute somehow to this world' - Edith Carter"
            };

        List<string> namesFromList = new List<string>();
        List<string> countryFromList = new List<string>();

        void Awake() {
            listCanvasGroup.alpha = openingCanvasGroup.alpha = textIndex = 0;
            listCanvasGroup.gameObject.SetActive(false);

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

            // Set names and birth/death dates to single text in UI
            var arrayOfNames = namesFromList.ToArray();
            string stringOfNames = string.Join("\n", namesFromList);
            nameAndDateText.text = stringOfNames;

            // Set names and birth/death dates to single text in UI
            var arrayOfCountries = countryFromList.ToArray();
            string stringOfCountries = string.Join("\n", countryFromList);
            countryText.text = stringOfCountries;
        }
        void Start() {
            StartCoroutine(OpeningTextControl());
        }

        void Update() {
            if (!canScroll) return;
            listTextHolder.transform.position += transform.up * Time.deltaTime * 0.3f;
        }

        public void BeginListScroll() {
            StartCoroutine(ListScrollRoutine());
        }
        private IEnumerator ListScrollRoutine() {
            openingRemarksText.text = "And many more...";
            openingCanvasGroup.FadeCanvasGroup(0.8f, 1);
            yield return textDisplayDuration;
            openingCanvasGroup.FadeCanvasGroup(0, 0.5f);
            yield return new WaitForSeconds(0.5f);
            openingCanvasGroup.gameObject.SetActive(false);
            listCanvasGroup.gameObject.SetActive(true);
            listCanvasGroup.FadeCanvasGroup(0.8f, 1);
            canScroll = true;
        }

        IEnumerator OpeningTextControl() {
            while (textIndex < openingRemarksArray.Length) {
                openingRemarksText.text = openingRemarksArray[textIndex];
                yield return changeDelay;
                openingCanvasGroup.FadeCanvasGroup(0.8f, 1);
                yield return textDisplayDuration;
                openingCanvasGroup.FadeCanvasGroup(0, 1);
                yield return changeDelay;
                textIndex++;
            }

            VictimsListSlideshowController.Current.StartSlideshow();
        }
    }
}
