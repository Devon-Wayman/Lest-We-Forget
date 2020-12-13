using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class SceneSetup : MonoBehaviour {

    [HideInInspector] public bool overrideFarClip;
    [HideInInspector] public int farDistanceOverride;

    private Camera userCam = null; 

    private void Awake() {
        if (!overrideFarClip) return;

        userCam = Camera.main; 

        if(overrideFarClip){
            userCam.farClipPlane = farDistanceOverride;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SceneSetup))]
public class SceneSetup_Editor : Editor {
    public override void OnInspectorGUI(){
        DrawDefaultInspector(); // For non hidden inspector fields to be displayed
        SceneSetup sceneSetup = (SceneSetup)target; // Set reference to main SceneSetup script

        sceneSetup.overrideFarClip = EditorGUILayout.Toggle("Override Far Clip", sceneSetup.overrideFarClip);

        if (sceneSetup.overrideFarClip){
            sceneSetup.farDistanceOverride = EditorGUILayout.IntSlider("Far Distance Override", sceneSetup.farDistanceOverride, 1, 2000);
        }
    }
}
#endif