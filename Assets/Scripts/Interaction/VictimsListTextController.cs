// Author: Devon Wayman
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace LWF.Interaction {
    public class VictimsListTextController : MonoBehaviour {        

        public CanvasGroup openerCG = null;
        public CanvasGroup victimsListCG = null;
        public GameObject listTextHolder;
        public LocalizedString openerTextTranslation = new LocalizedString();
        public Text openerText = null;
        public Text scrollingVictimNames = null;
        private WaitForSeconds textChangeDelay = new WaitForSeconds(0.5f);
        private WaitForSeconds textDisplayDuration = new WaitForSeconds(7);

        private bool canScroll = false;
        private int openerTextIndex;


        private void Awake() {
            openerTextTranslation.StringChanged += OnStringChange;

            victimsListCG.alpha = openerCG.alpha = openerTextIndex = 0;
            victimsListCG.gameObject.SetActive(false);

            var textFile = Resources.Load<TextAsset>("Text/victims");
            string rawText = textFile.ToString();
            scrollingVictimNames.text = rawText;
        }
        

        private void Start() {
            UpdateOpenerText();
            StartCoroutine(FadeOpenerText()); 
        }
        private void Update() {
            if (!canScroll) return;
            listTextHolder.transform.position += transform.up * Time.deltaTime * 0.3f;
        }

        private void OnStringChange(string value) {
            openerText.text = value;
        }
        private void UpdateOpenerText() {     
            openerTextTranslation.TableEntryReference = $"line{openerTextIndex + 1}";
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
                yield break;
            }
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
