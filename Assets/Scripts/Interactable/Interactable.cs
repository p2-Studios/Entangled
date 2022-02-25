using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    private Boolean inRange;
    private Transform indicator;
    void Start() {
        inRange = false;
        indicator = transform.GetChild(0);
        indicator.gameObject.SetActive(false);
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            if (inRange) {
                Interact();
            }
        }
    }

    protected virtual void Interact() {
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            inRange = true;
            indicator.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            inRange = false;
            indicator.gameObject.SetActive(false);
        }
    }
}
