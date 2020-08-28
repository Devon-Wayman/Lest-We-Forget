using System;
using UnityEngine;

/// <summary>
/// Sets up requested plane in Plane scene
/// </summary>
namespace WWIIVR.PlaneScene {    
    public class PlaneSetup : MonoBehaviour {
        GameObject plane; // Variable to store requested plane model in
        GameObject planePath; // Variable to store requested plane's path
        public string planeOverride;
        public string desiredPlaneValue;

        private void Awake () {
            
            if (planeOverride == String.Empty || planeOverride == null)
                desiredPlaneValue = PlayerPrefs.GetString ("DesiredPlane");
            else
                desiredPlaneValue = planeOverride;

            Debug.Log($"Requested plane: {desiredPlaneValue}");

            if (desiredPlaneValue == null || desiredPlaneValue == "") {
                Debug.LogError ("Error loading requested plane on Awake");
                return;
            }

            // Load the requested plane's path object
            planePath = (GameObject) Instantiate (Resources.Load<GameObject>($"Planes/{desiredPlaneValue}PATH"), new Vector3 (0, 0, 0), Quaternion.identity);

            // Load in desired plane model
            plane = (GameObject) Instantiate (Resources.Load<GameObject>($"Planes/{desiredPlaneValue}"), new Vector3 (0, 0, 0), Quaternion.identity);
        }
    }
}