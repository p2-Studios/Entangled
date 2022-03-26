using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Activation_System {
    public class Gate : Activatable {

	    public string openSound, closeSound;
	    public bool stayOpen = false;			// whether the gate should stay open after being opened once
	    
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

			if (activateByDefault) {
				StartCoroutine(openDelay());
			}
		}
		
		bool isOpened() {
			return active;
		}
		
		bool isClosed() {
			return !active;
		}
		
		void setOpen(bool sound) {
			active = true;
			gate.enabled = false;
			if (openSound.Length != 0 && sound) FindObjectOfType<AudioManager>().Play(openSound); // play opening sound
		}
		
		void setClose(bool sound) {
			active = false;
			gate.enabled = true;
			if (closeSound.Length != 0 && sound) FindObjectOfType<AudioManager>().Play(closeSound); // play closing sound
		}

		void FixedUpdate() {							// NOTE: OnTrigger event is timed on FixedUpdate
			if (!active && IsActivated()) {
				setOpen(true);
			}
			else if (active && !IsActivated()) {
				if (!stayOpen) setClose(true);
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

		private IEnumerator openDelay() {
			yield return new WaitForSeconds(0.1f);
			activated = true;
		}
    }
}