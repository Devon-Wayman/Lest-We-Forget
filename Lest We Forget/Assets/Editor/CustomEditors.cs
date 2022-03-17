using System;
using UnityEngine;
using System.IO;
using LWF;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CustomEditors : MonoBehaviour {
    [MenuItem("LWF/SceneIDMap Fixer")]
    public static void KillSceneIdMap() {
        var obj = GameObject.Find("SceneIDMap");
        if (obj != null) {
            DestroyImmediate(obj);
            Debug.Log("Cleared a SceneIDMap instance");
        }
    }
}