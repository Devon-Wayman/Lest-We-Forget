using LWF.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildLoader : DevSingleton<BuildLoader> {

    public bool didWatchIntroVid { get; private set; } = false;

    private void Awake() {
        if (!Application.isEditor) {
            LoadPersistent();
        }
    }


    public void ChangeFirstLoad(bool didWatch) {
        didWatchIntroVid = didWatch;
    }

    private void LoadPersistent() {
        SceneManager.LoadSceneAsync((int)SceneIndexes.MainMenu, LoadSceneMode.Additive);
    }
}
