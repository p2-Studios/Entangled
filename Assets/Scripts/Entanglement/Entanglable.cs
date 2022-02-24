using System;
using System.Collections;
using System.Collections.Generic;
using Entanglement;
using UnityEngine;

/// <summary>
/// Entanglable type - represents objects that can be entangled and have forces applied to them.
/// Can be active, passive, or neither (not entangled)
/// </summary>
public class Entanglable : MonoBehaviour {
    // basic object physics data
    private Rigidbody2D rb;             // rigidbody
    //public Transform isGroundedChecker; // to check if the object is grounded
    public Transform respawnLocation;   // location that the object should respawn at when necessary
    public float checkGroundRadius = 0.05f;     // radius around the bottom of the object to check for the ground
    public LayerMask groundLayer;       // the layer type of the ground

    // object states
    public bool respawnable = true;     // object respawns after being destroyed, true by default
    public float respawnTime = 3.0f;    // how long it should take the object to respawn after being destroyed, 3.0s by default
    private bool destroyed;             // object is currently destroyed
    private bool respawning;            // object is currently respawning

    // entanglement states
    private bool entangled, active, passive;
    
    // force data
    private Vector2 velocity;                   // list of queued forces
    public float maxVelocity = 20.0f;
    private Boolean velocityUpdate;

    // VelocityManager instance
    private VelocityManager velocityManager;
    

    void Start() {
        rb = GetComponent<Rigidbody2D>();       // get the Rigidbody2D component of the object

        entangled = passive = active 
            = destroyed = respawning = false;   // new object is not entangled, destroyed, or respawning

        velocity = Vector2.zero;
        velocityUpdate = false;

        velocityManager = VelocityManager.GetInstance();

    }


    void Update() {
        if (velocityUpdate) {       // mirror active object velocity
            Vector2 vel = rb.velocity;
            if (vel.x == 0) velocity.x += vel.x;
            if (vel.y == 0) velocity.y += vel.y;
            rb.velocity = velocity;
            velocityUpdate = false; // unflag velocity change
        }

        /*
        // if velocity > max velocity, set velocity to max velocity
        if (rb.velocity.magnitude > maxVelocity) {
            rb.velocity = Vector2.zero;
        }*/
        
        if (active) {   // if active and moving, mirror velocity
            Vector2 vel = rb.velocity;
            if (!(vel.Equals(Vector2.zero))) {
                velocityManager.ActiveMoved(this, vel * rb.mass);
            }
        }

        if (active)
            GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(255f/255f, 136f/255f, 220f/255f));
        if (passive)
            GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(250f/255f, 255f/255f, 127f/255f));
        if (!entangled)
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white);

    }
    
    /// <summary>
    /// Modifies entanglement states of the object
    /// </summary>
    /// <param name="isActive"> (bool) Is it active?</param>
    /// <param name="isPassive"> (bool) Is it passive?</param>
    public void SetEntanglementStates(bool isActive, bool isPassive) {
        active = isActive;
        passive = isPassive;
        entangled = active || passive;
    }

    /// <summary>
    /// Apply a velocity to the entanglable.
    /// applied to the entanglable on the next frame. 
    /// </summary>
    /// <param name="vel">A Vector2 of the velocity to mirror.</param>
    public void ApplyVelocity(Vector2 vel) {
        velocity = vel / rb.mass;
        velocityUpdate = true;
    }

    /// <summary>
    /// Returns whether this object is entangled.
    /// </summary>
    /// <returns>true if the object is entangled.</returns>
    public bool IsEntangled() {
        return entangled;
    }

    /// <summary>
    /// Returns whether this object is entangled as an active object.
    /// </summary>
    /// <returns>true if object is entangled and is an active object.</returns>
    public bool IsActive() {
        return (entangled && active);
    }

    /// <summary>
    /// Returns whether this object is entangled as a passive object.
    /// </summary>
    /// <returns>true if object is entangled and is a passive object.</returns>
    public bool IsPassive() {
        return (entangled && passive);
    }

    /// <summary>
    /// Destroys the object (currently, destroyed = set as inactive)
    /// </summary>
    public void Destroy() {
        destroyed = true;                                    // flag as destroyed
        gameObject.SetActive(false);                         // disable the object
        if (respawnable) Invoke(nameof (Respawn), respawnTime);     // respawn after respawnTime delay
    }

    /// <summary>
    /// Respawns the object and moves it to the position of the respawnLocation transform
    /// </summary>
    void Respawn() {
        destroyed = false;
        gameObject.SetActive(true);                                         // re-enable the object
        gameObject.transform.position = respawnLocation.transform.position; // move the object to respawnLocation
    }
    
    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Platform")) {    // object on platform
                transform.parent = col.gameObject.transform;      // set parent to platform so object doesn't slide
        }
    }
    
    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Platform")) {    // object leaving platform
            transform.parent = null;
        }
    }

    // from https://gamedev.stackexchange.com/questions/174710/unity2d-force-of-an-impact
    /// <summary>
    /// Calculate the total impulse force from a 2D collision
    /// </summary>
    /// <param name="collision"></param>
    /// <returns></returns>
    static Vector2 ComputeTotalImpulse(Collision2D collision) {
        Vector2 impulse = Vector2.zero;

        int contactCount = collision.contactCount;
        for(int i = 0; i < contactCount; i++) {
            var contact = collision.GetContact(0);
            impulse += contact.normal * contact.normalImpulse;
            impulse.x += contact.tangentImpulse * contact.normal.y;
            impulse.y -= contact.tangentImpulse * contact.normal.x;
        }

        return impulse;
    }
}
