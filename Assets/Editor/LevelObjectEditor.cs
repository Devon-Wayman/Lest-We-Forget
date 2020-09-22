// Author Devon Wayman 
// Date Sept 22 2020
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(LevelObject))]
public class LevelObjectEditor : Editor {
    public override void OnInspectorGUI() {
        // Cast target
        var levelObjectScript = target as LevelObject;

        //Enum drop down
        levelObjectScript.sceneType = (LevelObject.SceneType)EditorGUILayout.EnumPopup(levelObjectScript.sceneType);

        //switch statement for different variables
        switch (levelObjectScript.sceneType) {

            // If simply loading a scene is needed with no other 
            case LevelObject.SceneType.SimpleScene:
                levelObjectScript.sceneName = EditorGUILayout.TextField("Scene name", levelObjectScript.sceneName); //Vector3 example
                break;

            // If loading a vehicle in a vehicle scene
            case LevelObject.SceneType.Vehicle:
                levelObjectScript.vehicleScene = EditorGUILayout.TextField("Vehicle scene", levelObjectScript.vehicleScene);
                levelObjectScript.requestedVehicle = EditorGUILayout.TextField("Vehicle", levelObjectScript.requestedVehicle);
                break;

            // If loading a film for the theater scene
            case LevelObject.SceneType.Film:
                levelObjectScript.movieName = EditorGUILayout.TextField("Movie name", levelObjectScript.movieName);
                break;
        }
    }
}
#endif


