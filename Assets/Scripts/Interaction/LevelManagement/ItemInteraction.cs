// Author Devon Wayman 
// Date Sept 22 2020
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Calls a function to have an object once held by player to float back to its original location
/// </summary>
public class ItemInteraction : XRGrabInteractable {

    // Amount of time to take to return to default location when player lets go of object
    private float returnDelay = 0.3f;

    // Object's initial rotation and position
    private Vector3 startPosition = Vector3.zero;
    private Quaternion startRotation = Quaternion.identity;

    // Used for grab offset input
    private Vector3 interactorPosition = Vector3.zero;
    private Quaternion interactorRotation = Quaternion.identity;


    protected override void Awake() {
        base.Awake();
        startPosition = gameObject.transform.position;
        startRotation = gameObject.transform.rotation;
    }


    protected override void OnSelectEnter(XRBaseInteractor interactor) {
        base.OnSelectEnter(interactor);
        StoreInteractor(interactor);
        MatchAttachmentPoints(interactor);
    }
    private void StoreInteractor(XRBaseInteractor interactor) {
        interactorPosition = interactor.attachTransform.localPosition;
        interactorRotation = interactor.attachTransform.localRotation;
    }
    private void MatchAttachmentPoints(XRBaseInteractor interactor) {
        bool hasAttach = attachTransform != null;
        interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
        interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
    }

    protected override void OnSelectExit(XRBaseInteractor interactor) {
        base.OnSelectExit(interactor);
        ResetAttachmentPoiints(interactor);
        ClearInteractor(interactor);
        StartCoroutine(SmoothReturn(gameObject.transform.position, startPosition, startRotation));
    }
    private void ResetAttachmentPoiints(XRBaseInteractor interactor) {
        interactor.attachTransform.localPosition = interactorPosition;
        interactor.attachTransform.localRotation = interactorRotation;
    }
    private void ClearInteractor(XRBaseInteractor interactor) {
        interactorPosition = Vector3.zero;
        interactorRotation = Quaternion.identity;
    }
    private IEnumerator SmoothReturn(Vector3 currentPosition, Vector3 targetPosition, Quaternion targetRotation) {
        // Disable collision detections
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        float startTime = Time.time;
        while (Time.time < startTime + returnDelay) {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, (Time.time - startTime) / returnDelay);
            yield return null;
        }

        // Detect collisions again
        gameObject.GetComponent<Rigidbody>().isKinematic = false;

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }
}
