// Author: Devon Wayman - January 2021 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWF.Interaction.LevelManagement;

/// <summary>
/// This is to be placed on the player's boat ONLY! Other boats containing NPCs will be using the Unity's ECS system
/// to improve stability and lower resources needed for them to function without creating mass amounts of overhead
/// </summary>
namespace LWF.DDay {
    public class PlayerLCVP : MonoBehaviour {

        private List<AudioSource> slowmoAudios = new List<AudioSource>(); // List of audio sources in scene. Used to create slow motion effect
        public Transform lcvpGate = null; // Gate gameobject transform
        private Transform stopPosition; // Position to stop the given boat at
        public bool AllowNPCActivation { get; private set; } = false; // Boolean to inform NPC manager if NPCs can be activated
        private bool doorDropping = false; // Determine if the door has started dropping
        private bool hasCalledToSlow = false; // Determine if we have begun to slow down the boat
        public float boatSpeed; // Speed to have boat travel
        public float stopDistance; // Distance to begin slowing down at

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Vector3 direction = transform.TransformDirection(Vector3.forward) * stopDistance; // Draw ray as long as stop distance. Used to figure out where boat will slow down and where to place stop transform
            Gizmos.DrawRay(transform.position, direction);
        }

        #region Boat Setup
        private void Awake() {
            CreateStopPoint();
            ObtainSlowMoSources();
        }
        private void ObtainSlowMoSources() {
            foreach (AudioSource aSource in FindObjectsOfType<AudioSource>()) {
                if (aSource.gameObject.tag == "SlowMo") {
                    slowmoAudios.Add(aSource);
                }
            }
        }
        private void CreateStopPoint() {
            GameObject g = new GameObject($"{gameObject.name}_StopPoint");
            g.transform.position = transform.forward * stopDistance;
            g.transform.rotation = transform.rotation;
            g.tag = $"{gameObject.name}_StopPoint";
            stopPosition = g.transform;
        }
        #endregion

        void Update() {
            if (doorDropping) return;

            transform.localPosition += transform.forward * Time.deltaTime * boatSpeed; // Move boat forward

            // Slow boat down once the stopPosition has been reached or surpassed
            if (transform.localPosition.z >= stopPosition.transform.position.z && !hasCalledToSlow) {
                hasCalledToSlow = true; // Ensure the coroutine can not be called again in further updates
                StartCoroutine(SlowVessel(boatSpeed, 0f, 5f)); // Bring vessel speed to 0 in 5 seconds
            }
        }

        // Begin slowing the boat down
        private IEnumerator SlowVessel(float originalSpeed, float desiredSpeed, float stopDelay) {
            for (float t = 0f; t < stopDelay; t += Time.deltaTime) {
                boatSpeed = Mathf.Lerp(originalSpeed, desiredSpeed, t / stopDelay);
                yield return null;
            }
            boatSpeed = desiredSpeed; // Ensure boat speed is exactly at 0 before exiting function
            yield return new WaitForSeconds(1); // Wait 1 second before beginning to drop the ramp
            StartCoroutine(LowerRamp());
        }

        private IEnumerator LowerRamp() {
            doorDropping = true;

            while (lcvpGate.localRotation.x <= 0) {
                lcvpGate.Rotate(+15f * Time.deltaTime, 0, 0);
                yield return null;
            }

            // Start slow motion effect
            while (Time.timeScale >= 0.4f) {
                Time.timeScale -= 0.7f * Time.deltaTime;

                if (slowmoAudios.Count >= 1) {
                    for (int i = 0; i < slowmoAudios.Count; i++) {
                        slowmoAudios[i].pitch = Time.timeScale;
                    }
                }
                yield return null;
            }

            LevelChanger.Instance.GetComponent<Animator>().speed = 4f; // Increase animation speed of level transition canvas to compensate for slow motion
            AllowNPCActivation = true;
            this.enabled = false;
        }
    }
}