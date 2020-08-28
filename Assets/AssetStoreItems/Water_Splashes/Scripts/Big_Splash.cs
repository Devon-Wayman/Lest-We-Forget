// Copyright Devon Wayman 2020
using UnityEngine;
using System.Collections;
using WWIIVR.Interfaces;

/// <summary>
/// Run splash coroutine each time the attatched splash object is activated
/// </summary>
namespace WWIIVR.DDay {
    public class Big_Splash : MonoBehaviour, IPooledObject {

        public GameObject BigSplash;
        public AudioSource explosionAudio;

        public void OnObjectSpawn() {
            float pitchValue = Random.Range(0.6f, 1f);
            explosionAudio.pitch = pitchValue;
            StartCoroutine("TriggerSplash");
        }


        IEnumerator TriggerSplash() {
            BigSplash.SetActive(true);
            Debug.Log($"Activated splash on {gameObject.name}");
            yield return new WaitForSeconds(3.5f);
            BigSplash.SetActive(false);
        }
    }
}