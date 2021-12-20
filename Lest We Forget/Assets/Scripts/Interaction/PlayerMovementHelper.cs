using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// TUTORIALS:
// Using sockets (grabbing items with both hands): https://www.youtube.com/watch?v=rRNvq09Itdw
// Interactables: https://www.youtube.com/watch?v=furfe8E7SOA
// Player upper body: https://www.youtube.com/watch?v=MYOjQICbd8I
// Player lower body: https://www.youtube.com/watch?v=1Xr3jB8ik1g
// Button interations: https://www.youtube.com/watch?v=HFNzVMi5MSQ
// UI Interaction: https://www.youtube.com/watch?v=BZt74PVb7sM

namespace LWF {
    public class PlayerMovementHelper : MonoBehaviour {

        [SerializeField] XRRig xRRig;
        [SerializeField] CharacterController characterController;
        [SerializeField] CharacterControllerDriver driver;



        private void Start() {

        }

        void Update() {
            UpdateCharacterController();
        }

        protected virtual void UpdateCharacterController() {
            if (xRRig == null || characterController == null) return;

            var height = Mathf.Clamp(xRRig.cameraInRigSpaceHeight, driver.minHeight, driver.maxHeight);

            Vector3 center = xRRig.cameraInRigSpacePos;
            center.y = height / 2f + characterController.skinWidth;

            characterController.height = height;
            characterController.center = center;
        }
    }
}
