using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player movement state (for left and right inputs)
public class MoveState : Grounded {

    private float horzInput;

    // Constructor
    public MoveState(Player player) : base("Moving", player){}

    public override void Enter(){
        base.Enter();
        horzInput = 0f;
        playerStateMachine.spriteRenderer.color = Color.green;  // For testing purposes, will be used later for player animations
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horzInput) < Mathf.Epsilon)
            playerStateMachine.ChangeState(player.idleState);
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        Vector2 velocity = player.rigidbody.velocity;
        velocity.x = horzInput * player.speed;
        player.rigidbody.velocity = velocity;
    }
}
