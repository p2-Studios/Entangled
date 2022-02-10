using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player Specific state machine
// inherents StateMachine properties
// Contains Player's variables and states
public class PlayerStateMachine : StateMachine{

    // Player Object Components/Variables
    public new Rigidbody2D rigidbody;
    public float speed = 6f;
    public float jumpForce = 12f;
    public SpriteRenderer spriteRenderer;

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

    // Initalize first state to idle
    protected override BaseState GetInitialState(){
        return idleState;
    }
}
