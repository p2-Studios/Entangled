using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player Specific state machine
// inherents StateMachine properties
// Contains Player's variables and states
public class PlayerStateMachine : MonoBehaviour {

    public BaseState currentState;

    // States
    public IdleState idleState;
    public MoveState moveState;
    public JumpState jumpState;
    public PushPullState pushpullState;

    public Player player;

    public void Initialize(Player player) {

        AudioManager audioManager = FindObjectOfType<AudioManager>();

        idleState = new IdleState(this,player,audioManager);
        moveState = new MoveState(this,player,audioManager);
        jumpState = new JumpState(this,player,audioManager);
        pushpullState = new PushPullState(this,player,audioManager);

        this.player = player;
        
        currentState = idleState;   
        if (currentState != null)
            currentState.Enter();
    }    

    // Calculate transition logic for states
    void Update(){
        if(currentState != null)
            currentState.UpdateLogic();
    }

    // Calculate Physics logic (movement)
    void LateUpdate(){
        if(currentState != null)
            currentState.UpdatePhysics();
    }

    // Detect start of collider collisions with objects
    void OnCollisionEnter2D(Collision2D collision){
        if(currentState != null)
            currentState.EnterCollision(collision);
    }

    // Detect start of trigger collisions with objects
    void OnTriggerEnter2D(Collider2D collider){
        if(currentState != null)
            currentState.EnterTrigger(collider);
    }

    // Updates contact with collider
    void OnCollisionStay2D(Collision2D collision){
        if(currentState != null)
            currentState.UpdateCollision(collision);
    }

    // Updates contact with trigger
    void OnTriggerStay2D(Collider2D collider){
        if(currentState != null)
            currentState.UpdateTrigger(collider);
    }

    // Detect when collisions with trigger colliders ends
    void OnCollisionExit2D(Collision2D collision){
        if(currentState != null)
            currentState.ExitCollision(collision);
    }

    // Detect when collisions with trigger colliders ends
    void OnTriggerExit2D(Collider2D collider){
        if(currentState != null)
            currentState.ExitTrigger(collider);
    }

    // When changing states, call exit method for current state, change state, them call enter method for new state
    public void ChangeState(BaseState newState){
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    /*
    // For testing purposes (displays current state name when running game)
    private void OnGUI(){
        GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
        string content = currentState != null ? currentState.name : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        GUILayout.EndArea();
    }
    */


}
