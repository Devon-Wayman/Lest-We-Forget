// Author: Devon Wayman
using UnityEngine;
using WWIIVR.Interaction.LevelManagement;

namespace WWIIVR.Interaction.Player {
    [RequireComponent(typeof(InputHandler))]
    public class GameController : MonoBehaviour {
        
        [SerializeField] private InputHandler inputHandler;

        public static bool changingScenes = false;

        private void Awake() {
            // Ensure changing scenes is false on awake when loading to a new scene
            changingScenes = false; 

            LevelChanger.Instance.GetComponent<Animator>().speed = 1f; // Make sure on start that the level changer animation speed is normal

            if (inputHandler == null)
                inputHandler = GetComponent<InputHandler>();
        }

        private void Update() {
            if (changingScenes) return;

            if (inputHandler.GetApplicationMenuDown()) {
                if (Time.timeScale != 1)
                    Time.timeScale = 1;
                LevelChanger.Instance.FadeToLevel("MainMenu");
                changingScenes = true;
            }
        }
    }
}