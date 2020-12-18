#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO;

public class RemoveAppSettings : MonoBehaviour {

    [MenuItem("Tools/Delete App Settings JSON")]
    public static void DeleteAppSettings() {
        if(File.Exists(Application.persistentDataPath + "/wwiisettings.json")) {
            File.Delete(Application.persistentDataPath + "/wwiisettings.json");
            Debug.Log($"Deleted app settings file at {Application.persistentDataPath}/wwiisettings.json");
        } else {
            Debug.Log("Settings file not found. Nothing will be removed");
        }
    }
}
