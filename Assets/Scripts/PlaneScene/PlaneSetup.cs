// Author: Devon Wayman - November 2020
using System;
using UnityEngine;

/// <summary>
/// Sets up requested plane in Plane scene
/// </summary>
namespace LWF.PlaneScene {
    public class PlaneSetup : MonoBehaviour {
        private GameObject plane = null; // Variable to store requested plane model in
        private GameObject planePath = null; // Variable to store requested plane's path

        public string planeOverride;
        public string desiredPlaneValue;

        private void Awake() {
            if (planeOverride == String.Empty || planeOverride == null || planeOverride == "")
                desiredPlaneValue = PlayerPrefs.GetString("DesiredPlane");
            else
                desiredPlaneValue = planeOverride;
        }

        private void Start() {
            SpawnPlaneAndPath();
        }

        //TOOD: Change this system to use a scriptable object to instead load the path and plane model needed by index number instead of loading all
        // planes at this directory and THEN choosing the correct one
        private void SpawnPlaneAndPath() {
            // Load the requested plane's path object
            planePath = (GameObject)Instantiate(Resources.Load<GameObject>($"Planes/{desiredPlaneValue}PATH"), new Vector3(0, 0, 0), Quaternion.identity);

            // Load in desired plane model
            plane = (GameObject)Instantiate(Resources.Load<GameObject>($"Planes/{desiredPlaneValue}"), new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}