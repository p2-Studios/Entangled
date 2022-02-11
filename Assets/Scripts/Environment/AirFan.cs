using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFan : MonoBehaviour {

    public float power = 0.25f; // the power of the fan pushing objects upward
    
    void Start() {
        
    }

    void Update() {
        
    }

    public float getPower() {
        return power;
    }

    public void setPower(float p) {
        power = p;
    }

    // if the fan is pointing straight upward (no rotation), then apply a force equal to the object's y velocity when
    // it leaves the trigger area. This makes it stay at the top of the fan's reach without bouncing a few times 
    // before coming to an equilibrium
    private void OnTriggerExit2D(Collider2D other) {
        if (transform.rotation.Equals(Quaternion.identity)) {
            Rigidbody2D rb = other.transform.GetComponent<Rigidbody2D>();
            if (!rb.Equals(null)) {
                rb.AddForce((transform.up * (rb.velocity.y * -1)), ForceMode2D.Impulse);
            }  
        }
    }

    // apply a force equal to the object's mass when it enters the collider
    // this balances the force of gravity and keeps it floating on top of the fan
    private void OnTriggerEnter2D(Collider2D other) {
        Rigidbody2D rb = other.transform.GetComponent<Rigidbody2D>();
        if (!rb.Equals(null)) {
            rb.AddForce((transform.up * rb.mass), ForceMode2D.Impulse);
        }
    }

    // while the object is in the fan collider, apply a force to raise it
    private void OnTriggerStay2D(Collider2D other) {
        Rigidbody2D rb = other.transform.GetComponent<Rigidbody2D>();
        if (!rb.Equals(null)) {
            rb.AddForce(transform.up * power, ForceMode2D.Impulse);
        }
    }
}
