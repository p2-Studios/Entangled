using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticLights : MonoBehaviour {
    public BoxCollider2D trigger;
    public bool delayFirst;
    public float onDelay = 0.5f;
    public ArrayList lights;
    private bool activated;
    void Awake() {  // link all activatables
        lights = new ArrayList();
        foreach (Transform child in transform) {
            LightToggle l = child.GetComponentInChildren<LightToggle>();
            if (l != null) {
                l.activateByDefault = false;    // make sure light is off by default
                lights.Add(l);
            }
        }
    }

    private IEnumerator switchLightsOn() {
        if (delayFirst) 
            yield return new WaitForSeconds(onDelay);
        foreach (LightToggle l in lights) {
            l.Activate();
            yield return new WaitForSeconds(onDelay);
        }

        activated = true;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && !activated) {
            StartCoroutine(switchLightsOn());
        }
    }
}
