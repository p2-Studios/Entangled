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

    [HideInInspector]
    public GameObject box;
    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public RaycastHit2D hit;

    private EntangleComponent entangleComponent;
    PlayerStateMachine stateMachine;

    private float horzInput;
    

    private void Start(){
        // Initialize entangleComponent
        entangleComponent = gameObject.AddComponent<EntangleComponent>();
        // Add and initialize PlayerStateMachine
        stateMachine = gameObject.AddComponent<PlayerStateMachine>() as PlayerStateMachine;
        stateMachine.Initialize(this);
    }

    private void Update(){
        horzInput = Input.GetAxisRaw("Horizontal");
        if (horzInput > 0f)
            facingRight = true;
        else if (horzInput < 0f)
            facingRight = false;

        if(facingRight)
            hit = Physics2D.Raycast(transform.position, Vector2.right, grabDistance);
        else
            hit = Physics2D.Raycast(transform.position, Vector2.left, grabDistance);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Platform")) {
            stateMachine.ChangeState(stateMachine.idleState);   // set state to idle
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
           
}
