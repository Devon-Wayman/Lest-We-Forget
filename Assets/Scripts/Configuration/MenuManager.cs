// Author Devon Wayman
// Date Sept 21 2020
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to play manage minor functionality from within the main menu scene (not including object interactions)
/// </summary>
public class MenuManager : MonoBehaviour {


    [SerializeField] private AudioSource radioAudio = null;
    [SerializeField] private List<GameObject> graphicContentItems = new List<GameObject>();
    public int graphicContentCondition = 0;

    private void Awake() {
        GetGraphicContentObjects();
    }

    private void GetGraphicContentObjects() {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("GraphicContent")) {
            graphicContentItems.Add(obj);
        }

        graphicContentCondition = PlayerPrefs.GetInt("GraphicEnabled");

        if(graphicContentCondition != 0 || graphicContentCondition != 1) {
            graphicContentCondition = 1;
        }

        SwitchGraphicContent(graphicContentItems);
    }

    private void SwitchGraphicContent(List<GameObject> graphicContentItems) {
        if(graphicContentCondition == 1) {
            foreach(GameObject obj in graphicContentItems) {
                obj.SetActive(true);
            }
        } else if (graphicContentCondition == 0) {
            foreach(GameObject obj in graphicContentItems) {
                obj.SetActive(false);
            }
        }
    }
}
