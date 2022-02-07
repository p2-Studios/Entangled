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
    // basic rigidbody data
    private Rigidbody2D rb;

    // entanglement data
    private bool entangled, active, passive;

    // list of queued forces
    ArrayList forces;


    void Start() {
        // get the Rigidbody2D component of the object
        rb = GetComponent<Rigidbody2D>();

        // new object is not entangled
        entangled = false;
        active = false;
        passive = false;

        // object starts with no queued forces
        forces = new ArrayList();
    }

    void Update() {
        // apply each queued force
        if (forces.Count != 0) {
            foreach (Vector2 force in forces) {
                rb.AddForce(force, ForceMode2D.Impulse);
            }
            // all forces applied, clear the queue
            forces.Clear();
        }
    }

    /// <summary>
    /// Apply a force to the entanglable. Force will be added to the force queue and
    /// applied to the entanglable on the next frame. 
    /// </summary>
    /// <param name="force">A Vector2 force to apply.</param>
    public void applyForce(Vector2 force) {
        // add force to queued forces list
        if (entangled && passive) {
            forces.Add(force);
        }
    }

    /// <summary>
    /// Returns whether this object is entangled.
    /// </summary>
    /// <returns>true if the object is entangled.</returns>
    public bool isEntangled() {
        return entangled;
    }

    /// <summary>
    /// Returns whether this object is entangled as an active object.
    /// </summary>
    /// <returns>true if object is entangled and is an active object.</returns>
    public bool isActive() {
        return (entangled && active);
    }

    /// <summary>
    /// Returns whether this object is entangled as a passive object.
    /// </summary>
    /// <returns>true if object is entangled and is a passive object.</returns>
    public bool isPassive() {
        return (entangled && passive);
    }
}
