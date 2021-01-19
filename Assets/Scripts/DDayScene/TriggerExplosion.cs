// Author: Devon Wayman
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LWF.Interfaces;

/// <summary>
/// Sets up and triggers various water explosions for player's entry into Omaha beach. Implements object
/// pooling for better resource management and less frequent GC.
/// </summary>
namespace LWF.DDay {
    public class TriggerExplosion : MonoBehaviour {

        public Dictionary<string, Queue<GameObject>> explosionDictionary;
        private PlayerLCVP playerBoat = null; // Reference to player's LCVP
        [System.Serializable]
        public class Pool {
            public string tag;
            public GameObject prefab;
            public int maxQuantity; // Amount of reusable objects to allow
        }
        public List<Pool> pools;
        public Vector2 explosionXPlacements;
        public Vector2 explosionZPlacements;
        public Vector2 explosionLocation;
        private bool allowExplosions; // Enable/disable water explosion spawning

        private void Awake() {
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
            string[] explosionsAvailable = { "BigExplosion", "BigExplosion2" };

            while (allowExplosions) {
                int waitTime = Random.Range(4, 8); // Wait before dropping a bomb
                explosionLocation = GetRandomExplosionLocation();
                int selectRandomExplosion = Random.Range(0, explosionsAvailable.Length); // Select explosion type

                try {
                    SpawnBombFromPool(explosionsAvailable[selectRandomExplosion], new Vector2(explosionLocation.x, explosionLocation.y)); // Spawn bomb in designated location
                } catch (IOException ex) {
                    Debug.LogError($"Explosion error: {ex}");
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

        // Generarate random explosion position using given restraints
        private Vector2 GetRandomExplosionLocation() {
            int invertX = Random.Range(0, 1);
            explosionLocation = new Vector2(Random.Range(explosionXPlacements.x, explosionXPlacements.y), Random.Range(explosionZPlacements.x, explosionZPlacements.y));

            if (invertX == 1) {
                explosionLocation.x = explosionLocation.x * -1;
            }
            return explosionLocation;
        }

        // Spawn explosion from pool into the scene
        private GameObject SpawnBombFromPool(string tag, Vector2 position) {
            if (!explosionDictionary.ContainsKey(tag)) {
                Debug.LogWarning($"Pool with tag {tag} does not exist");
                return null;
            }

            Vector3 boatCurrentPosition = gameObject.transform.localPosition; // Save boats current position
            GameObject explosionToSpawn = explosionDictionary[tag].Dequeue(); // Pull out first element in queue

            explosionToSpawn.SetActive(true); // Activate the object
            explosionToSpawn.transform.position = new Vector3(boatCurrentPosition.x + position.x, 0, boatCurrentPosition.z + position.y); // Set spawned explosion location to passed in Vector3 + boat's current position

            IPooledObject pooledObj = explosionToSpawn.GetComponent<IPooledObject>(); // Try to get rid of this somehow for better performance
            if (pooledObj != null)
                pooledObj.OnObjectSpawn();

            explosionDictionary[tag].Enqueue(explosionToSpawn); // Add explosion back into dictionary
            return explosionToSpawn;
        }
    }
}
