using System;
using System.Collections;
using System.Collections.Generic;
using Game.CustomKeybinds;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public string triggerSound = "";    // optional sound to play when interacting
    
    private Boolean inRange;
    private Transform indicator;
    void Start() {
        inRange = false;
        indicator = transform.GetChild(0);
        indicator.gameObject.SetActive(false);
    }
    
    void Update() {
        if (Input.GetKeyDown(Keybinds.GetInstance().interact)) {
            if (inRange) {
                Interact();
            }
        }
    }

    protected virtual void Interact() {
    }

    protected virtual void OnRangeEnter() {
        inRange = true;
        indicator.gameObject.SetActive(true);
    }

    protected virtual void OnRangeExit() {
        inRange = false;
        indicator.gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            OnRangeEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            OnRangeExit();
        }
    }

    protected void PlayInteractionSound() {
        if (triggerSound.Length != 0) FindObjectOfType<AudioManager>().Play(triggerSound); // play trigger sound
    }
}
