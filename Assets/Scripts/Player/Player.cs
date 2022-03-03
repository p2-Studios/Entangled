using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Player : MonoBehaviour {
    // Player Object Components/Variables
    public new Rigidbody2D rigidbody;
    public float speed = 6f;
    public float jumpForce = 12f;
    public float grabDistance = 0.5f;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public LayerMask pushMask;
    public float rayHeight = 0;

    private EntangleComponent entangleComponent;
    private AudioManager audioManager;
    PlayerStateMachine stateMachine;

    private Vector2 position, previousPosition;
    public Transform respawnLocation;   // location that the player should respawn at when necessary
    [HideInInspector]
    public Vector2 worldVelocity;  // worldVelocity information

    [HideInInspector]
    public Entanglable pushedObject;
    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public RaycastHit2D hit;
    private float horzInput;

    private void Start(){
        // Initialize entangleComponent
        entangleComponent = gameObject.AddComponent<EntangleComponent>();
        // Add and initialize PlayerStateMachine
        stateMachine = gameObject.AddComponent<PlayerStateMachine>() as PlayerStateMachine;
        audioManager = FindObjectOfType<AudioManager>();
        stateMachine.Initialize(this);
    }

    private void Update(){
        if (gameObject.transform.parent != null) {  // update world velocity while attached to a parent object
            position = transform.position;
            worldVelocity = (position - previousPosition) / Time.deltaTime;
            previousPosition = position;
        } else {
            worldVelocity = Vector2.zero;
        }

        horzInput = Input.GetAxisRaw("Horizontal");
        if (horzInput > 0f)
            facingRight = true;
        else if (horzInput < 0f)
            facingRight = false;

        Vector3 ray = transform.position;
        ray.y += rayHeight; 

        if (facingRight) {
            hit = Physics2D.Raycast(ray, Vector2.right, grabDistance, pushMask);
            spriteRenderer.flipX = false;
        }
        else {
            hit = Physics2D.Raycast(ray, Vector2.left, grabDistance, pushMask);
            spriteRenderer.flipX = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Platform")) {
            transform.parent = other.gameObject.transform;      // set parent of player to platform so player doesn't slide
        } else if (other.gameObject.CompareTag("Destroyer")) {
            Kill();
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Platform")) {
            transform.parent = null;
        }
    }

    void OnDrawGizmos(){
        Vector3 ray = transform.position;
        ray.y += rayHeight; 
		Gizmos.color = Color.yellow;
        if (facingRight)
            Gizmos.DrawLine(ray, (Vector2)ray + Vector2.right *  grabDistance);
        else
            Gizmos.DrawLine(ray, (Vector2)ray + Vector2.left * grabDistance);
	}


    // Sets the animator state (int) of the player's animator, to transition between animations
    // author: Dakota
    public void SetAnimatorState(String state) {
        // 0 = idle
        // 1 = running
        // 2 = jumping
        // 3 = push/pull
        // 4 = destroyed
        switch (state) {
            case "idle":
                animator.SetInteger("State", 0);
                break;
            case "running":
                animator.SetInteger("State", 1);
                break;
            case "jumping":
                animator.SetInteger("State", 2);
                break;
            case "pushing":
                animator.SetInteger("State", 3);
                break;
            case "pulling":
                animator.SetInteger("State", 3);
                break;
            case "dying":
                animator.SetInteger("State", 4);
                break;
            default:
                Debug.LogWarning("Invalid player state set ('" + state + "')");
                break;
        }
    }

    /// <summary>
    /// Destroys the object (currently, destroyed = set as inactive)
    /// </summary>
    public void Kill() {
        //if (!gameObject.activeSelf) return;                  // cancel if already dead
        //gameObject.SetActive(false);                         // disable the object
        //audioManager.Play("player_death");
        //Invoke(nameof (Respawn), 3.0f);     // respawn after respawnTime delay
        gameObject.transform.position = respawnLocation.transform.position; // move the object to respawnLocation

    }

    /// <summary>
    /// Respawns the object and moves it to the position of the respawnLocation transform
    /// </summary>
    void Respawn() {
        gameObject.SetActive(true);                                         // re-enable the object
        gameObject.transform.position = respawnLocation.transform.position; // move the object to respawnLocation
    }
}
