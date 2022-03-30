using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughOpacity : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    public Color opacityColor;
    private Color regularColor;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("BehindForeground")) {
            Debug.Log("entering");
            regularColor = spriteRenderer.color;
            spriteRenderer.color = opacityColor;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.CompareTag("BehindForeground")) {
            Debug.Log("exiting");
            spriteRenderer.color = regularColor;
        }
    }
}
