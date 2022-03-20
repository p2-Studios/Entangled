using System;
using System.Collections;
using System.Collections.Generic;
using Activation_System;
using UnityEngine;
using Activator = Activation_System.Activator;

namespace Environment {
    public class AirFan : Activatable {
        public float power = 5.0f; // the power of the fan pushing objects upward
        public float range = 3.0f;
        
        public Activator[] activators;			// -- array of activators, REQUIRED to set the activators manually! --

        private ArrayList inRange;           // array of gameobjects within the fan's range
        
        private BoxCollider2D airCollider;
        private Transform airVisualTransform;
        private GameObject fanBase, airVisual;
        private Animator fanBaseAnimator, airVisualAnimator;

        private float fanBaseAnimatorSpeed, airVisualAnimatorSpeed;
        
        /// <summary>
        /// initialize fan objects and settings
        /// </summary>
        void Start() {
            airCollider = GetComponent<BoxCollider2D>();                 // collider component
            
            fanBase = transform.GetChild(0).gameObject;                  // fan base components
            fanBaseAnimator = fanBase.GetComponent<Animator>();
            fanBaseAnimatorSpeed = fanBaseAnimator.speed;

            airVisual = transform.GetChild(1).gameObject;                // air visual components
            airVisualTransform = airVisual.GetComponent<Transform>();
            airVisualAnimator = transform.GetChild(1).GetChild(0).GetComponent<Animator>(); 
            airVisualAnimatorSpeed = power / 2;                          // animation speed based on power 

            RescaleAirCollider();                                        // set air collider scale to match range

            // add manually set activators
            foreach (Activator a in activators) {
                AddActivator(a);
            }

            inRange = new ArrayList();

            if (activateByDefault) { Activate(); } else { Deactivate();}
        }

        /// <summary>
        /// Activates the fan, enabling the wind animation and fan turning
        /// </summary>
        public override void Activate() {
            base.Activate();            // call the parent activate function to handle general activation stuff
            fanBaseAnimator.speed = fanBaseAnimatorSpeed;
            airVisualAnimator.speed = airVisualAnimatorSpeed;
            airVisual.SetActive(true);  // enable visual effect

            // nudge any objects that are on the fan, so they trigger the collider events
            foreach (GameObject obj in inRange) {
                Rigidbody2D rb = obj.GetComponent <Rigidbody2D>();
                if (!rb.Equals(null)) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 0.1f);
            }
        }

        /// <summary>
        /// Deactivates the fan, disabling the wind animation and fan turning
        /// </summary>
        public override void Deactivate() {
            base.Deactivate();          // call the parent deactivate function to handle general deactivation stuff
            fanBaseAnimator.speed = 0;  // stop fan animation
            airVisual.SetActive(false); // disable visual effect
            inRange.Clear();            // clear all in range objects
        }

        public override void ToggleState() {
            if (IsActivated()) Deactivate();
            else Activate();
        }

        /// <summary>
        /// Rescales the collidable area of the fan depending on the current power
        /// </summary>
        private void RescaleAirCollider() {
            airCollider.size = new Vector2(airCollider.size.x, range);
            airCollider.offset = new Vector2(airCollider.offset.x, (range / 2));
            RescaleAirVisual();         // rescale the visual animation to match the new size
        }
        
        /// <summary>
        /// Rescales the air visual sprite based on the range of the fan
        /// </summary>
        private void RescaleAirVisual() {
            // rescale the visual (temporarily unlinks from parent to avoid scaling parent)
            Transform oldParent = airVisualTransform.parent;
            airVisualTransform.parent = null;
            Vector3 scale = airVisualTransform.localScale;
            airVisualTransform.localScale = new Vector3(scale.x, range / 2, scale.z);
            airVisualTransform.SetParent(oldParent);
            // move the visual
        }
        
        // if the fan is pointing straight upward (no rotation), then apply a force equal to the object's y velocity when
        // it leaves the trigger area. This makes it stay at the top of the fan's reach without bouncing a few times 
        // before coming to an equilibrium
        private void OnTriggerExit2D(Collider2D other) {
            Debug.Log("exiting");
            inRange.Remove(other.gameObject);
            if (!IsActivated()) return;
            if (!transform.rotation.Equals(Quaternion.identity)) return;    // only when fan is vertical
            Rigidbody2D rb = other.transform.GetComponent<Rigidbody2D>();
            if (rb.Equals(null)) return;
            ApplyFanVelocity(other, rb,transform.up * (rb.velocity.y * -1));
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        
        // apply a force equal to the object's mass when it enters the collider
        // this balances the force of gravity and keeps it floating on top of the fan
        private void OnTriggerEnter2D(Collider2D other) {
            inRange.Add(other.gameObject);
            if (!IsActivated()) return;
            Rigidbody2D rb = other.transform.GetComponent<Rigidbody2D>();
            if (rb.Equals(null)) return;
            ApplyFanVelocity(other, rb,(transform.up * rb.mass));
        }

        // while the object is in the fan collider, apply a force to raise it
        private void OnTriggerStay2D(Collider2D other) {
            Debug.Log(other.name);
            if (!IsActivated()) return;
            Rigidbody2D rb = other.transform.GetComponent<Rigidbody2D>();
            if (rb == null) return;
            ApplyFanVelocity(other, rb,(transform.up * power));
        }

        private void ApplyFanVelocity(Collider2D other, Rigidbody2D rb, Vector2 velocity) {
            Entanglable e = other.gameObject.GetComponent<Entanglable>();
            if (e != null) {    // entanglable
                e.ApplyVelocity(velocity, false);
            } else {            // non-entanglable (including player)
                rb.velocity = velocity;
            }
        }

        public float GetPower() {
            return power;
        }

        public void SetPower(float p) {
            power = p;
            airVisualAnimatorSpeed = power / 2; 
        }
        
    }
}
