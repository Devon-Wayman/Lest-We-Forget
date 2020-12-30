// Author Devon Wayman - December 2020
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace LWF.Localisation {
    public class SetDefaultLanguage : MonoBehaviour {

        void Start() {
            StartCoroutine(setDefaultLanguage());
        }

        public IEnumerator setDefaultLanguage() {
            // Wait for the localization system to initialize
            yield return LocalizationSettings.InitializationOperation;

            var langauge = Application.systemLanguage;

            if (langauge == SystemLanguage.French) {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)LocalisationIndexes.French];
            } else if (langauge == SystemLanguage.German) {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)LocalisationIndexes.German];
            } else if (langauge == SystemLanguage.Spanish) {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)LocalisationIndexes.Spanish];
            } else {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)LocalisationIndexes.English];
            }
        }
    }
}
