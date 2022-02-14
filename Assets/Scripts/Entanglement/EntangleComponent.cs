using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// EntangleComponent - Keeps track of the Entanglable objects that a player currently has entangled.
/// </summary>
public class EntangleComponent : MonoBehaviour
{

    public Entanglable active;
    public List <Entanglable> passives;


    // Start is called before the first frame update
    void Start()
    {
        passives = new List <Entanglable>();
        ForceManager.current.onActiveForced += OnActive;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Set the current active Entanglable. 
    /// </summary>
    /// <param name="currentObject">An Entanglable object to set as active.</param>
    public void SetActive(Entanglable currentObject) {
        active = currentObject;
    }


    /// <summary>
    /// The list of passives is reset and a new passive is added in its place.
    /// </summary>
    /// <param name="newPassive">An Entanglable object to add to empty passive list.</param>
    public void SetPassive(Entanglable newPassive) {
        passives = new List<Entanglable> { newPassive };
    }


    /// <summary>
    /// Appends a list of passives to the current list. 
    /// </summary>
    /// <param name="newPassives">A list of Entanglable objects to add.</param>
    public void AddPassives(Entanglable[] newPassives) {
        passives.AddRange(newPassives);
    }


    /// <summary>
    /// Reset active entanglement. 
    /// </summary>
    public void UnsetActive() {
        active = null;
    }


    /// <summary>
    /// Removes the provided entanglable from the current list of passives. 
    /// </summary>
    /// <param name="removed">A currently existing Entanglable object to remove from list.</param>
    public void RemovePassive(Entanglable removed) {
        passives.Remove(removed);
    }


    /// <summary>
    /// Clears passive list (sets to null). Use SetPassive to create new list of passives.
    /// </summary>
    public void ClearPassives() {
        passives = null;
    }


    /// <summary>
    /// Loops through passive objects and applies force (if force is applied to current active object).
    /// </summary>
    private void OnActive(Entanglable e, Vector2 force) {
        if (e == active) {
            foreach (Entanglable passive in passives) {  // Loop through passive objects and apply force
                passive.ApplyForce(force);
            }
        }
    }
}
