using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Activation_System
{
    public class PressurePad : Activator
    {
        public float requiredMass = 1.0f;								// required mass to trigger the pressure pad
		
		Dictionary<Collider2D, float> obj_mass;							// store object and gather masses
		Dictionary<Collider2D, ArrayList> obj_stacked;					// store the object being stacked
		
		public Activatable[] activatables;								// -- array of activatables, REQUIRED to set the activatables manually! --
		
		public String[] triggerers;
		
		private BoxCollider2D padCollider;
		
		private Animator pressurePadAnimator;							// For animations
		private float animationSpeed = 0.0f;							// This has no use currently
		
		void Start() {
			padCollider = GetComponent<BoxCollider2D>();				// fetch collider
			
			obj_mass = new Dictionary<Collider2D,float>();
			
			obj_stacked = new Dictionary<Collider2D,ArrayList>();
			
			pressurePadAnimator = GetComponent<Animator>();
			
			foreach (Activatable a in activatables) {
				AddActivatable(a);
			}
			
		}
		
		void OnTriggerEnter2D(Collider2D col) {
			if (!canTrigger(col.gameObject.tag)) return;
			obj_mass.Add(col,col.gameObject.GetComponent<Rigidbody2D>().mass);				// The sum of all mass on the pressure pad
			
			obj_stacked.Add(col,null);
			
			float sumOfMass = 0.0f;
			
			foreach (KeyValuePair<Collider2D,float> kvp in obj_mass) {
				sumOfMass += kvp.Value;
			}
			
			if (!IsActivated()) {
				if (requiredMass <= sumOfMass) {											// When the required mass target is reached
					 ToggleState(animationSpeed);
					 pressurePadAnimator.SetBool("active",true);
				}
			}
		}
		
		
		void OnTriggerStay2D(Collider2D col) {
			if (!canTrigger(col.gameObject.tag)) return;
			float sumOfMass = 0.0f;
			
			obj_stacked[col] = new ArrayList();
			
			get_objects(obj_stacked[col], col.gameObject);	
			
			foreach (GameObject go in obj_stacked[col]) {
				Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
				if (rb != null)
					sumOfMass += rb.mass;
			}
			
			obj_mass[col] = sumOfMass;
			
			foreach (KeyValuePair<Collider2D,float> kvp in obj_mass) {
				if (kvp.Key != col)
					sumOfMass += kvp.Value;
			}
			
			if (!IsActivated()) {
				if (requiredMass <= sumOfMass) {											// When the required mass target is reached
					 ToggleState(animationSpeed);
					 pressurePadAnimator.SetBool("active",true);
				}
			}
			else {
				if (requiredMass > sumOfMass) {												// When the required mass target is not reached
					ToggleState(animationSpeed);
					pressurePadAnimator.SetBool("active",false);
				}
			}
		}
		
		void OnTriggerExit2D(Collider2D col) {
			if (!canTrigger(col.gameObject.tag)) return;
			obj_mass.Remove(col);															// Remove from dictionary since no longer on pad
			obj_stacked.Remove(col);
			
			float sumOfMass = 0.0f;
			
			foreach (KeyValuePair<Collider2D,float> kvp in obj_mass) {
				sumOfMass += kvp.Value;
			}
			
			if (IsActivated()) {
				if (requiredMass > sumOfMass) {
					ToggleState(animationSpeed);
					pressurePadAnimator.SetBool("active",false);
				}
			}
		
		}
		
		// Used to fetch list of stacking Game Objects
		void get_objects(ArrayList obj_array, GameObject g) {
			foreach (KeyValuePair<Collider2D,ArrayList> kvp in obj_stacked) {
				if (kvp.Value != null && kvp.Value.Contains(g)) {				// Excludes any object already seen
					return;
				}
			}
			
			obj_array.Add(g);													// if hasn't been added before then add it
			
			Collider2D col = g.GetComponent<Collider2D>();
			
			if (col == null) {
				return;
			}
			else {
				ContactPoint2D[] contacts = new ContactPoint2D[25];				// The maximum contacts to search
				col.GetContacts(contacts);
				
				foreach (ContactPoint2D cp in contacts) {
					if (cp.collider != null) {
						if (cp.normal.y < -0.3f)
							get_objects(obj_array, cp.collider.gameObject);
					}
				}
			}
		}


		// checks if the given tag is in the list of tags that can trigger the pressure pad
		private Boolean canTrigger(String tag) {
			return triggerers.Contains(tag);
		}
    }
}