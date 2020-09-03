// Author: Devon Wayman
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

/// <summary>
/// Used to scroll text view with the left controller stick. Min and max y position values are clamped to prevent over scrolling. 
/// Text can be loaded from a resources file or stay at default text attatched to object
/// </summary>
namespace WWIIVR.Interaction {
    public class TextScroller : MonoBehaviour {

        private bool replaceText = false; // Determine if Text element text should be set to contents of a resource folder file
        
        private RectTransform thisRect;
        public string resourceFileName; // Name of file in resources (excluding extension)
        public bool CanScroll  { get; private set; } = false; // Determine if the control stick should move the text or nots
        public float maxScrollUp, maxScrollDown;
        public float scrollSpeed; // Speed at which text scrolls

        // VR OBJECTS
        private List<InputDevice> devices = new List<InputDevice> ();
        private InputDevice device;
        [SerializeField] private XRNode xrNode = XRNode.LeftHand;

        private void OnEnable () {
            if (!device.isValid)
                GetDevice ();

            CanScroll = true;
            thisRect = gameObject.GetComponent<RectTransform> ();

            if (replaceText) {
                var textFile = Resources.Load<TextAsset> (resourceFileName); // Load victims text file
                string rawText = textFile.ToString ();
                this.GetComponent<Text> ().text = rawText;
            } else {
                Debug.Log ("Keeping default text");
            }
        }
        private void GetDevice () {
            InputDevices.GetDevicesAtXRNode (xrNode, devices); // get left hand device XR node
        }

        private void OnDisable () {
            this.CanScroll = false; // Disable scroll input on this object when disabled
        }

        void Update () {
            if (!CanScroll || !device.isValid)
                return;

            Vector2 primary2DAxisValue = Vector2.zero;
            InputFeatureUsage<Vector2> primary2DAxis = CommonUsages.primary2DAxis;

            if (device.TryGetFeatureValue (primary2DAxis, out primary2DAxisValue) && primary2DAxisValue != Vector2.zero) {
                thisRect.transform.position += transform.up * Time.deltaTime * scrollSpeed * primary2DAxisValue.y;
            }
        }
    }
}