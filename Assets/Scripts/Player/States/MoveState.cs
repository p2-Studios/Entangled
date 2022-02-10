using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player movement state (for left and right inputs)
public class MoveState : Grounded {

    private float horzInput;

    // Constructor
    public MoveState(PlayerStateMachine stateMachine) : base("Moving", stateMachine){}

    public override void Enter(){
        base.Enter();
        horzInput = 0f;
        sm.spriteRenderer.color = Color.green;  // For testing purposes, will be used later for player animations
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horzInput) < Mathf.Epsilon)
            stateMachine.ChangeState(sm.idleState);
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        Vector2 velocity = sm.rigidbody.velocity;
        velocity.x = horzInput * sm.speed;
        sm.rigidbody.velocity = velocity;
    }
}
