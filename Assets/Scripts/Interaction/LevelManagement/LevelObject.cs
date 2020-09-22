// Author Devon Wayman
// Date Sept 22 2020
using System;
using System.Collections;
using UnityEngine;

namespace WWIIVR.Interaction.LevelManagement {
    public class LevelObject : MonoBehaviour {

        public enum SceneType { SimpleScene, Vehicle, Film };
        public SceneType sceneType;

        // SimpleScene enum variables
        public string sceneName;

        // Vehicle enum variables
        public string vehicleScene;
        public string requestedVehicle;

        // Film enum variables
        public string movieName;

        /// <summary>
        /// Loads level if object has such capability
        /// </summary>
        public void LoadLevel() {
            switch (sceneType) {
                // Simple scene load:
                case SceneType.SimpleScene:
                    if (sceneName == null || sceneName == String.Empty) {
                        Debug.Log("Scene name not given. Returning");
                        return;
                    }

                    FindObjectOfType<LevelChanger>().FadeToLevel(sceneName);
                    break;

                // Simple scene load:
                case SceneType.Vehicle:
                    if (requestedVehicle == null || requestedVehicle == String.Empty || vehicleScene == null || vehicleScene == String.Empty) {
                        Debug.Log("Vehicle missing values. Returining");
                        return;
                    }

                    if (vehicleScene == "plane") {
                        PlayerPrefs.SetString("requestedPlane", requestedVehicle);
                    }

                    FindObjectOfType<LevelChanger>().FadeToLevel(vehicleScene);
                    break;

                // Simple scene load:
                case SceneType.Film:
                    if (movieName == null || movieName == String.Empty) {
                        Debug.Log("No movie name entered for film reel. Returning.");
                        return;
                    }
                    PlayerPrefs.SetString("Movie", movieName);
                    FindObjectOfType<LevelChanger>().FadeToLevel("Theater");
                    break;
            }
        }


    }
}