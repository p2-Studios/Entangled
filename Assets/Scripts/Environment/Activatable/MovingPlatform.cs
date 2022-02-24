using System;
using System.Collections;
using System.Collections.Generic;
using Activation_System;
using UnityEngine;
using UnityEngine.UI;
using Activator = Activation_System.Activator;

public class MovingPlatform : Activatable {
    public Transform posStart, posEnd, startPos;        // positions

    public float speed;                                 // speed
    public Boolean stopAtEnd;                           // whether the platform should stop upon reaching posEnd, 
                                                        // or move back and forth
    private Vector2 nextPos;
    
    public Activator[] activators;			// -- array of activators, REQUIRED to set the activators manually! --
    
    void Start() {
        nextPos = startPos.position;
        
        foreach (Activator a in activators) {
            AddActivator(a);
        }
        
        
        activated = activateByDefault;
    }

    public override void Deactivate() {
        base.Deactivate();
        if (stopAtEnd) { // if stopAtEnd, move back to start on deactivate
            nextPos = posStart.position;
        }
    }

    public override void Activate() {
        base.Activate();
        if (stopAtEnd) { // if stopAtEnd and moving back to posStart, send back to posEnd
            if (nextPos == (Vector2) posStart.position) nextPos = posEnd.position;
        }
    }

    void Update() {
        // if at one of the end positions, switch next position
        if (activated) {    // only change nextPos while active
            if (transform.position == posStart.position) {
                nextPos = posEnd.position;                   // start going to posEnd
            } else if (transform.position == posEnd.position) {
                if (!stopAtEnd) nextPos = posStart.position; // only go back to posStart if stopAtEnd is false
            }
        }

        // move towards the next position, only move if activated or moving back to start
        if (activated || stopAtEnd) {
            transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
    }

    // draw lines between start and end in inspector
    private void OnDrawGizmos() {
        Gizmos.DrawLine(posStart.position, posEnd.position);
    }
}
