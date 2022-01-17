// Author: Devon Wayman - December 2021
using LWF.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LWF {
    public static class Extensions {

        /// <summary>
        /// Find all objects of given type inside scene and return them to requesting class instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Makes a random selection from passed in List object
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RandomListSelection<T>(this IList<T> list) {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Fades canvas group to desired alpha value within given time (adding onLeanComplete support soon)
        /// </summary>
        /// <param name="canvas">Canvas group to change value of</param>
        /// <param name="desiredAlpha">Desired float value of canvas alpha to transition to</param>
        /// <param name="transitionTime">Amount of time in seconds to complete the fade in</param>
        /// /// <param name="onCompleteAction">Action to invoke from passed in object once fade is completed</param>
        public static void FadeCanvasGroup(this CanvasGroup canvas, float desiredAlpha, float transitionTime) {
            LeanTween.alphaCanvas(canvas, desiredAlpha, transitionTime);
        }
        /// <summary>
        /// Fades canvas group over given time and calls a function once the fade has completed
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="desiredAlpha"></param>
        /// <param name="transitionTime"></param>
        /// <param name="callback"></param>
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

        /// <summary>
        /// Return a component that is already on an object or add it if it does not exist
        /// </summary>
        /// <param name="gameObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a save file in persistent path if one doesnt exist to load in
        /// </summary>
        public static void CreateDefaultFile() {
            PlayerSettings playerSettings = new PlayerSettings();
            playerSettings.graphicContentEnabled = true;
            playerSettings.requestedMovie = "assorted";
            playerSettings.firstLaunch = false;
            Save(playerSettings);
        }

        /// <summary>
        /// Serializes and saves a file in the persistent data path /GoatSave.g
        /// </summary>
        /// <param name="saveFile">The class to save</param>
        public static void Save(object saveFile) {
            var bf = new BinaryFormatter();
            var fs = File.Create(Application.persistentDataPath + "/LWFSave.sav");

            try {
                bf.Serialize(fs, saveFile);
            } catch (Exception ex) {
                Debug.LogWarning($"Unable to save '{Application.persistentDataPath}/LWFSave.sav'. {ex.Message}");
            } finally {
                fs.Close();
            }
        }

        /// <summary>
        /// Serializes and saves a file in the persistent data path with the given file name
        /// </summary>
        /// <param name="saveFile">The class to save</param>
        /// <param name="fileName">Name of the file including it's extension</param>
        public static void Save(object saveFile, string fileName) {
            var bf = new BinaryFormatter();
            var fs = File.Create(Application.persistentDataPath + "/" + fileName);

            try {
                bf.Serialize(fs, saveFile);
            } catch (Exception ex) {
                Debug.LogWarning($"Unable to save '{Application.persistentDataPath}/{fileName}'. {ex.Message}");
            } finally {
                fs.Close();
            }
        }

        /// <summary>
        /// Serializes and saves a file in the given path with the given file name
        /// </summary>
        /// <param name="saveFile">The class to save</param>
        /// <param name="path">Path were it should be saved</param>
        /// <param name="fileName">Name of the file</param>
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

        /// <summary>
        /// Loads, deserializes and returns the result. The file should be in the persistent data path /GoatSave.g
        /// </summary>
        /// <typeparam name="T">Type of the saved data</typeparam>
        /// <returns>The saved data</returns>
        public static T Load<T>() {
            var bf = new BinaryFormatter();

            // Check if a file exists yet. If not, create a default then load its settings
            if (File.Exists(Application.persistentDataPath + "/LWFSave.sav")) {
                Debug.Log("Save file exists. Loading contents now");
            } else {
                Debug.LogWarning("Unable to find save file. We will create one with default parameters and load it for you");
                CreateDefaultFile();
            }

            var fs = File.Open(Application.persistentDataPath + "/LWFSave.sav", FileMode.Open);
            T result = (T)bf.Deserialize(fs);
            fs.Close();
            return result;
        }

        /// <summary>
        /// Loads a file from the persistent data path with the given file name, deserializes and returns the result.
        /// </summary>
        /// <typeparam name="T">Type of the saved data</typeparam>
        /// <param name="fileName">File name</param>
        /// <returns>The saved data</returns>
        public static T Load<T>(string fileName) {
            var bf = new BinaryFormatter();
            var fs = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);
            T result = (T)bf.Deserialize(fs);
            fs.Close();
            return result;
        }

        /// <summary>
        /// Loads a file from the given path with the given file name, deserializes and returns the result.
        /// </summary>
        /// <typeparam name="T">Type of the saved data</typeparam>
        /// <param name="path">File name</param>
        /// <param name="fileName">The saved data</param>
        /// <returns>The saved data</returns>
        public static T Load<T>(string path, string fileName) {
            var bf = new BinaryFormatter();
            var fs = File.Open(path + fileName, FileMode.Open);
            T result = (T)bf.Deserialize(fs);
            fs.Close();
            return result;
        }
    }
}





