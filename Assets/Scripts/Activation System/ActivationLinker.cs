using System.Collections;
using System.Collections.Generic;
using Activation_System;
using UnityEngine;

// loops through all child objects, and links activators to activatables
public class ActivationLinker : MonoBehaviour {

    private ArrayList activators, activatables;
    
    void Awake() {
        activators = new ArrayList();
        activatables = new ArrayList();
        foreach (Transform child in transform) {
            Activator activator = child.GetComponentInChildren<Activator>();
            if (activator != null) {
                activators.Add(activator);
            } else {
                Activatable activatable = child.GetComponentInChildren<Activatable>();
                if (activatable != null) {
                    activatables.Add(activatable);
                }
            }
        }

        // link each activator and activatable
        foreach (Activator activator in activators) {
            foreach (Activatable activatable in activatables) {
                activator.AddActivatable(activatable);
                activatable.AddActivator(activator);
            }
        }
    }

    public void GetActivationObjects() {
        activators = new ArrayList();
        activatables = new ArrayList();
        foreach (Transform child in transform) {
            Activator activator = child.GetComponentInChildren<Activator>();
            if (activator != null) {
                activators.Add(activator);
            } else {
                Activatable activatable = child.GetComponentInChildren<Activatable>();
                if (activatable != null) {
                    activatables.Add(activatable);
                }
            }
        }
    }
    
    // draw lines between activator and its activatables
    private void OnDrawGizmos() {
        GetActivationObjects();
        print(activators);
        foreach (Activator av in activators) {
            foreach (Activatable ac in activatables) {
                Gizmos.DrawLine(av.transform.position, ac.transform.position);
            }
        }
    }

}
