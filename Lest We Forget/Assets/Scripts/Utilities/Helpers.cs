// Author: Devon Wayman - December 2021
using LWF.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LWF {
    public static class Extensions {

        public static List<T> FindAllInScene<T>() where T : Component {
            List<T> results = new List<T>();
            T[] allObjsAry = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];

            for (var i = 0; i < allObjsAry.Length; ++i) {
                results.Add(allObjsAry[i]);
            }

            if (results.Count <= 0) {
                Debug.LogWarning("Unable to find any of requested objects in scene");
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

    public static class SaveLoadManager {

        public static void CreateDefaultFile() {
            PlayerSettings playerSettings = new PlayerSettings();
            playerSettings.graphicContentEnabled = true;
            playerSettings.requestedMovie = "assorted";
            playerSettings.firstLaunch = false;
            Save(playerSettings);
        }

        public static void Save(object saveFile) {
            var bf = new BinaryFormatter();
            var fs = File.Create($"{Application.persistentDataPath}/LWFSave.sav");

            try {
                bf.Serialize(fs, saveFile);
            } catch (Exception ex) {
                Debug.LogWarning($"Unable to save '{Application.persistentDataPath}/LWFSave.sav': {ex.Message}");
            } finally {
                fs.Close();
            }
        }

        public static void Save(object saveFile, string fileName) {
            var bf = new BinaryFormatter();
            var fs = File.Create($"{Application.persistentDataPath}/{fileName}");

            try {
                bf.Serialize(fs, saveFile);
            } catch (Exception ex) {
                Debug.LogWarning($"Unable to save '{Application.persistentDataPath}/{fileName}'. {ex.Message}");
            } finally {
                fs.Close();
            }
        }

        public static void Save(object saveFile, string path, string fileName) {
            var bf = new BinaryFormatter();
            var fs = File.Create(path + fileName);

            try {
                bf.Serialize(fs, saveFile);
            } catch (Exception ex) {
                Debug.LogWarning($"Unable to save '{path}/{fileName}'. {ex.Message}");
            } finally {
                fs.Close();
            }
        }

        public static T Load<T>() {
            var bf = new BinaryFormatter();

            // Check if a file exists yet. If not, create a default then load its settings
            if (File.Exists($"{Application.persistentDataPath}/LWFSave.sav")) {
                Debug.Log("Save file exists. Loading contents now");
            } else {
                Debug.LogWarning("Unable to find save file. We will create one with default parameters and load it for you");
                CreateDefaultFile();
            }

            var fs = File.Open($"{Application.persistentDataPath}/LWFSave.sav", FileMode.Open);
            T result = (T)bf.Deserialize(fs);
            fs.Close();
            return result;
        }

        public static T Load<T>(string fileName) {
            var bf = new BinaryFormatter();
            var fs = File.Open($"{Application.persistentDataPath}/{fileName}", FileMode.Open);
            T result = (T)bf.Deserialize(fs);
            fs.Close();
            return result;
        }

        public static T Load<T>(string path, string fileName) {
            var bf = new BinaryFormatter();
            var fs = File.Open(path + fileName, FileMode.Open);
            T result = (T)bf.Deserialize(fs);
            fs.Close();
            return result;
        }
    }
}