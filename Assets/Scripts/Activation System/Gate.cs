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
		private float timer = 1.0f;
		
		// Gate collider
		BoxCollider2D gate;
		
		// Boolean state
		private bool active = false;
		
		void Start() {
			
			gateAnimator = GetComponent<Animator>();
			// animationSpeed = gateAnimator.speed;
			
			gate = GetComponent<BoxCollider2D>();
			
			foreach (Activator a in activators) {
				AddActivator(a);
			}
			
		}
		
		bool isOpened() {
			return !gate.enabled;
		}
		
		bool isClosed() {
			return gate.enabled;
		}
		
		void setOpen() {
			active = true;
			gate.enabled = false;
			
			gateAnimator.SetBool("open",true);
			gateAnimator.SetBool("toggleAnim", true);
			
		}
		
		void setClose() {
			active = false;
			gate.enabled = true;
			
			gateAnimator.SetBool("open",false);
			gateAnimator.SetBool("toggleAnim", true);
			
		}

		void FixedUpdate() {							// NOTE: OnTrigger event is timed on FixedUpdate
			if (!active && IsActivated()) {
				setOpen();
			}
			else if (active && !IsActivated()) {
				setClose();
			}
			
			if (gateAnimator.GetBool("toggleAnim")) {
				timer -= Time.deltaTime;
				if (timer <= 0) {
					gateAnimator.SetBool("toggleAnim",false);
					timer = 1.0f;
				}
			}
			
		}

    }
}