// Author: Devon Wayman
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class SceneSetup : MonoBehaviour {

    [HideInInspector] public bool overrideFarClip; // Bool to allow far clip override
    [HideInInspector] public int farDistanceOverride;

    private Camera userCam = null; // User's vr headset cam

    private void Awake() {
        if (!overrideFarClip)
            return;

        userCam = Camera.main; // Get the camera

        // If overrideFarClip is enabled, change the farclip value to that assigned by user
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

        // Draw checkboxes for bools
        sceneSetup.overrideFarClip = EditorGUILayout.Toggle("Override Far Clip", sceneSetup.overrideFarClip);

        // Show overrride for far clip plane
        if (sceneSetup.overrideFarClip){
            sceneSetup.farDistanceOverride = EditorGUILayout.IntSlider("Far Distance Override", sceneSetup.farDistanceOverride, 1, 2000);
        }
    }
}
#endif