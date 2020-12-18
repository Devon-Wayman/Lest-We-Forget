// Author Devon Wayman
// Date 12/16/2020
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit.Examples;

/// <summary>
/// Used to apply player game settings from the settings view on user's desktop
/// Only enabled on PC builds; not available on android
/// </summary>
public class SettingsManager : MonoBehaviour {

    public Button movementMode = null;
    public Button turnMode = null;
    public Toggle postProcessToggle = null;
    public Button applyButton = null;

    private Text movementModeText = null;
    private Text turnModeText = null;

    public GameSettings gameSettings;

    [SerializeField] private LocomotionSchemeManager locomotionSchemeManager = null;


    private void Awake() {
        if (Application.platform == RuntimePlatform.Android) {
            Debug.Log("Player running on Android. Disabling setting view");
            this.gameObject.SetActive(false);
            return;
        }

        gameSettings = new GameSettings();
        locomotionSchemeManager = FindObjectOfType<LocomotionSchemeManager>();


        movementModeText = movementMode.GetComponentInChildren<Text>();
        turnModeText = turnMode.GetComponentInChildren<Text>();

        movementMode.onClick.AddListener(delegate { OnMovementModeChanged(); });
        turnMode.onClick.AddListener(delegate { OnTurnModeChanged(); });
        postProcessToggle.onValueChanged.AddListener(delegate { OnPostProcessingChanged(); });
        applyButton.onClick.AddListener(delegate { OnApplyClicked(); });

        // Check if settings file exists
        if (File.Exists(Application.persistentDataPath + "/wwiisettings.json") == true) {
            Debug.Log("Game settings file exists...");
            LoadSettings();
        }
        // If no file exists, set some default values, save to file and apply
        else if (File.Exists(Application.persistentDataPath + "/wwiisettings.json") == false) {
            Debug.Log("No game settings file found! Generating default values...");
            CreateDefaultValues();
        }
    }

    private void CreateDefaultValues() {
        gameSettings.snapTurnMovement = true;
        gameSettings.postProcessEnabled = true;
        gameSettings.teleportMovement = true;
        SaveSettings();
    }


    private void OnPostProcessingChanged() {
        gameSettings.postProcessEnabled = postProcessToggle.isOn;
    }
    private void OnTurnModeChanged() {
        gameSettings.snapTurnMovement = !gameSettings.snapTurnMovement;

        if (gameSettings.snapTurnMovement) {
            turnModeText.text = "Snap";
        } else {
            turnModeText.text = "Continuous";
        }
    }
    private void OnMovementModeChanged() {
        gameSettings.teleportMovement = !gameSettings.teleportMovement;

        if (gameSettings.teleportMovement) {
            movementModeText.text = "Teleport";
        } else {
            movementModeText.text = "Continuous";
        }
    }
  

    private void OnApplyClicked() {
        SaveSettings();
    }
    private void SaveSettings() {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/wwiisettings.json", jsonData);

#if UNITY_EDITOR
        Debug.Log(jsonData);
#endif
        // Reread settings from file to ensure everything saved properly
        LoadSettings();
    }
    private void LoadSettings() {
        Debug.Log($"Loading game settings from {Application.persistentDataPath}/wwiisettings.json");
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/wwiisettings.json"));

        ApplySettingsToGame(gameSettings);
    }

    // Runs when new settings are applied as well as on Awake call during load
    // if any  saved changes need to be applied
    private void ApplySettingsToGame(GameSettings gameSettings) {
        if (gameSettings.teleportMovement) {
            movementModeText.text = "Teleport";

            // TODO: Set LocomotionSchemeManager settings accordingly
            locomotionSchemeManager.moveScheme = LocomotionSchemeManager.MoveScheme.Noncontinuous;
        } else if(!gameSettings.teleportMovement) {
            movementModeText.text = "Continuous";

            // TODO: Set LocomotionSchemeManager settings accordingly
            locomotionSchemeManager.moveScheme = LocomotionSchemeManager.MoveScheme.Continuous;
        }

        if (gameSettings.snapTurnMovement) {
            turnModeText.text = "Snap";

            // TODO: Set LocomotionSchemeManager settings accordingly
            locomotionSchemeManager.turnStyle = LocomotionSchemeManager.TurnStyle.Snap;
        } else if (!gameSettings.snapTurnMovement) {
            turnModeText.text = "Contnuous";

            // TODO: Set LocomotionSchemeManager settings accordingly
            locomotionSchemeManager.turnStyle = LocomotionSchemeManager.TurnStyle.Continuous;
        }

        if (gameSettings.postProcessEnabled) {
            Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
            postProcessToggle.isOn = true;
        } else if (!gameSettings.postProcessEnabled) {
            Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = false;
            postProcessToggle.isOn = false;
        }
    }
}
