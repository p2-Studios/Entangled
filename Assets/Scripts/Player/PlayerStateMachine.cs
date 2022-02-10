using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player Specific state machine
// inherents StateMachine properties
// Contains Player specific variables and states
public class PlayerStateMachine : StateMachine{

    public new Rigidbody2D rigidbody;
    public float speed = 6f;
    public SpriteRenderer spriteRenderer;

    public IdleState idleState;
    public MoveState moveState;

    private void Awake(){
        idleState = new IdleState(this);
        moveState = new MoveState(this);
    }

    protected override BaseState GetInitialState(){
        return idleState;
    }
}
