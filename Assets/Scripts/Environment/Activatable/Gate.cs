using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Activation_System {
    public class Gate : Activatable {
	    public bool stayOpen = false;			// whether the gate should stay open after being opened once

		public bool reverseOpen = false;

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
				Open();
			}
		}
		
		bool isOpened() {
			return active;
		}
		
		bool isClosed() {
			return !active;
		}
		
		public override void Activate() {
			base.Activate();
			Open();
		}

        public override void Deactivate()
        {
            base.Deactivate();
			Close();
        }

        void Open() {
			active = true;
			// gate.enabled = false;
			gateAnimator.SetBool("reverse", reverseOpen);
			gateAnimator.SetBool("open", true);
		}
		
		void Close() {
			active = false;
			// gate.enabled = true;
			gateAnimator.SetBool("reverse", reverseOpen);
			gateAnimator.SetBool("open", false);
		}
    }
}