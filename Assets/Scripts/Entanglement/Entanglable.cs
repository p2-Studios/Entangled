using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entanglable type - represents objects that can be entangled and have forces applied to them.
/// Can be active, passive, or neither (not entangled)
/// </summary>
public class Entanglable : MonoBehaviour
{
    // enum flags for entanglement mode (Active and Passive)
    enum entanglement
    {
        Active,
        Passive
    }

    // basic object physics data
    private Rigidbody2D rb;             // rigidbody
    public Transform isGroundedChecker; // to check if the object is grounded
    public Transform respawnLocation;   // location that the object should respawn at when necessary
    public float checkGroundRadius;     // radius around the bottom of the object to check for the ground
    public LayerMask groundLayer;       // the layer type of the ground

    // object states
    public bool respawnable = true;     // object respawns after being destroyed, true by default
    public float respawnTime = 3.0f;    // how long it should take the object to respawn after being destroyed, 3.0s by default
    private bool destroyed;             // object is currently destroyed
    private bool respawning;            // object is currently respawning

    // entanglement states
    private bool entangled, active, passive;

    // force data
    ArrayList forces;                   // list of queued forces


    void Start() {
        rb = GetComponent<Rigidbody2D>();       // get the Rigidbody2D component of the object

        entangled = passive = active 
            = destroyed = respawning = false;   // new object is not entangled, destroyed, or respawning

        forces = new ArrayList();               // object starts with no queued forces
        
    }

    void Update() {
        if (forces.Count != 0) {                        // apply each queued force
            foreach (Vector2 force in forces) {
                rb.AddForce(force, ForceMode2D.Impulse);
            }
            forces.Clear();                             // all forces applied, clear the queue
        }
        

        if (active)
            GetComponent<Renderer>().material.SetColor("_Color", new Color(255f/255f, 136f/255f, 220f/255f));
        if (passive)
            GetComponent<Renderer>().material.SetColor("_Color", new Color(250f/255f, 255f/255f, 127f/255f));
        if (!entangled)
            GetComponent<Renderer>().material.SetColor("_Color", Color.white);

    }
    
    /// <summary>
    /// Modifies entanglement states of the object
    /// </summary>
    /// <param name="isActive"> (bool) Is it active?</param>
    /// <param name="isPassive"> (bool) Is it passive?</param>
    public void SetEntanglementStates(bool isActive, bool isPassive)
    {
        active = isActive;
        passive = isPassive;
        entangled = active || passive;
    }

    /// <summary>
    /// Apply a force to the entanglable. Force will be added to the force queue and
    /// applied to the entanglable on the next frame. 
    /// </summary>
    /// <param name="force">A Vector2 force to apply.</param>
    public void ApplyForce(Vector2 force) {
        if (entangled && passive) {         // add force to queued forces list
            forces.Add(force);
        }
        ForceManager.current.ActiveForced(this, force);
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
    /// Checks if the object is in contact with a ground
    /// </summary>
    /// <returns>true if the object is on the ground</returns>
    public bool IsGrounded() {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);  // check for contact with ground layer
        return (collider != null);
    }

    /// <summary>
    /// Destroys the object (currently, destroyed = set as inactive)
    /// </summary>
    public void Destroy() {
        destroyed = true;                                    // flag as destroyed
        gameObject.SetActive(false);                         // disable the object
        if (respawnable) Invoke("Respawn", respawnTime);     // respawn after respawnTime delay
    }

    /// <summary>
    /// Respawns the object and moves it to the position of the respawnLocation transform
    /// </summary>
    void Respawn() {
        destroyed = false;
        gameObject.SetActive(true);                                         // re-enable the object
        gameObject.transform.position = respawnLocation.transform.position; // move the object to respawnLocation
    }
}
