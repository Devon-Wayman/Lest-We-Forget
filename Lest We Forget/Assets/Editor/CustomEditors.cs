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

    [MenuItem("LWF/Save System/Delete Save File")]
    public static void DeleteSave() {
        try {
            File.Delete(Application.persistentDataPath + "/LWFSave.sav");
            Debug.Log($"Save file found and removed from {Application.persistentDataPath}/LWFSave.sav");
        } catch (Exception ex) {
            Debug.LogWarning($"Unable to delete save file: {ex.Message}");
        }
    }

    [MenuItem("LWF/Save System/Create Default Save")]
    public static void CreateSaveDefaults() {
        if (File.Exists(Application.persistentDataPath + "/LWFSave.sav")) {
            Debug.LogWarning("Save file already exists at {Application.persistentDataPath}/LWFSave.sav. Please delete before requesting to swap out for default values");
            return;
        }

        try {
            SaveLoadManager.CreateDefaultFile();
            Debug.Log($"Default save file created at {Application.persistentDataPath}/LWFSave.sav");
        } catch (Exception ex) {
            Debug.LogWarning($"Unable to create save file at {Application.persistentDataPath}/LWFSave.sav: {ex.Message}");
        }
    }
}