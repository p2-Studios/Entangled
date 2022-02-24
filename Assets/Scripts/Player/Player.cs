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
    public SpriteRenderer spriteRenderer;

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
}
