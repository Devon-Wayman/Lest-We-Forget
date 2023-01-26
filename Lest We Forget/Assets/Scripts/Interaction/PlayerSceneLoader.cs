using LWF.Systems;
using UnityEngine;

public class PlayerSceneLoader : MonoBehaviour {

    public void LoadMemorial() {
        ScreenFade.Instance.LoadNewScene((int)SceneIndexes.VictimsList);
    }
}
