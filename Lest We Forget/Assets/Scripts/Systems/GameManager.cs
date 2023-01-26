using UnityEngine;

namespace LWF.Systems {
    [DefaultExecutionOrder(-1)]
    public static class GameManager {

        public static void QuitGame() {
            Debug.Log("Exiting application...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    public enum SceneIndexes {
        MainMenu = 1,
        VictimsList,
        Planes
    }
}
