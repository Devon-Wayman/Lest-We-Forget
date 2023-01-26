// Author: Devon Wayman - December 2021
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

    public static List<T> FindAllInScene<T>() where T : Component {
        List<T> results = new();
        T[] allObjsAry = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];

        for (var i = 0; i < allObjsAry.Length; ++i) {
            results.Add(allObjsAry[i]);
        }

        if (results.Count <= 0) {
            Debug.LogWarning("Unable to find any of requested objects in scene");
            return null;
        }

        Debug.Log($"Found {results.Count} items.");
        return results;
    }

    public static T RandomListSelection<T>(this IList<T> list) {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static void FadeCanvasGroup(this CanvasGroup canvas, float desiredAlpha, float transitionTime) {
        LeanTween.alphaCanvas(canvas, desiredAlpha, transitionTime);
    }

    public static void FadeCanvasGroup(this CanvasGroup canvas, float desiredAlpha, float transitionTime, System.Action callback) {
        LeanTween.alphaCanvas(canvas, desiredAlpha, transitionTime).setOnComplete(() => {
            callback?.Invoke();
        });
    }

    public static void FadeCanvasGroup(this CanvasGroup canvas, float desiredAlpha, float transitionTime, System.Action callback, float delay) {
        LeanTween.alphaCanvas(canvas, desiredAlpha, transitionTime).setOnComplete(() => {
            callback?.Invoke();
        }).setDelay(delay);
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
        if (gameObject.TryGetComponent<T>(out T requestedComponent)) {
            return requestedComponent;
        }
        return gameObject.AddComponent<T>();
    }

    public static void ChangeValueOverTime(this float val, float from, float to) {
        LeanTween.easeInExpo(from, to, val);
    }
}