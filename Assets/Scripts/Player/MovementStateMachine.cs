using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateMachine : StateMachine
{

    public Rigidbody2D rigidbody;
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
