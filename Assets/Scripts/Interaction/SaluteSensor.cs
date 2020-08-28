// Copyright Devon Wayman 2020
using UnityEngine;

/// <summary>
/// Used to detect if player does a Nazi salute. If so, take action.
/// </summary>
namespace WWIIVR.Interaction {
    public class SaluteSensor : MonoBehaviour {

        // Gameobjects
        [SerializeField] private Transform headObject = null;
        [SerializeField] private Transform leftHand = null;
        [SerializeField] private Transform rightHand = null;

        // Distance between left and right hand relative to head
        public float rightDistance;
        public float leftDistance;

        // Minimum distance between hand and head needed to check for salute
        public float distanceThreshold = 0.6f; 

        // Min and max angles for salute check
        [SerializeField] private float minAngle;
        [SerializeField] private float maxAngle;

        void FixedUpdate () {
            // Exit function if not right hand or head devices found
            if (headObject == null || leftHand == null || rightHand == null) return;

            rightDistance = Vector3.Distance(headObject.position, rightHand.position);
            leftDistance = Vector3.Distance(headObject.position, leftHand.position);

            Debug.Log ($"Distance from head to right hand: {rightDistance}\nDistance from head to left hand: {leftDistance}");

            if (rightDistance  >= distanceThreshold || leftDistance >= distanceThreshold){
                CheckAngles();
            }
        }

        // Check if each hand's angle relative ot face is within angle threshold
        private void CheckAngles() {
            
            if (WithinAngleRange()){
                    
            }
        }

        // Determine if either hand is in the angle range to 
        // execute a salute response
        private bool WithinAngleRange() {
            // Left hand angle
            Vector3 leftDir = leftHand.position - headObject.position;
            leftDir = leftHand.InverseTransformDirection(leftDir);
            float leftAngle = Mathf.Atan2(leftDir.y, leftDir.x) * Mathf.Rad2Deg;

            // Right hand angle
            Vector3 rightDir = leftHand.position - headObject.position;
            rightDir = rightHand.InverseTransformDirection(rightDir);
            float rightAngle = Mathf.Atan2(rightDir.y, rightDir.x) * Mathf.Rad2Deg;

            if ((leftAngle > minAngle && leftAngle < maxAngle) || (rightAngle > minAngle && rightAngle < maxAngle)){
                Debug.Log("SALUTE DETECTED!");
                return true;
            } else {
                Debug.Log("Salute not found");
                return false;
            }
        }
    }
}