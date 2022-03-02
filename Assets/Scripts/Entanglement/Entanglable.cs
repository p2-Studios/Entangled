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
    protected Rigidbody2D rb;             // rigidbody
    //public Transform isGroundedChecker; // to check if the object is grounded
    public Transform respawnLocation;   // location that the object should respawn at when necessary
    public float checkGroundRadius = 0.05f;     // radius around the bottom of the object to check for the ground
    public LayerMask groundLayer;       // the layer type of the ground

    // sprite stuff
    public Sprite unentangledSprite, activeSprite, passiveSprite;
    protected SpriteRenderer spriteRenderer;
    
    // object states
    public bool respawnable = true;     // object respawns after being destroyed, true by default
    public float respawnTime = 3.0f;    // how long it should take the object to respawn after being destroyed, 3.0s by default
    protected bool destroyed;             // object is currently destroyed
    //private bool respawning;            // object is currently respawning

    // entanglement states
    protected bool entangled, active, passive;

    // force data
    protected Vector2 velocity;                   // list of queued forces
    protected Vector3 position, previousPosition;
    public float maxVelocity = 20.0f;
    protected Boolean velocityUpdate;

    // VelocityManager instance
    protected VelocityManager velocityManager;
    

    void Start() {
        rb = GetComponent<Rigidbody2D>();       // get the Rigidbody2D component of the object

        entangled = passive = active 
            = destroyed = false;   // new object is not entangled, destroyed, or respawning

        velocity = Vector2.zero;
        velocityUpdate = false;

        velocityManager = VelocityManager.GetInstance();

        spriteRenderer = GetComponent<SpriteRenderer>();

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
            } else { // check if the object has a parent object that's moving
                var parent = rb.transform.parent;
                if (!(parent == null)) {    // has a parent object, so calculate the object's absolute velocity
                    position = transform.position;
                    Vector3 worldVelocity = (position - previousPosition) / Time.deltaTime;
                    previousPosition = position;
                    if (!(worldVelocity.Equals(Vector2.zero))) {    // if absolute velocity > 0, apply
                        velocityManager.ActiveMoved(this, worldVelocity * rb.mass);
                    }
                }
            }
        }

        /*
        if (active)
            GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(255f/255f, 136f/255f, 220f/255f));
        if (passive)
            GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(250f/255f, 255f/255f, 127f/255f));
        if (!entangled)
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white);
        */
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

        if (entangled) {
            if (active) {
                spriteRenderer.sprite = activeSprite;
            } else {
                spriteRenderer.sprite = passiveSprite;
            }
        } else {
            spriteRenderer.sprite = unentangledSprite;
        }
    }

    /// <summary>
    /// Apply a velocity to the entanglable.
    /// applied to the entanglable on the next frame. 
    /// </summary>
    /// <param name="vel">A Vector2 of the velocity to mirror.</param>
    public virtual void ApplyVelocity(Vector2 vel) {
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
    
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Platform")) {    // object on platform
                transform.parent = col.gameObject.transform;      // set parent to platform so object doesn't slide
        }
    }
    
    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Platform")) {    // object leaving platform
            transform.parent = null;
        }
    }
    
    /// <summary>
    /// When the mouse hovers over an Entanglable, changes cursor texture (calls MouseHover class)
    /// </summary>
    public void OnMouseEnter() {
        MouseHover.instance.Clickable();
    }

    /// <summary>
    /// When the mouse leaves an Entanglable, changes cursor texture (calls MouseHover class)
    /// </summary>
    public void OnMouseExit() {
        MouseHover.instance.Default();
    }
}
