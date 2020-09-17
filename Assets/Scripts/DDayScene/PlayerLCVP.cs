// Copright Devon Wayman 2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WWIIVR.Interaction;

/// <summary>
/// This is to be placed on the player's boat ONLY! Other boats containing NPCs will be using the Unity's ECS system
/// to improve stability and lower resources needed for them to function without creating mass amounts of overhead
/// </summary>
namespace WWIIVR.DDay {
    public class PlayerLCVP : MonoBehaviour {

        private LevelChanger levelChanger; // LevelChanger reference
        private bool doorDropping = false; // Determine if the door has started dropping
        private bool hasCalledToSlow = false; // Determine if we have begun to slow down the boat

        // USING ACCESSOR SO THAT VALUE CAN BE MODIFIED IN THIS CLASS BUT ONLY READ BY OTHERS
        public bool AllowNPCActivation { get; private set; } = false; // Boolean to inform NPC manager if NPCs can be activated

        private Transform stopPosition; // Position to stop the given boat at
        private List<AudioSource> audioSources; // List of audio sources in scene. Used to create slow motion effect
        public float stopDistance; // Distance to begin slowing down at
        public float boatSpeed; // Speed to have boat travel

        [SerializeField] private Transform lcvpGate = null; // Gate gameobject transform

        void OnDrawGizmosSelected () {
            Gizmos.color = Color.red;
            Vector3 direction = transform.TransformDirection (Vector3.forward) * stopDistance; // Draw ray as long as stop distance. Used to figure out where boat will slow down and where to place stop transform
            Gizmos.DrawRay (transform.position, direction);
        }

        #region Boat Setup
        private void Awake () {
            levelChanger = FindObjectOfType<LevelChanger> ();
            GetComponentInChildren<Camera>().farClipPlane = 900; // Set further far plane distance so beach is visible

            // Create boat stop point based on distance entered
            CreateStopPoint();

            // Grab all audio sources in the scene
            ObtainAudioSources();
        }
        private void ObtainAudioSources() {
            foreach (AudioSource aSource in FindObjectsOfType<AudioSource>())
                audioSources.Add(aSource);
        }
        private void CreateStopPoint() {
            GameObject g = new GameObject($"{gameObject.name}_StopPoint");
            g.transform.position = transform.forward * stopDistance;
            g.transform.rotation = transform.rotation;
            g.tag = $"{gameObject.name}_StopPoint";
            stopPosition = g.transform;
        }
        #endregion


        void Update () {
            // Exit function if speed is 0 and doorDropping has been set to true
            if (boatSpeed == 0 && doorDropping)
                return;

            transform.localPosition += transform.forward * Time.deltaTime * boatSpeed; // Move boat forward

            // Slow boat down once the stopPosition has been reached or surpassed
            if (transform.localPosition.z >= stopPosition.transform.position.z && !hasCalledToSlow) {
                Debug.Log("Slowing LCVP", this);
                hasCalledToSlow = true; // Ensure the coroutine can not be called again in further updates
                StartCoroutine (SlowVessel (boatSpeed, 0f, 5f)); // Bring vessel speed to 0 in 5 seconds
            }
        }

        // Begin slowing the boat down
        private IEnumerator SlowVessel (float originalSpeed, float desiredSpeed, float stopDelay) {
            for (float t = 0f; t < stopDelay; t += Time.deltaTime) {
                boatSpeed = Mathf.Lerp (originalSpeed, desiredSpeed, t / stopDelay);
                yield return null;
            }
            boatSpeed = desiredSpeed; // Ensure boat speed is exactly at 0 before exiting function
            yield return new WaitForSeconds(1); // Wait 1 second before beginning to drop the ramp
            StartCoroutine(LowerRamp());
        }

        private IEnumerator LowerRamp () {
            Debug.Log("Lowering ramp");

            doorDropping = true;

            while (lcvpGate.localRotation.x <= 0) {
                lcvpGate.Rotate (+15f * Time.deltaTime, 0, 0);
                yield return null;
            }

            /*
            Debug.Log("Activating slow motion");

            // Start slow motion effect
            while (Time.timeScale >= 0.4f) {
                if (audioSources.Count >= 1) {
                    for (int i = 0; i < audioSources.Count; i++)
                        audioSources[i].pitch = Time.timeScale;
                }
                Time.timeScale -= 0.7f * Time.deltaTime;
                yield return null;
            }
            levelChanger.GetComponent<Animator>().speed = 4f; // Increase animation speed of level transition canvas to compensate for slow motion
            */

            // Allow NPCs to begin leaving their boats
            AllowNPCActivation = true;
        }
    }
}