using System;
using System.Collections;
using System.Collections.Generic;
using Game.CustomKeybinds;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public string triggerSound = "";    // optional sound to play when interacting

    protected bool interactionEnabled = true;
    private Boolean inRange;
    protected Transform indicator;
    public bool showIndicator = true;
    void Start() {
        inRange = false;
        indicator = transform.GetChild(0);
        indicator.gameObject.SetActive(false);
    }
    
    void Update() {
        if (Input.GetKeyDown(Keybinds.GetInstance().interact)) {
            if (inRange && interactionEnabled) {
                Interact();
            }
        }
    }

    protected virtual void Interact() {
    }

    protected virtual void OnRangeEnter() {
        inRange = true;
        if (showIndicator) indicator.gameObject.SetActive(true);
    }

    protected virtual void OnRangeExit() {
        inRange = false;
        if (showIndicator) indicator.gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && interactionEnabled) {
            OnRangeEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            OnRangeExit();
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (interactionEnabled && !inRange) {
                OnRangeEnter();
            }
        }
    }

    protected void PlayInteractionSound() {
        if (triggerSound.Length != 0) FindObjectOfType<AudioManager>().Play(triggerSound); // play trigger sound
    }
}
