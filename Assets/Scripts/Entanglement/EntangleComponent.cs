using System.Collections;
using System.Collections.Generic;
using Entanglement;
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
        VelocityManager.GetInstance().onActiveMoved += OnActiveMoved;
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse Controls for Entangling Objects
        // (Hit detector- https://stackoverflow.com/a/61659152. I have modified this a bit to suit our needs)
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)      // If one object is clicked, all objects get the click input. This is to prevent multiple selection
            {
                if (active == hit.collider.gameObject.GetComponent<Entanglable>())
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        Debug.Log("Removed the active object");
                        active.SetEntanglementStates(false, false);
                        UnsetActive();
                        if (passives != null)
                        {
                            foreach (Entanglable passive in passives)
                            {
                                passive.SetEntanglementStates(false, false);
                            }
                        }
                        ClearPassives();
                        passives = new List<Entanglable>();
                    }
                    else
                        Debug.Log("This is already active");
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        Debug.Log("Cannot remove a non-active object");
                    }
                    else
                    {
                        if (active != null)
                            active.SetEntanglementStates(false, false);
                        active = hit.collider.gameObject.GetComponent<Entanglable>();

                        if (passives != null)
                        {
                            foreach (Entanglable passive in passives)
                            {
                                passive.SetEntanglementStates(false, false);
                            }
                        }
                        
                        ClearPassives();
                        active.SetEntanglementStates(true, false);
                        passives = new List<Entanglable>();
                        Debug.Log("Selected " + hit.collider.gameObject.name + " as active");
                    }
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    Debug.Log("Deselected active and all passive objects");

                    if (active != null)
                        active.SetEntanglementStates(false, false);
                    UnsetActive();

                    if (passives != null)
                    {
                        foreach (Entanglable passive in passives)
                        {
                            passive.SetEntanglementStates(false, false);
                        }
                    }

                    ClearPassives();
                    passives = new List<Entanglable>();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))    // When right click is pressed
        {
            if (active == null)
            {
                Debug.Log("No active object found");
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                if (hit.collider !=
                    null) // If one object is clicked, all objects get the click input. This is to prevent multiple selection
                {
                    if (active == hit.collider.gameObject.GetComponent<Entanglable>())
                    {
                        if (Input.GetKey(KeyCode.LeftControl))
                            Debug.Log("Cannot remove an active object from list of passives");
                        else
                            Debug.Log("This object cannot be used as a passive object as it is currently active");
                    }
                    else
                    {
                        if (passives.Contains(hit.collider.gameObject.GetComponent<Entanglable>()))
                        {
                            if (Input.GetKey(KeyCode.LeftControl))
                            {
                                Debug.Log("Removed the object from the list of passives");
                                hit.collider.gameObject.GetComponent<Entanglable>().SetEntanglementStates(false, false);
                                passives.Remove(hit.collider.gameObject.GetComponent<Entanglable>());
                            }
                            else
                                Debug.Log("Object is already passive");
                        }
                        else
                        {
                            if (Input.GetKey(KeyCode.LeftControl))
                            {
                                Debug.Log("Cannot remove a non-passive object");
                            }
                            else
                            {
                                Debug.Log("Added " + hit.collider.gameObject.name +
                                          " to passive objects. Currently active- " +
                                          active.name);
                                hit.collider.gameObject.GetComponent<Entanglable>().SetEntanglementStates(false, true);
                                passives.Add(hit.collider.gameObject.GetComponent<Entanglable>());
                            }
                        }
                    }
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Q))        // Debugging
        {
            Debug.Log("Active- " + active);
            foreach (Entanglable e in passives)
            {
                Debug.Log("Passive- " + e);
            }
        }
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
    private void OnActiveMoved(Entanglable e, Vector2 force) {
        if (e == active) {
            foreach (Entanglable passive in passives) {  // Loop through passive objects and apply force
                passive.ApplyVelocity(force);
            }
        }
    }
}
