// Author: Devon Wayman - March 2021
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LWF.Interaction {
    public class VictimsListTextController : MonoBehaviour {

        public CanvasGroup openerCG = null;
        public CanvasGroup victimsListCG = null;
        public GameObject listTextHolder;
        public Text openerText = null;
        public Text scrollingVictimNames = null;
        private WaitForSeconds textChangeDelay = new WaitForSeconds(0.5f);
        private WaitForSeconds textDisplayDuration = new WaitForSeconds(7);

        private bool canScroll = false;
        private int textIndex;
        private string[] openingTextArray = new string[] { "Hey", "Sup fam" };

        private void Awake() {
            victimsListCG.alpha = openerCG.alpha = textIndex = 0;
            victimsListCG.gameObject.SetActive(false);
            var textFile = Resources.Load<TextAsset>("Text/victims");
            string rawText = textFile.ToString();
            scrollingVictimNames.text = rawText;
        }

        private void Start() {
            StartCoroutine(OpeningTextControl());
        }
        private void Update() {
            if (!canScroll) return;
            listTextHolder.transform.position += transform.up * Time.deltaTime * 0.3f;
        }

        private IEnumerator OpeningTextControl() {

            while (textIndex < openingTextArray.Length) {
                openerText.text = openingTextArray[textIndex];

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
                textIndex++;
            }

            VictimsListSlideshowController.Current.StartSlideshow();
            StartCoroutine(FadeInList());
        }

        private IEnumerator FadeInList() {
            openerText.gameObject.SetActive(false);
            canScroll = true;
            victimsListCG.gameObject.SetActive(true);

            while (victimsListCG.alpha < 0.9f) {
                victimsListCG.alpha += 0.8f * Time.deltaTime;
                yield return null;
            }

            victimsListCG.alpha = 0.9f;
            yield break;
        }
    }
}
