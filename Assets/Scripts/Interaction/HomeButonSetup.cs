using System;
// Author: Devon Wayman - June 2021
using UnityEngine;

/// <summary>
/// Used to set the home button in a custom location relative to the player's head location on scene start
/// </summary>
namespace WWIIVR {
    public class HomeButonSetup : MonoBehaviour {

        [SerializeField] private Transform playerHead = null;
        [SerializeField] private Transform playerFloor = null;
        [Tooltip("Whether or not to override the position to floor level (0) in front of player")] [SerializeField] private bool inFrontOnFloor;

        private void Start() {
            if (!inFrontOnFloor) return;

            float floorDistance = Vector3.Distance(playerHead.position, playerFloor.position);
            gameObject.transform.localPosition = new Vector3(playerHead.transform.position.x, playerHead.transform.position.y - floorDistance, playerFloor.transform.position.z + 0.03f);
            gameObject.transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }
}
