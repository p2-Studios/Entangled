using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Activation_System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using Activator = Activation_System.Activator;

public class MovingPlatform : Activatable {
    public Transform posStart, posEnd, startPos;        // positions
    public Light2D indicatorLight;

    public Sprite indicatorLoop, indicatorStop;
    public SpriteRenderer indicator;
    
    
    public float speed;                                 // speed
    public Boolean stopAtEnd;                           // whether the platform should stop upon reaching posEnd, 
                                                        // or move back and forth
    private Vector2 nextPos;

    private bool moving = true;
    private bool justActivated = false;                 // flag for whether the platform was just activated, to know whether the delay can be skipped
    public bool moveInstantly = true;
    public float startDelay = 1.5f;
    public float endDelay = 1.5f;
    public bool makeObjectChild = true;
    
    
    public Activator[] activators;			// -- array of activators, REQUIRED to set the activators manually! --
    
    private Color activatedTint;
    
    void Start() {
        nextPos = posStart.position;
        
        foreach (Activator a in activators) {
            AddActivator(a);
        }

        if (activateByDefault) {
            Activate();
        } else Deactivate();

        if (stopAtEnd) {
            indicator.sprite = indicatorStop;
        } else {
            indicator.sprite = indicatorLoop;
        }
    }

    public override void Deactivate() {
        base.Deactivate();
        indicatorLight.color = Color.red; // no colour when deactivated
        if (stopAtEnd) { // if stopAtEnd, move back to start on deactivate
            nextPos = posStart.position;
        }
    }

    public override void Activate() {
        base.Activate();
        indicatorLight.color =  Color.green;
        justActivated = true;
        if (stopAtEnd) { // if stopAtEnd and moving back to posStart, send back to posEnd
            if (nextPos == (Vector2) posStart.position) nextPos = posEnd.position;
        }
    }

    void FixedUpdate() {
        // if at one of the end positions, switch next position
        if (activated) {    // only change nextPos while active
            if (transform.position == posStart.position) {
                nextPos = posEnd.position;                   // start going to posEnd
                
                if (justActivated && moveInstantly) {
                    moving = true;
                } else {
                    if (moving) StartCoroutine(WaitAtDestination(startDelay));
                }
                
                //transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
                
            } else if (transform.position == posEnd.position) {
                if (!stopAtEnd) nextPos = posStart.position; // only go back to posStart if stopAtEnd is false
                
                if (justActivated && moveInstantly) {
                    moving = true;
                } else {
                    if (moving) StartCoroutine(WaitAtDestination(endDelay));
                }
            }
            justActivated = false;
        }

        // move towards the next position, only move if activated or moving back to start
        if ((activated || stopAtEnd) && moving) {
            transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
    }

    // draw lines between start and end in inspector
    private void OnDrawGizmos() {
        Gizmos.DrawLine(posStart.position, posEnd.position);
    }

    public IEnumerator WaitAtDestination(float time) {
        moving = false;
        yield return new WaitForSeconds(time);
        moving = true;
        transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    public bool DoesMakeObjectChild() {
        return makeObjectChild;
    }
}
