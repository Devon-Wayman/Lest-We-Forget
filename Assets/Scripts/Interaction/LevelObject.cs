// Author Devon Wayman
// Date Sept 22 2020
using UnityEngine;
using WWIIVR.Interaction;

public class LevelObject : MonoBehaviour {
    public enum SceneType { SimpleScene, Vehicle, Film };
    public SceneType sceneType;

    // Simple scene load
    public string sceneName;

    // Vehicle load
    public string vehicleScene;
    public string requestedVehicle;

    // Film selection
    public string movieName;

    public void LoadLevel() {
        switch (sceneType) {
            
            // Simple scene load:
            case SceneType.SimpleScene:
                FindObjectOfType<LevelChanger>().FadeToLevel(sceneName);
                break;

            // Simple scene load:
            case SceneType.Vehicle:
                FindObjectOfType<LevelChanger>().FadeToLevel(sceneName);
                break;

            // Simple scene load:
            case SceneType.Film:
                PlayerPrefs.SetString("Movie", movieName);
                FindObjectOfType<LevelChanger>().FadeToLevel("Theater");
                break;
        }
    }
}
