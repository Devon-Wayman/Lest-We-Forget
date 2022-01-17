using UnityEngine;

namespace LWF.Managers {
    public class GUIManager : DevSingletonPersistent<GUIManager> {

        [Header("Scene Transition")]
        public CanvasGroup levelFadeCanvas;
        public float fadeTime = 3;


        protected override void Awake() {
            base.Awake();
        }


        public void FadeLevel(float desiredAlpha, System.Action callback) {
            levelFadeCanvas.gameObject.SetActive(true);

            LeanTween.alphaCanvas(levelFadeCanvas, desiredAlpha, fadeTime).setOnComplete(() => {
                callback?.Invoke();
                levelFadeCanvas.gameObject.SetActive(false);
            });
        }
    }
}
