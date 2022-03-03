using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name + " entering.");
        if (other.gameObject.GetComponent<Entanglable>() != null) {
            other.gameObject.GetComponent<Entanglable>().Destroy();
        } else if (other.gameObject.GetComponent<Player>()) {
            other.gameObject.GetComponent<Player>().Kill();
        }
    }
}
