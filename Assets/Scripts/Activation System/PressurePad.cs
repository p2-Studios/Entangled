using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Activation_System
{
    public class PressurePad : Activator
    {
        public float requiredMass = 1.0f;								// required mass to trigger the pressure pad
		
		Dictionary<Collider2D, float> obj_mass;						// store object and gather masses

		public Activatable[] activatables;								// -- array of activatables, REQUIRED to set the activatables manually! --
		
		private BoxCollider2D padCollider;
		
		private Animator pressurePadAnimator;							// For animations
		private float animationSpeed = 0.0f;							// This has no use currently
		
		void Start() {
			padCollider = GetComponent<BoxCollider2D>();				// fetch collider
			
			obj_mass = new Dictionary<Collider2D,float>();
			
			pressurePadAnimator = GetComponent<Animator>();
			
			foreach (Activatable a in activatables) {
				AddActivatable(a);
			}
			
		}
		
		void OnTriggerEnter2D(Collider2D col) {
			obj_mass.Add(col,col.gameObject.GetComponent<Rigidbody2D>().mass);				// The sum of all mass on the pressure pad
			
			float sumOfMass = 0.0f;
			
			foreach (KeyValuePair<Collider2D,float> kvp in obj_mass) {
				sumOfMass += kvp.Value;
			}
			
			if (!IsActivated()) {
				if (requiredMass <= sumOfMass) {							// When the required mass target is reached
					 Activate(animationSpeed);
					 pressurePadAnimator.SetBool("active",true);
				}
			}
		}
		
		
		void OnTriggerStay2D(Collider2D col) {
			float sumOfMass = 0.0f;
			
			ArrayList array_obj = new ArrayList();
			
			get_objects(array_obj, col.gameObject);
			
			foreach (GameObject go in array_obj) {
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
				if (requiredMass <= sumOfMass) {							// When the required mass target is reached
					 Activate(animationSpeed);
					 pressurePadAnimator.SetBool("active",true);
				}
			}
			else {
				if (requiredMass > sumOfMass) {					// When the required mass target is not reached
					Deactivate(animationSpeed);
					pressurePadAnimator.SetBool("active",false);
				}
			}
		}
		
		void OnTriggerExit2D(Collider2D col) {
			obj_mass.Remove(col);
			
			float sumOfMass = 0.0f;
			
			foreach (KeyValuePair<Collider2D,float> kvp in obj_mass) {
				sumOfMass += kvp.Value;
			}
			
			if (IsActivated()) {
				if (requiredMass > sumOfMass) {
					Deactivate(animationSpeed);
					pressurePadAnimator.SetBool("active",false);
				}
			}
		
		}
		
		// Used to fetch list of stacking Game Objects
		void get_objects(ArrayList obj_array, GameObject g) {
			if (obj_array.Contains(g)) {
					return;															// Ignore objects already seen (though not needed) but for caution
			}
			else {
				obj_array.Add(g);
				
				Collider2D col = g.GetComponent<Collider2D>();
				
				if (col == null) {
					return;
				}
				else {
					ContactPoint2D[] contacts = new ContactPoint2D[25];				// The maximum contacts to search
					col.GetContacts(contacts);
					
					foreach (ContactPoint2D cp in contacts) {
						if (cp.collider != null) {
							if (cp.normal.y < 0.0f)
								get_objects(obj_array, cp.collider.gameObject);
						}
					}
				}
			}
		}
		
    }
}