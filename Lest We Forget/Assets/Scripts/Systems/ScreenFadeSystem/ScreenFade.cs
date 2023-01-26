using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScreenFade : DevSingleton<ScreenFade> {

    [SerializeField] private CanvasGroup screenFadeCanvas = null;

    public UnityEvent OnLoadBegin = new UnityEvent();
    public UnityEvent OnLoadEnd = new UnityEvent();

    private bool isLoading = false;

    private static float fadeDuration = 2f;
    private WaitForSeconds fadeDurationWait = new WaitForSeconds(fadeDuration);

    private void OnEnable() {
        Debug.Log("instantiating screen fade material");
        SceneManager.sceneLoaded += SetActiveScene;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= SetActiveScene;
    }

    private void Start() {
        Debug.Log("Start called on ScreenFade");
        FadeOut();
        isLoading = false;
    }

    public void LoadNewScene(int sceneIndex) {
        if (isLoading) return;

        Debug.Log("LoadNewScene called");


        LeanTween.alphaCanvas(screenFadeCanvas, 1, 2).setOnComplete(() => {
            StartCoroutine(LoadScene(sceneIndex));
        });

    }

    private IEnumerator LoadScene(int sceneIndex) {
        isLoading = true;
        OnLoadBegin?.Invoke();

        float newAudVal;

        while (AudioListener.volume >= 0) {
            newAudVal = AudioListener.volume - 0.1f * Time.deltaTime;
            Debug.Log(newAudVal);
            AudioListener.volume = newAudVal;
        }

        yield return StartCoroutine(UnloadCurrent());
        yield return StartCoroutine(LoadNew(sceneIndex));

        FadeOut();
    }

    private IEnumerator UnloadCurrent() {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        while (!unloadOperation.isDone) {
            yield return null;
        }
    }

    private IEnumerator LoadNew(int sceneIndex) {
        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        while (!loadSceneOperation.isDone) {
            yield return null;
        }
    }

    private void SetActiveScene(Scene scene, LoadSceneMode mode) {
        SceneManager.SetActiveScene(scene);
    }

    private void FadeOut() {
        LeanTween.alphaCanvas(screenFadeCanvas, 0, 10).setOnComplete(() => {
            OnLoadEnd?.Invoke();
            isLoading = false;
        });
    }
}
