using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterMovementHelper : MonoBehaviour {

    [SerializeField] private XROrigin xrRig;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CharacterControllerDriver driver;

    private void FixedUpdate() {
        UpdateCharacterController();
    }

    protected virtual void UpdateCharacterController() {
        if (xrRig == null || characterController == null)
            return;

        var height = Mathf.Clamp(xrRig.CameraInOriginSpaceHeight, driver.minHeight, driver.maxHeight);

        Vector3 center = xrRig.CameraInOriginSpacePos;
        center.y = height / 2f + characterController.skinWidth;

        characterController.height = height;
        characterController.center = center;
    }
}
