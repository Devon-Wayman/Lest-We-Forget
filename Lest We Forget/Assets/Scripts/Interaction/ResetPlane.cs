// Author: Devon Wayman - March 2022
using UnityEngine;

namespace LWF.Managers {

    /// <summary>
    /// Resets the player in the currently loaded scene if they should collide with the reset plane
    /// Instances of this objects usage are mainly in the event that the player clips through a collider 
    /// and beings to fall infinitly
    /// </summary>
    public class ResetPlane : MonoBehaviour {
        private void OnCollisionEnter(Collision collision) {
            Debug.Log("Resetting scene");
            GameManager.ResetScene();
        }
    }
}
