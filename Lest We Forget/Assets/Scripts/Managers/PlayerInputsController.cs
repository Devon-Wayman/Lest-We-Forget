using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace LWF.Managers {
    public class PlayerInputsController : DevSingleton<PlayerInputsController> {

        [Header("Movement Systems")]
        [SerializeField] private ContinuousMoveProviderBase continuousMoveProvider;
        [SerializeField] private ContinuousTurnProviderBase continuousTurnProvider;


        public void SetSceneStartStates(bool allowMovement, bool allowTurning) {
            continuousMoveProvider.enabled = allowMovement;
            continuousTurnProvider.enabled = allowTurning;
        }

        public void ChangePlayerMovementPermission(bool state) {
            continuousMoveProvider.enabled = state;
        }

        public void ChangePlayerRotationPermission(bool state) {
            continuousTurnProvider.enabled = state;
        }
    }
}
