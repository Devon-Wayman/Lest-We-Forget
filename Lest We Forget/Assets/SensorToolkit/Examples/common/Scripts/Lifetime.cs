using UnityEngine;

namespace SensorToolkit.Example {
    public class Lifetime : MonoBehaviour {

        public float MaxAge;

        float age;

        void OnEnable() {
            age = 0f;
        }

        void Update() {
            age += Time.deltaTime;

            if (age >= MaxAge) {
                Destroy(gameObject);
            }
        }
    }
}