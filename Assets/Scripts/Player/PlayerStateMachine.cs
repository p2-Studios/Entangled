using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player Specific state machine
// inherents StateMachine properties
// Contains Player's variables and states
public class PlayerStateMachine : MonoBehaviour {

    BaseState currentState;


    // States
    public IdleState idleState;
    public MoveState moveState;
    public JumpState jumpState;
    public PushState pushState;
    public PullState pullState;


    public void Initialize(Player player){


        idleState = new IdleState(this,player);
        moveState = new MoveState(this,player);
        jumpState = new JumpState(this,player);
        pushState = new PushState(this,player);
        pullState = new PullState(this,player);

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

    // Detect collisions with objects
    void OnTriggerEnter2D(Collider2D collider){
        if(currentState != null)
            currentState.EnterCollision(collider);
    }

    void OnTriggerStay2D(Collider2D collider){
        if(currentState != null)
            currentState.UpdateCollision(collider);
    }

    // Detect when collisions with trigger colliders ends
    void OnTriggerExit2D(Collider2D collider){
        if(currentState != null)
            currentState.ExitCollision(collider);
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
