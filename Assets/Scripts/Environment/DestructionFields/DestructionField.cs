using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionField : MonoBehaviour {
    public bool destroyPlayer;
    public bool destroyObjects;


    private void OnTriggerEnter2D(Collider2D other) {
        Player p = other.GetComponent<Player>();
        Entanglable e = other.GetComponent<Entanglable>();

        if (p != null) {
            if (destroyPlayer) {
                p.Kill();
            } else {
                p.entangleComponent.ClearEntangled();
            }
        }
        
        if (e != null && destroyObjects) {
            e.Kill();
        }
    }
}
