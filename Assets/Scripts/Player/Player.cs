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

    private EntangleComponent entangleComponent;
    PlayerStateMachine stateMachine;

    private Vector2 position, previousPosition;
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

        if (facingRight) {
            hit = Physics2D.Raycast(transform.position, Vector2.right, grabDistance, pushMask);
            spriteRenderer.flipX = false;
        }
        else {
            hit = Physics2D.Raycast(transform.position, Vector2.left, grabDistance, pushMask);
            spriteRenderer.flipX = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Platform")) {
            transform.parent = other.gameObject.transform;      // set parent of player to platform so player doesn't slide
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Platform")) {
            transform.parent = null;
        }
    }

    void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
        if (facingRight)
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right *  grabDistance);
        else
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * grabDistance);
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
}
