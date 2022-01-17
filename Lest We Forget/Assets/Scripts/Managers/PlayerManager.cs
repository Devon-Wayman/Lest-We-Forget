using LWF.Settings;
using UnityEngine;


namespace LWF.Managers {
    public class PlayerManager : DevSingletonPersistent<PlayerManager> {

        public PlayerSettings playerSettings { get; private set; }

        protected override void Awake() {
            base.Awake();
            LoadSettings();
        }

        private void LoadSettings() {
            playerSettings = SaveLoadManager.Load<PlayerSettings>();
            Debug.Log($"Graphic content enabled: {playerSettings.graphicContentEnabled} \nRequested movie: {playerSettings.requestedMovie}");
        }

        private void OnApplicationQuit() {
            SaveLoadManager.Save(new PlayerSettings { firstLaunch = true }); ;
        }
    }
}
