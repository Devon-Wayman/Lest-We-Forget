﻿// Author: Devon Wayman - September 2020
using System;
using UnityEngine;

namespace LWF.Interaction.LevelManagement {
    public class LevelObject : MonoBehaviour {

        public enum SceneType { SimpleScene, Vehicle, Film };
        public SceneType sceneType;
        public string sceneName;
        public string vehicleScene;
        public string requestedVehicle;
        public string movieName;

        public void LoadLevel() {
            switch (sceneType) {
                case SceneType.SimpleScene:
                    if (sceneName == null || sceneName == String.Empty) {
                        Debug.Log("Scene name not given. Returning");
                        return;
                    }
                    LevelChanger.Current.FadeToLevel(sceneName);
                    break;

                case SceneType.Vehicle:
                    if (requestedVehicle == null || requestedVehicle == String.Empty || vehicleScene == null || vehicleScene == String.Empty) {
                        Debug.Log("Vehicle missing values. Returining");
                        return;
                    }

                    if (vehicleScene == "plane") {
                        PlayerPrefs.SetString("requestedPlane", requestedVehicle);
                    }

                    LevelChanger.Current.FadeToLevel(vehicleScene);
                    break;

                case SceneType.Film:
                    if (movieName == null || movieName == String.Empty) {
                        Debug.Log("No movie name entered for film reel. Returning.");
                        return;
                    }
                    PlayerPrefs.SetString("Movie", movieName);
                    LevelChanger.Current.FadeToLevel("Theater");
                    break;
            }
        }
    }
}