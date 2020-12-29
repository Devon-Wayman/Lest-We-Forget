#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO;

public class RemoveAppSettings : MonoBehaviour {

    [MenuItem("LWF/Delete App Settings JSON")]
    public static void DeleteAppSettings() {
        if(File.Exists(Application.persistentDataPath + "/lwfsettings.json")) {
            File.Delete(Application.persistentDataPath + "/lwfsettings.json");
            Debug.Log($"Deleted app settings file at {Application.persistentDataPath}/lwfsettings.json");
        } else {
            Debug.Log("Settings file not found. Nothing will be removed");
        }
    }
}
