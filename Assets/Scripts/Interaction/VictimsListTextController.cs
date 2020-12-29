// Author: Devon Wayman
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Used to control all text aspects of the VictimsList scene
// NO LOGIC SHOULD BE PLACED HERE FOR THE SLIDESHOW PORTION
namespace LWF.Interaction {
    public class VictimsListTextController : MonoBehaviour {        
        public GameObject listTextHolder;
        private int openerTextIndex; 

        public Text openerText = null;
        public Text scrollingVictimNames = null;

        public CanvasGroup openerCG = null;
        public CanvasGroup victimsListCG = null;

  
        private bool canScroll = false; 

        // Cached wait for seconds used for text fade and display time delays
        private WaitForSeconds textChangeDelay = new WaitForSeconds(1);
        private WaitForSeconds textDisplayDuration = new WaitForSeconds(7);

        private void Awake() {
            openerText = openerCG.GetComponent<Text>();
            scrollingVictimNames = victimsListCG.GetComponent<Text>();
            victimsListCG.alpha = openerCG.alpha = openerTextIndex = 0;
            victimsListCG.gameObject.SetActive(false);
            var textFile = Resources.Load<TextAsset>("Text/victims"); // Load victims text file
            string rawText = textFile.ToString();
            scrollingVictimNames.text = rawText;
        }
        private void Start() {
            UpdateOpenerText();
            StartCoroutine(FadeOpenerText()); // Check opener index if we are in the Victims scene after all references are create
        }
        
        private void Update() {
            if (!canScroll) return;

            listTextHolder.transform.position += transform.up * Time.deltaTime * 0.3f;
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
                VictimsListSlideshowController.Current.StartSlideshow();
                StartCoroutine(FadeInList());
                canScroll = true;
            }
        }

        // Fade in the long list of names (runs once)
        private IEnumerator FadeInList() {
            openerText.gameObject.SetActive(false);

            victimsListCG.gameObject.SetActive(true);

            while (victimsListCG.alpha < 0.9f) {
                victimsListCG.alpha += 0.8f * Time.deltaTime;
                yield return null;
            }
            victimsListCG.alpha = 0.9f;
        }
    }
}
