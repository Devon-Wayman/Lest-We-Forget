using UnityEditor;
using UnityEngine;

public class RemoveSceneIDMap : MonoBehaviour {
    [MenuItem("Tools/SceneIDMap Fixer")]
    public static void KillSceneIdMap() {
        var obj = GameObject.Find("SceneIDMap");
        if (obj != null) {
            DestroyImmediate(obj);
            Debug.Log("Cleared a SceneIDMap instance");
        }
    }
}