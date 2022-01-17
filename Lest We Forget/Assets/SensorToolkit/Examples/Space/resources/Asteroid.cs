using UnityEngine;

namespace SensorToolkit.Example {
    [RequireComponent(typeof(Rigidbody))]
    public class Asteroid : MonoBehaviour {

        public float MaxRandomSpin;
        public float MaxRandomForce;
        public float BoundaryRadius;
        public float ReturnForce;
        public float ReturnForceLerpDistance;

        [SerializeField] private Rigidbody rb;

        void Start() {
            if (rb == null) return;

            rb.AddTorque(randomVector() * Random.Range(0f, MaxRandomSpin));
            rb.AddForce(randomVector() * Random.Range(0f, MaxRandomForce));
        }

        void Update() {
            if (rb == null) return;


            var distFromOrigin = transform.position.magnitude;
            if (distFromOrigin >= BoundaryRadius) {
                var f = Mathf.Lerp(0f, ReturnForce, (distFromOrigin - BoundaryRadius) / ReturnForceLerpDistance);
                rb.AddForce(f * -transform.position.normalized);
            }
        }

        Vector3 randomVector() {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
    }
}