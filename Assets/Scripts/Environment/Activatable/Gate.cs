using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Activation_System
{
    public class Gate : Activatable
    {
        public Activator[] activators;			// -- array of activators, REQUIRED to set the activators manually! --
		
		// Animation variables
		private Animator gateAnimator;
		private float timer = 0.1f;				// timer when gate moves
		
		// Gate collider
		BoxCollider2D gate;						// Blocks rigidbodies and other colliders from passing
		
		// Boolean state
		private bool active = false;			// check when the gate is activated or not
		private int state = 1;					// the current state of the gate, used for animators
		
		void Start() {
			
			gateAnimator = GetComponent<Animator>();
			// animationSpeed = gateAnimator.speed;
			
			gate = GetComponent<BoxCollider2D>();
			
			foreach (Activator a in activators) {
				AddActivator(a);
			}
		}
		
		bool isOpened() {
			return active;
		}
		
		bool isClosed() {
			return !active;
		}
		
		void setOpen() {
			active = true;
			gate.enabled = false;
		}
		
		void setClose() {
			active = false;
			gate.enabled = true;
		}

		void FixedUpdate() {							// NOTE: OnTrigger event is timed on FixedUpdate
			if (!active && IsActivated()) {
				setOpen();
			}
			else if (active && !IsActivated()) {
				setClose();
			}
			
			// Play's animation
			if (isOpened() && state != 11) {
				timer -= Time.deltaTime;
				if (timer <= 0) {
					state += 1;
					gateAnimator.SetInteger("state",state);
					timer = 0.1f;
				}
			}
			else if (isClosed() && state != 1) {
				timer -= Time.deltaTime;
				if (timer <= 0) {
					state -= 1;
					gateAnimator.SetInteger("state",state);
					timer = 0.1f;
				}
			}
			
		}

    }
}