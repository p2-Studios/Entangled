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

        if (p != null && destroyPlayer) {
            p.Kill();
        } else if (e != null && destroyObjects) {
            e.Kill();
        }
    }
}
