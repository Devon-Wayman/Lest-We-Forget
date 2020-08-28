using PathCreation;
using PathCreation.Examples;
using UnityEngine;

namespace WWIIVR.PlaneScene {
    public class PlaneAI : MonoBehaviour {
        [SerializeField] private float planeSpeed = 5f; // Desired start speed
        [SerializeField] private PathFollower pathFollowerObject = null; // PathFollower connected to plane (controls speed and sets curve to follow)
        [SerializeField] private Transform prop = null; // Plane prop object (transform)
        [SerializeField] private float propSpeed = 20f; // Prop rotation speed
        private float startSpeed;


        #region Initial Setup
        private void Start () {
            startSpeed = planeSpeed;

            if (pathFollowerObject == null) {
                GetPathObject ();
            }
        }
        private void OnEnable () {
            startSpeed = planeSpeed;

            if (pathFollowerObject == null) {
                GetPathObject ();
            }
        }
        // Get the path object in scene and set a reference for the plane to use
        private void GetPathObject () {
            if (TryGetComponent (out PathFollower pathFollower)) {
                pathFollowerObject = pathFollower; // Set this scripts reference of the PathFollower
                pathFollowerObject.pathCreator = GameObject.FindObjectOfType<PathCreator> (); // Find the path object in scene and set it for the pathFollower
                pathFollowerObject.speed = startSpeed; // Set travel speed on path follower to this scripts start speed
            }
        }
        #endregion


        private void Update () {
            prop.transform.Rotate (Vector3.back , 2 * (propSpeed * Time.deltaTime)); // Spin plane prop constantly
        }
    }
}