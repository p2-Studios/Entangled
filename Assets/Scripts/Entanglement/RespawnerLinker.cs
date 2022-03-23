using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnerLinker : MonoBehaviour {
    void Awake() {
        ArrayList objects = new ArrayList();
        Transform respawner = transform;
        foreach (Transform child in transform) {
            if (child.gameObject.name.Equals("ObjectRespawner")) {
                respawner = child;
            } else {
                Entanglable e = child.GetComponentInChildren<Entanglable>();
                if (e != null) {
                    objects.Add(e);
                }
            }
        }

        foreach (Entanglable e in objects) {
            e.respawnLocation = respawner;
        }
    }
}
