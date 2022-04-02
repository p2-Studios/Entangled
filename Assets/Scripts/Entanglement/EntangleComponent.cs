using System.Collections;
using System.Collections.Generic;
using Entanglement;
using Game.CustomKeybinds;
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
        entangleMask = LayerMask.GetMask("Objects");

        allEntanglableObjects = GameObject.FindGameObjectsWithTag("Pushable");
    }

    // Update is called once per frame
    void Update() {
        if (PauseMenu.instance.paused || TerminalManager.instance.IsTerminalOpen()) {   // if paused or in a terminal, don't allow entanglement, and remove entangling helix
            if (EntanglingHelix != null) {
                Destroy(EntanglingHelix);
                if (active != null && passives.Count == 0) { // prevents active with no passive
                    ClearEntangled();
                }
            }
            return;
        }
        
        Keybinds keybindInstance = Keybinds.GetInstance();
        
        // draw helixes from active to passive(s)
        if (showEntangledHelix) {
            if (passives.Count != 0) {
                foreach (Entanglable e in passives) {
                    if (active != null) DrawEntangledHelix(active.transform.position, e.transform.position);
                }
            }
        }
        
        // Mouse Controls for Entangling Objects
        // (Hit detector- https://stackoverflow.com/a/61659152. I have modified this a bit to suit our needs)
        // mouse button held, draw line if there is an active
        if (Input.GetKey(keybindInstance.entangle)) {
            if (active != null) {
                // active, but no passives. Draw Entangling line
                Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cursorPos.z = 0;
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, entangleMask);
                if (hit.collider != null) {
                    Entanglable e = hit.collider.gameObject.GetComponentInParent<Entanglable>();
                    if (e != null && e != active) {
                        DrawEntanglingHelix(active.transform.position, e.transform.position);
                    }
                } else {
                    DrawEntanglingHelix(active.transform.position, cursorPos);
                }
            }
        } else {
            if (EntanglingHelix != null) Destroy(EntanglingHelix);
        }
        
        if (Input.GetKeyDown(keybindInstance.entangle)) {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, entangleMask);
            if (hit.collider != null) {
                // If one object is clicked, all objects get the click input. This is to prevent multiple selection
                //Debug.Log(hit.collider.gameObject.name);
                Entanglable e = hit.collider.gameObject.GetComponentInParent<Entanglable>();
                if (e == null) return;
                
                if (e == active) {  // e is already the active
                    mousePressedOnActive = true;
                    //Debug.Log("This is currently active.");
                } else { // e is not the active, so clear passives for new entanglement
                    ClearEntangled();
                }
                
                active = e;

                active.SetEntanglementStates(true, false, true);
                mousePressedOnActive = true;
            } else {
                // clicked on background
                if (active != null && passives.Count == 0) {
                    active.SetEntanglementStates(false, false, true);
                    UnsetActive();
                }
            }
        }

        if (Input.GetKeyUp(keybindInstance.entangle)) {
            // When mouse click is released
            if (active != null) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, entangleMask);
                if (hit.collider != null) {
                    // If one object is clicked, all objects get the click input. This is to prevent multiple selection
                    Entanglable e = hit.collider.gameObject.GetComponentInParent<Entanglable>();
                    if (e == null) return;
                    if (!active.Equals(e) && mousePressedOnActive) {
                        mousePressedOnActive = false;
                        // for now, only one passive object is possible, so clear other actives
                        foreach (Entanglable passive in passives) {
                            passive.SetEntanglementStates(false, false, true);
                        }
                        if (!passives.Contains(e)) {
                            FindObjectOfType<AudioManager>().Play("object_entangled");
                        }
                        e.SetEntanglementStates(false, true, true);
                        passives.Clear();
                        passives.Add(e);
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
        if (Input.GetKeyDown(keybindInstance.unentangle)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, entangleMask);
            if (hit.collider != null) {
                // If one object is clicked, all objects get the click input. This is to prevent multiple selection
                Entanglable e = hit.collider.gameObject.GetComponentInParent<Entanglable>();
                if (e == null) return;
                if (active == e || passives.Contains(e)) {
                    ClearEntangled();
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


        if (Input.GetKeyDown(keybindInstance.clearAllEntangled)) { // quick clear entangled objects
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

    public void RemoveActive(Entanglable e) {
        if (active == null) return;
        if (active.Equals(e)) {
            UnsetActive();
            Destroy(EntangledHelix);
            e.SetEntanglementStates(false, false, true);
            ClearPassives();
        }
    }

    public void Unentangle(Entanglable e) {
        if (e == null) return;
        RemoveActive(e);
        RemovePassive(e);
    }

    /// <summary>
    /// Removes the provided entanglable from the current list of passives. 
    /// </summary>
    /// <param name="removed">A currently existing Entanglable object to remove from list.</param>
    public void RemovePassive(Entanglable e) {
        if (e == null) return;
        if (passives.Contains(e)) {
            passives.Remove(e);
            Destroy(EntangledHelix);
            e.SetEntanglementStates(false, false, true);
            ClearEntangled();
        }
    }

    /// <summary>
    /// Loops through passive objects and applies force (if force is applied to current active object).
    /// </summary>
    private void OnActiveMoved(Entanglable e, Vector2 force) {
        if (e == null) return;
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