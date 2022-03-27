using System;
using System.Collections;
using System.Collections.Generic;
using Entanglement;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Entanglable type - represents objects that can be entangled and have forces applied to them.
/// Can be active, passive, or neither (not entangled)
/// </summary>
public class Entanglable : MonoBehaviour, IDestroyable {
    #region Fields
    // basic object physics data
    protected Rigidbody2D rb;             // rigidbody
    //public Transform isGroundedChecker; // to check if the object is grounded
    public Transform respawnLocation;   // location that the object should respawn at when necessary

    // sprite stuff
    protected SpriteRenderer spriteRenderer;
    public Transform deathAnimation;
    [SerializeField] Material defaultMat, activeGlow, pasiveGlow, hoverGlow;

    // object states
    public float respawnDelay = 3.0f;    // how long it should take the object to respawn after being destroyed, 3.0s by default

    // entanglement states
    protected bool entangled, active, passive, destroyed;

    // force data
    protected Vector2 velocity;
    protected Vector3 position, previousPosition;
    public float maxVelocity = 20.0f;
    protected Boolean velocityUpdate;

    // VelocityManager instance
    protected VelocityManager velocityManager;
    #endregion

    void Start() {
        rb = GetComponent<Rigidbody2D>();       // get the Rigidbody2D component of the object

        entangled = passive = active = false;   // new object is not entangled, destroyed, or respawning

        velocityUpdate = false;

        velocityManager = VelocityManager.GetInstance();

        spriteRenderer = GetComponent<SpriteRenderer>();

        SetEntanglementStates(false, false, true);
        
    }

    private bool VelAboveThreshold(float f) {
        return (Math.Abs(f) > 0.3);
    }
    
    void FixedUpdate() {
        if (velocityUpdate) {       // mirror active object velocity
            float x = VelAboveThreshold(velocity.x) ? velocity.x : rb.velocity.x;
            float y = VelAboveThreshold(velocity.y) ? velocity.y : rb.velocity.y;
            rb.velocity = new Vector2(x, y);
            velocityUpdate = false; // unflag velocity change
            velocity = Vector2.zero;
        }

        if (active) {   // if active and moving, mirror velocity
            Vector2 vel = rb.velocity;
            if (!(vel.Equals(Vector2.zero))) {
                velocityManager.ActiveMoved(this, vel * rb.mass);
            } else { // check if the object has a parent object that's moving
                var parent = rb.transform.parent;
                if (!(parent == null)) {    // has a parent object, so calculate the object's absolute velocity
                    position = transform.position;
                    Vector3 worldVelocity = (position - previousPosition) / Time.fixedDeltaTime;
                    previousPosition = position;
                    if (!worldVelocity.Equals(Vector2.zero) && worldVelocity.sqrMagnitude < maxVelocity) {    // if absolute velocity > 0, apply
                        velocityManager.ActiveMoved(this, worldVelocity * rb.mass);
                    }
                }
            }
        }
    }
    
    #region Entanglement Methods
    /// <summary>
    /// Modifies entanglement states of the object
    /// </summary>
    /// <param name="isActive"> (bool) Is it active?</param>
    /// <param name="isPassive"> (bool) Is it passive?</param>
    public void SetEntanglementStates(bool isActive, bool isPassive, bool hover) {
        active = isActive;
        passive = isPassive;
        entangled = active || passive;

        if (entangled) {
            if (active) {
                spriteRenderer.material = activeGlow;
            } else {
                spriteRenderer.material = pasiveGlow;
            }
        } 
        else if(hover){
            spriteRenderer.material = hoverGlow;
        }
        else {
            spriteRenderer.material = defaultMat;
        }
    }

    public virtual void ApplyVelocity(Vector2 vel) {
        ApplyVelocity(vel, true);
    }
    
    /// <summary>
    /// Apply a velocity to the entanglable.
    /// applied to the entanglable on the next frame. 
    /// </summary>
    /// <param name="vel">A Vector2 of the velocity to mirror.</param>
    /// <param name="fromActive">Whether this velocity is being applied from an active object</param>
    public virtual void ApplyVelocity(Vector2 vel, bool fromActive) {
        velocity += (vel / rb.mass);
        velocityUpdate = true;
    }
    #endregion

    #region Getters/Setters
    
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
    
    public GameObject GetGameObject() {
        return gameObject;
    }

    public bool IsDestroyed() {
        return destroyed;
    }
    
    #endregion
    
    #region Collision Management
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Platform")) {    // object on platform
                MovingPlatform mp = col.gameObject.GetComponent<MovingPlatform>();
                if (mp != null && mp.makeObjectChild) transform.parent = col.gameObject.transform;      // set parent to platform so object doesn't slide
        } else if (col.gameObject.CompareTag("Destroyer") && !(col is PolygonCollider2D)) { // hacky solution to not having the box top trigger kill the box when it touches the push collider
            Kill();
        } 
    }
    
    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Platform")) {    // object leaving platform
            if (gameObject.activeSelf) {
                MovingPlatform mp = col.gameObject.GetComponent<MovingPlatform>();
                if (mp != null && mp.makeObjectChild) transform.parent = null;
            }
        }
    }
    #endregion

    #region Destructable Methods
    public void Kill() {
        DestructionManager dm = DestructionManager.instance;
        if (dm != null) dm.Destroy(this, respawnDelay);
    }

    // do things that need to be done on destroying, before the gameobject is set to inactive
    public void Destroy() {
        destroyed = true;
        Transform destructionAnimation = Instantiate(deathAnimation, transform.position, quaternion.identity);  // play destruction animation
        destructionAnimation.localScale = transform.localScale;
        GetComponent<FixedJoint2D>().enabled = false;   // disable object being pushed/pulled
        rb.velocity = Vector2.zero; // reset velocity
        EntangleComponent ec = FindObjectOfType<EntangleComponent>();
        if (ec != null) ec.Unentangle(this);
    }
    
    // do things that need to be done on respawning, right after the game object is set as active again
    public void Respawn() {
        destroyed = false;
        transform.parent = null;
        gameObject.transform.position = respawnLocation.transform.position; // move the object to respawnTransform
        rb.velocity = Vector2.zero; // reset velocity
    }
    #endregion Destructible Methods

}
