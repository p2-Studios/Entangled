using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInteractable : MonoBehaviour {
    private GameObject controlIndicator;
    private GameObject grabIndicator;
    public GameObject indicators;
    private bool toggle;
    Quaternion iniRot;
    float iniY;
    // Start is called before the first frame update
    void Start() {
        toggle = false;
        
        controlIndicator = indicators.transform.GetChild(0).gameObject;
        grabIndicator = indicators.transform.GetChild(1).gameObject;
        controlIndicator.SetActive(false);
        grabIndicator.SetActive(false);
        
        indicators.gameObject.SetActive(toggle);
        
        iniRot = indicators.transform.rotation;
        iniY = indicators.transform.position.y - transform.position.y;
    }

    public void ToggleControlSprite(bool state) {
        indicators.SetActive(state);
        controlIndicator.SetActive(state);
    }

    public void ToggleGrabbingSprite(bool state) {
        indicators.SetActive(state);
        grabIndicator.SetActive(state);
        controlIndicator.SetActive(!state);
    }


    public void LateUpdate(){
        indicators.transform.rotation = iniRot;
        indicators.transform.position = new Vector2(transform.position.x,transform.position.y + iniY);
    }
}
