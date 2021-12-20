using UnityEngine;

/// <summary>
/// Allows player to save changes to game, selections to load for various levels from menu and more to a serialized file
/// Also calls static function to load the settings and apply them as needed
/// </summary>

namespace LWF.Settings {
    public class PlayerSaveLoadManager : DevSingleton<PlayerSaveLoadManager> {

        public PlayerSettings playerSettings { get; private set; }

        void Awake() {
            LoadSettings();
        }

        void LoadSettings() {
            playerSettings = SaveLoadManager.Load<PlayerSettings>();
            Debug.Log($"Continuous turn enabled: {playerSettings.continuousTurn}\nGraphic content enabled: {playerSettings.graphicContentEnabled} \nRequested movie: {playerSettings.requestedMovie}");
        }
    }
}
