// Author Devon Wayman 
// Date Sept 22 2020
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using LWF.Interaction.LevelManagement;
using LWF.Interaction.Player;

/// <summary>
/// Creates a grab offset for player hand to remove the "snap" that usually occurs
/// When we release the item, it will float back to its original location
/// Also hides menu title card when the object is selected and makes it reappear when we let go
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

    // Canvas containing text above item
    public GameObject titleCard = null;

    // Rigidbody on interactable object
    private Rigidbody rigidBody = null;

    protected override void Awake() {
        base.Awake();
        startPosition = gameObject.transform.position;
        startRotation = gameObject.transform.rotation;

        // Only get canvas item if level object is visible
        TryGetComponent<LevelObject>(out var exists);
        if (!exists) return;
        titleCard = transform.GetChild(0).gameObject;
        titleCard.SetActive(false);

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        Invoke("SetDefaultLocPos", 2);
    }
    private void SetDefaultLocPos() {
        startPosition = gameObject.transform.position;
        startRotation = gameObject.transform.rotation;
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        Debug.Log("Position and rotation set");
    }

    // When Activate button is pressed on controller
    //protected override void OnActivate(XRBaseInteractor interactor) {
    //    base.OnActivate(interactor);

    //    if (TryGetComponent(out LevelObject levelObject)) {
    //        // Prevent menu button requests or other controller inputs from
    //        // interferring with the current scene load
    //        GameController.changingScenes = true;

    //        levelObject.LoadLevel();
    //    }
    //}

    // Enable/disable titlecard when hovering on item
    //protected override void OnHoverEnter(XRBaseInteractor interactor) {
    //    base.OnHoverEnter(interactor);

    //    if (titleCard != null)
    //        titleCard.SetActive(true);
    //}
    //protected override void OnHoverExit(XRBaseInteractor interactor) {
    //    base.OnHoverExit(interactor);

    //    if (titleCard != null)
    //        titleCard.SetActive(false);
    //}

    // DEVON WAYMAN: NEED TO REWRITE ALL THESE SYSTEMS TO USE THE EVENT SYSTEM FOR NEW XR BUILD
    //protected override void OnSelectEnter(XRBaseInteractor interactor) {
    //    base.OnSelectEnter(interactor);
    //    StoreInteractor(interactor);
    //    MatchAttachmentPoints(interactor);

    //    if (titleCard != null) {
    //        if (titleCard.activeSelf)
    //            titleCard.SetActive(false);
    //    }
    //}
    private void StoreInteractor(XRBaseInteractor interactor) {
        interactorPosition = interactor.attachTransform.localPosition;
        interactorRotation = interactor.attachTransform.localRotation;
    }
    private void MatchAttachmentPoints(XRBaseInteractor interactor) {
        bool hasAttach = attachTransform != null;
        interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
        interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
    }

    #region On Player Hand Exit
    //protected override void OnSelectExit(XRBaseInteractor interactor) {
    //    base.OnSelectExit(interactor);
    //    ResetAttachmentPoiints(interactor);
    //    ClearInteractor(interactor);
    //    StartCoroutine(SmoothReturn(gameObject.transform.position, startPosition, startRotation));
    //}
    private void ResetAttachmentPoiints(XRBaseInteractor interactor) {
        interactor.attachTransform.localPosition = interactorPosition;
        interactor.attachTransform.localRotation = interactorRotation;
    }
    private void ClearInteractor(XRBaseInteractor interactor) {
        interactorPosition = Vector3.zero;
        interactorRotation = Quaternion.identity;
    }

    private IEnumerator SmoothReturn(Vector3 currentPosition, Vector3 targetPosition, Quaternion targetRotation) {

        float startTime = Time.time;
        while (Time.time < startTime + returnDelay) {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, (Time.time - startTime) / returnDelay);
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }
    #endregion
}
