// Author: Devon Wayman - January 2021 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LWF.DDay {
    public class DDaySceneController : MonoBehaviour {

        List<AudioSource> sceneAudioSources; // Audio sources in scene. Needed to change pitch during slow mo sequence
        public bool AllowNPCActivation { get; private set; } = false; // Boolean to inform NPC manager if NPCs can be activated

        [Header("Slideshow Control Items")]
        [SerializeField] CanvasGroup slideshowCanvas;
        [SerializeField] TMPro.TMP_Text slideText;

        void Awake() {
            sceneAudioSources = Extensions.FindAllInScene<AudioSource>();
        }

        // Call from Cinemachine timeline to flash a slide in front of player (renders in front of everything else!)
        public static void DisplaySlide() {

        }

    }
}