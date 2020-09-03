// Author: Devon Wayman
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WWIIVR.Interfaces;

// Trigger explosions next to LCVPs during DDay scene
namespace WWIIVR.DDay {
    public class TriggerExplosion : MonoBehaviour {

        #region Explosion Object Pools
        public Dictionary<string, Queue<GameObject>> explosionDictionary;
        [System.Serializable]
        public class Pool {
            public string tag;
            public GameObject prefab;
            public int maxQuantity; // Amount of reusable objects to allow
        }
        public List<Pool> pools;
        #endregion

        private bool allowExplosions; // Enable/disable water explosion spawning
        private PlayerLCVP playerBoat = null; // Reference to player's LCVP
        public Vector3[] explosionOffsets; // Offsets to spawn explosions from in reference to boats current location

        public void Awake() {
            allowExplosions = true; // Set to true at start
            playerBoat = GetComponent<PlayerLCVP>(); // Grab the playerLCVP class off this object
        }

        private void Start() {
            // Setup explosion object dictionary
            explosionDictionary = new Dictionary<string, Queue<GameObject>>();
            foreach (Pool pool in pools) {
                Queue<GameObject> bombPool = new Queue<GameObject>();
                for (int i = 0; i < pool.maxQuantity; i++) {
                    GameObject bomb = Instantiate(pool.prefab); // Spawn max allowed of bomb prefabs
                    bomb.SetActive(false); // Set newly created prefab active to false
                    bombPool.Enqueue(bomb); // Add object to end of queue
                }
                explosionDictionary.Add(pool.tag, bombPool);
            }
            StartCoroutine(DropBombs());
        }

        private IEnumerator DropBombs() {
            string[] explosionsAvailable = {"BigExplosion", "BigExplosion2"}; 

            while (allowExplosions) {
                int waitTime = Random.Range(4, 8); // Wait before dropping a bomb
                int selectedExplosionLocation = Random.Range(0, explosionOffsets.Length); // Select explosion location    
                int selectRandomExplosion = Random.Range(0, explosionsAvailable.Length); // Select explosion type

                try {
                    SpawnBombFromPool(explosionsAvailable[selectRandomExplosion], explosionOffsets[selectedExplosionLocation]); // Spawn bomb in designated location
                } catch (IOException ex) {
                    Debug.LogError($"Error spawning explosion: {ex.Message}");
                }

                // Exit loop if player boat speed is less than 9 on next check
                if (playerBoat.boatSpeed <= 9) {
                    allowExplosions = false;
                    Debug.Log("Exiting DropBombs");
                    break; // Discontinue this coroutine
                } 
                yield return new WaitForSeconds(waitTime); // Wait generated delay time from above
            }
        }
        // Spawn explosion from pool into the scene
        private GameObject SpawnBombFromPool(string tag, Vector3 position) {
            if (!explosionDictionary.ContainsKey(tag)) {
                Debug.LogWarning($"Pool with tag {tag} does not exist");
                return null;
            }

            Vector3 boatCurrentPosition = gameObject.transform.localPosition; // Save boats current position

            GameObject explosionToSpawn = explosionDictionary[tag].Dequeue(); // Pull out first element in queue
            explosionToSpawn.SetActive(true); // Activate the object
            explosionToSpawn.transform.position = new Vector3 (boatCurrentPosition.x + position.x, 0, boatCurrentPosition.z + position.z); // Set spawned explosion location to passed in Vector3 + boat's current position

            IPooledObject pooledObj = explosionToSpawn.GetComponent<IPooledObject>(); // Try to get rid of this somehow for better performance
            if (pooledObj != null)
                pooledObj.OnObjectSpawn();

            explosionDictionary[tag].Enqueue(explosionToSpawn); // Add explosion back into dictionary
            return explosionToSpawn;
        }
    }
}
