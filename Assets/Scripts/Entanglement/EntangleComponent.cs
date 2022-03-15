using System.Collections;
using System.Collections.Generic;
using Entanglement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.LWRP;

/// <summary>
/// EntangleComponent - Keeps track of the Entanglable objects that a player currently has entangled.
/// </summary>
public class EntangleComponent : MonoBehaviour {
    public Entanglable active;
    public List<Entanglable> passives;

    public bool showEntanglingHelix = true;
    public bool showEntangledHelix = true;

    private GameObject[] allEntanglableObjects;

    public GameObject EntanglingHelix, EntanglingHelixPrefab, EntangledHelix, EntangledHelixPrefab;

    LayerMask entangleMask;
    private bool mousePressedOnActive = false;

    // Start is called before the first frame update
    void Start() {
        passives = new List<Entanglable>();
        VelocityManager.GetInstance().onActiveMoved += OnActiveMoved;
        entangleMask = LayerMask.GetMask("Ground");

        allEntanglableObjects = GameObject.FindGameObjectsWithTag("Pushable");
    }

    // Update is called once per frame
    void Update() {
        // draw helixes from active to passive(s)
        if (showEntangledHelix) {
            if (passives.Count != 0) {
                foreach (Entanglable e in passives) {
                    DrawEntangledHelix(active.transform.position, e.transform.position);
                }
            }
        }
        
        // Mouse Controls for Entangling Objects
        // (Hit detector- https://stackoverflow.com/a/61659152. I have modified this a bit to suit our needs)
        // mouse button held, draw line if there is an active
        if (Input.GetMouseButton(0)) {
            if (active != null) {
                // active, but no passives. Draw Entangling line
                Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cursorPos.z = 0;
                DrawEntanglingHelix(active.transform.position, cursorPos);
            }
        } else {
            if (EntanglingHelix != null) Destroy(EntanglingHelix);
        }
        
        if (Input.GetMouseButtonDown(0)) {

            Debug.Log("Mouse down");
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, entangleMask);
            if (hit.collider != null) {
                // If one object is clicked, all objects get the click input. This is to prevent multiple selection
                //Debug.Log(hit.collider.gameObject.name);
                Entanglable e = hit.collider.gameObject.GetComponent<Entanglable>();
                if (e == null) return;

                if (active == null) {
                    active = e;
                    active.SetEntanglementStates(true, false, true);
                    mousePressedOnActive = true;
                    //Debug.Log("Selected " + hit.collider.gameObject.name + " as active");
                }

                if (e == active) {
                    mousePressedOnActive = true;
                    //Debug.Log("This is currently active.");
                }
            } else {
                // clicked on background
                if (active != null && passives.Count == 0) {
                    active.SetEntanglementStates(false, false, true);
                    UnsetActive();
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            // When mouse click is released
            if (active != null) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, entangleMask);
                if (hit.collider != null) {
                    // If one object is clicked, all objects get the click input. This is to prevent multiple selection
                    Entanglable e = hit.collider.gameObject.GetComponent<Entanglable>();
                    if (e == null) return;
                    if (active.Equals(e)) {
                        Debug.Log("Drag and release mouse on another object");
                    } else if (mousePressedOnActive) {
                        mousePressedOnActive = false;
                        if (!passives.Contains(e)) {
                            e.SetEntanglementStates(false, true, true);
                            passives.Add(e);
                            FindObjectOfType<AudioManager>().Play("object_entangled");
                        }
                    }
                }
            }
            
            if (passives.Count == 0){
                if (active != null) {
                    active.SetEntanglementStates(false, false, true);
                    active = null;
                }
            }
        }

        // Right click pressed
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, entangleMask);
            if (hit.collider != null) {
                // If one object is clicked, all objects get the click input. This is to prevent multiple selection
                Entanglable e = hit.collider.gameObject.GetComponent<Entanglable>();
                if (e == null) return;
                if (active == e) {
                    ClearEntangled();
                }

                if (passives.Contains(e)) {
                    //Debug.Log("Removed the object from the list of passives");
                    e.SetEntanglementStates(false, false, true);
                    passives.Remove(e);
                }
            }
        }

        if (active != null && passives.Count == 0) {
            foreach (GameObject obj in allEntanglableObjects) {
                Entanglable e = obj.GetComponent<Entanglable>();
                if (e != null) {
                    if (!e.IsEntangled())
                        e.SetEntanglementStates(false, false, true);
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Q)) { // Q to quick clear entangleds
            ClearEntangled();
        }
        
    }

    public void ClearEntangled() {
        if (active != null) {
            active.SetEntanglementStates(false, false, true);
            UnsetActive();
            ClearPassives();
            if (EntangledHelix != null) {
                Destroy(EntangledHelix);
            }
        }
    }
    
    /// <summary>
    /// Clears passive list (sets to new list). Use SetPassive to create new list of passives.
    /// </summary>
    public void ClearPassives() {
        if (passives != null) {
            foreach (Entanglable passive in passives) {
                passive.SetEntanglementStates(false, false, true);
            }
        }
        passives = new List<Entanglable>();
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
        passives = new List<Entanglable> {newPassive};
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
    /// Loops through passive objects and applies force (if force is applied to current active object).
    /// </summary>
    private void OnActiveMoved(Entanglable e, Vector2 force) {
        if (e == active) {
            foreach (Entanglable passive in passives) {
                // Loop through passive objects and apply force
                passive.ApplyVelocity(force);
            }
        }
    }
    
    // source: https://answers.unity.com/questions/844792/unity-stretch-sprite-between-two-points-at-runtime.html
    // Stretches the helix sprites appropriately
    // TODO: Refactor this mess
    public void DrawEntanglingHelix(Vector3 initialPosition, Vector3 finalPosition) {
        if (EntanglingHelix == null) EntanglingHelix = Instantiate(EntanglingHelixPrefab);
        Vector3 centerPos = (initialPosition + finalPosition) / 2f;
        EntanglingHelix.transform.position = centerPos;
        Vector3 direction = finalPosition - initialPosition; 
        direction = Vector3.Normalize(direction);
        EntanglingHelix.transform.right = direction;
        var scale = new Vector2(1f,0.33f) { x = Vector3.Distance(initialPosition, finalPosition) };
        //EntanglingHelix.transform.GetChild(0).localScale = scale; light stuff - todo
        EntanglingHelix.GetComponent<SpriteRenderer>().size = scale;
    }
    
    public void DrawEntangledHelix(Vector3 initialPosition, Vector3 finalPosition) {
        if (EntangledHelix == null) EntangledHelix = Instantiate(EntangledHelixPrefab);
        Vector3 centerPos = (initialPosition + finalPosition) / 2f;
        EntangledHelix.transform.position = centerPos;
        Vector3 direction = finalPosition - initialPosition;
        direction = Vector3.Normalize(direction);
        EntangledHelix.transform.right = direction;
        var scale = new Vector2(1f,0.33f) { x = Vector3.Distance(initialPosition, finalPosition)};
        //EntangledHelix.transform.GetChild(0).localScale = scale; light stuff - todo
        EntangledHelix.GetComponent<SpriteRenderer>().size = scale;
    }
}