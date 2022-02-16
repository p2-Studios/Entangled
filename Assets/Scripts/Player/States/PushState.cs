using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : Grounded{

    private float horzInput;

    // Constructor
    public PushState(PlayerStateMachine playerSM,Player player) : base("Pushing", playerSM, player){}

    public override void Enter(){
        base.Enter();
        horzInput = 0f;
        Player.spriteRenderer.color = Color.blue;  // For testing purposes, will be used later for player animations
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horzInput) < Mathf.Epsilon)
            playerStateMachine.ChangeState(playerSM.idleState);
        
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        Vector2 velocity = Player.rigidbody.velocity;
        velocity.x = horzInput * Player.speed;
        Player.rigidbody.velocity = velocity;
    }
}
