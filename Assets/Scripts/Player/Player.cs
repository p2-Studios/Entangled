using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player Specific state machine
// inherents StateMachine properties
// Contains Player's variables and states
public class Player : Entanglable{

    // Player Object Components/Variables
    public new Rigidbody2D rigidbody;
    public float speed = 6f;
    public float jumpForce = 12f;
    public SpriteRenderer spriteRenderer;


    BaseState currentState;

    // States
    public IdleState idleState;
    public MoveState moveState;
    public JumpState jumpState;

    // create states
    private void Awake(){
        idleState = new IdleState(this);
        moveState = new MoveState(this);
        jumpState = new JumpState(this);
    }




    // Below is StateMachine Code for Player  

    // Initialize state
    void Start(){
        currentState = idleState;   
        if (currentState != null)
            currentState.Enter();
    }

    // Calculate transition logic (state change)
    void Update(){
        if(currentState != null)
            currentState.UpdateLogic();
    }

    // Calculate Physics logic (movement)
    void LateUpdate(){
        if(currentState != null)
            currentState.UpdatePhysics();
    }

    // When changing states, call exit method for current state, change state, them call enter method for new state
    public void ChangeState(BaseState newState){
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // For testing purposes (displays current state name when running game)
    private void OnGUI(){
        GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
        string content = currentState != null ? currentState.name : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        GUILayout.EndArea();
    }


}
