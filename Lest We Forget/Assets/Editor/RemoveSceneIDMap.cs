﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RemoveSceneIDMap : MonoBehaviour {
    [MenuItem("LWF/SceneIDMap Fixer")]
    public static void KillSceneIdMap() {
        var obj = GameObject.Find("SceneIDMap");
        if (obj != null) {
            DestroyImmediate(obj);
            Debug.Log("Cleared a SceneIDMap instance");
        }
    }
}