using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Player : MonoBehaviour, IDestroyable {
    // Player Object Components/Variables
    public new Rigidbody2D rigidbody;
    public float speed = 6f;
    public float jumpForce = 12f;
    public float grabDistance = 0.5f;
    public float pushStrength = 20f;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Transform deathAnimation, respawnAnimation;
    public LayerMask pushMask;
    public float rayHeight = 0;


    [HideInInspector]
    public EntangleComponent entangleComponent;
    public GameObject EntanglingHelix, EntangledHelix;
    public bool showEntanglingHelix = true;
    public bool showEntangledHelix = true;
    
    private AudioManager audioManager;
    PlayerStateMachine stateMachine;

    private Vector2 position, previousPosition;
    public Transform respawnTransform;   // location that the player should respawn at when necessary
    [HideInInspector]
    public Vector3 respawnLocation;
    public float respawnDelay = 2.0f;
    [HideInInspector]
    public Vector2 worldVelocity;  // worldVelocity information

    [HideInInspector]
    public Entanglable pushedObject;
    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public RaycastHit2D hit;
    [HideInInspector]
    public bool grabbing;
    private float horzInput;

    private void Start(){
        // Initialize entangleComponent
        entangleComponent = gameObject.AddComponent<EntangleComponent>();
        entangleComponent.EntangledHelixPrefab = EntangledHelix;
        entangleComponent.EntanglingHelixPrefab = EntanglingHelix;
        entangleComponent.showEntanglingHelix = showEntanglingHelix;
        entangleComponent.showEntangledHelix = showEntangledHelix;
        // Add and initialize PlayerStateMachine
        stateMachine = gameObject.AddComponent<PlayerStateMachine>() as PlayerStateMachine;
        audioManager = FindObjectOfType<AudioManager>();
        stateMachine.Initialize(this);
        grabbing = false;


        // set the player's respawn position
        if (LevelRestarter.instance.GetCheckpointPosition() != Vector3.zero) {  // if LevelRestarter has a location, use it
            respawnLocation = LevelRestarter.instance.GetCheckpointPosition();
        } else {
            if (respawnTransform != null) {
                // otherwise, if a respawn transform was manually set, use that
                respawnLocation = respawnTransform.position;
            } else {
                // otherwise, default to using the elevator at the start of the level, if one is found
                GameObject elevator = GameObject.Find("Elevator_Open");
                if (elevator != null) {
                    respawnLocation = elevator.transform.position;
                } else {
                    // and finally, if no elevator was found, use the player's starting position
                    respawnLocation = transform.position;
                }
            }
        }
    }

    private void FixedUpdate(){
        if (gameObject.transform.parent != null) {  // update world velocity while attached to a parent object
            position = transform.position;
            worldVelocity = (position - previousPosition) / Time.deltaTime;
            previousPosition = position;
        } else {
            worldVelocity = Vector2.zero;
        }

        horzInput = Input.GetAxisRaw("Horizontal");
        if(!grabbing){
            if (horzInput > 0f)
                facingRight = true;
            else if (horzInput < 0f)
                facingRight = false;
        }
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
        if (other.gameObject.CompareTag("Platform")) {
            transform.parent = other.gameObject.transform;      // set parent of player to platform so player doesn't slide
        } else if (other.gameObject.CompareTag("Destroyer")) {
            Kill();
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Platform")) {
            if (gameObject.activeInHierarchy) transform.parent = null;
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
    public void SetAnimatorState(String state) {
        // 0 = idle
        // 1 = running
        // 2 = jumping
        // 3 = push/pull
        // 4 = destroyed
        // 5 = falling
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
            case "falling":
                animator.SetInteger("State", 2);
                break;
            default:
                Debug.LogWarning("Invalid player state set ('" + state + "')");
                break;
        }
    }

    public void ResetPlayer() {
        stateMachine.ChangeState(stateMachine.idleState);
        rigidbody.velocity = Vector2.zero;
    }

    public void Kill() {
        DestructionManager dm = DestructionManager.instance;
        if (dm != null) dm.Destroy(this, respawnDelay);
    }
    
    public GameObject GetGameObject() {
        return gameObject;
    }

    // do things that need to be done on destroying, before the gameobject is set to inactive
    public void Destroy() {
        entangleComponent.ClearEntangled();
        Instantiate(deathAnimation, transform.position, quaternion.identity);
        DestructionManager.instance.SetRespawnAnimation(respawnDelay - 1.01f, respawnAnimation, respawnLocation, "player_respawn");
    }
    
    // do things that need to be done on respawning, right after the game object is set as active again
    public void Respawn() {
        transform.parent = null;
        ResetPlayer();
        gameObject.transform.position = respawnLocation; // move the object to respawnTransform
    }
    
    private void OnCollisionEnter2D(Collision2D col) {
        print("bounce");
        if (col.gameObject.CompareTag("Bouncy")) {
            AudioManager.instance.Play("Bounce");
        }
    }

}
