using UnityEditor;
using UnityEditor.SceneManagement;

public static class PlayFromScene {

    [MenuItem("LWF/Begin Play/Main Menu")]
    public static void PlayFromMainMenu() {
        OpenScene("MainMenu");
    }

    [MenuItem("LWF/Begin Play/Victims List")]
    public static void BeginPlayAtVictimsList() {
        OpenScene("VictimsList");
    }

    [MenuItem("LWF/Begin Play/DDay")]
    public static void BeginPlayAtDDay() {
        OpenScene("DDay");
    }

    [MenuItem("LWF/Begin Play/Shooting Range")]
    public static void BeginPlayAtShootingRange() {
        OpenScene("ShootingRange");
    }

    private static void OpenScene(string sceneName) {
        EditorSceneManager.OpenScene("Assets/Scenes/Persistent.unity", OpenSceneMode.Single);
        EditorSceneManager.OpenScene($"Assets/Scenes/{sceneName}/{sceneName}.unity", OpenSceneMode.Additive);
        EditorApplication.EnterPlaymode();
    }
}

public static class SwitchEditorToScene {

    [MenuItem("LWF/Switch To Scene/Main Menu")]
    public static void SwitchToMainMenuInEditor() {
        OpenScene("MainMenu");
    }

    [MenuItem("LWF/Switch To Scene/Victims List")]
    public static void SwitchToVictimsListInEditor() {
        OpenScene("VictimsList");
    }

    [MenuItem("LWF/Switch To Scene/DDay")]
    public static void SwitchToDDayInEditor() {
        OpenScene("DDay");
    }

    [MenuItem("LWF/Begin Play/Shooting Range")]
    public static void SwitchToShootingRangeInEditor() {
        OpenScene("ShootingRange");
    }

    private static void OpenScene(string sceneName) {
        EditorSceneManager.OpenScene("Assets/Scenes/Persistent.unity", OpenSceneMode.Single);
        EditorSceneManager.OpenScene($"Assets/Scenes/{sceneName}/{sceneName}.unity", OpenSceneMode.Additive);
    }
}
