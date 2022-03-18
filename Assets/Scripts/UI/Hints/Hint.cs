using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour {
    [TextArea(3, 10)]
    public string text;

    private HintManager hintManager;
    
    private void Start() {
    }

    private void OnTriggerEnter2D(Collider2D col) {
        hintManager = FindObjectOfType<HintManager>();
        if (col.GetComponent<Player>() != null) hintManager.OpenHint(this);
    }

    private void OnTriggerExit2D(Collider2D col) {
        hintManager = FindObjectOfType<HintManager>();
        if (col.GetComponent<Player>() != null) hintManager.CloseHint();
    }
}
