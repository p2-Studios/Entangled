using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/OtUKsjPWzO8

// Player Jumping State
// Currently there is no air pivoting, player remains on trajectory when jumping
// Jump relies on ground layer, meaning that the floor they jump off has to have their layer as ground.
// This helps in State change detection
public class JumpState : BaseState {

    private Player player;
    
    private bool grounded;
    private int groundLayer = 1 << 6;   // Bitwise shift for ground layer number (should be 6)
 
    public JumpState(Player playerStateMachine) : base("Jumping", playerStateMachine){
        player = (Player)playerStateMachine;
    }

    // upon entering state, apply upward velocity to achieve jump
    public override void Enter(){
        base.Enter();
        player.spriteRenderer.color = Color.cyan;   // For testing purposes, will be used later for player animations

        Vector2 velocity = player.rigidbody.velocity;
        velocity.y += player.jumpForce;
        player.rigidbody.velocity = velocity;
    }

    // Switch states if grounded
    public override void UpdateLogic(){
        base.UpdateLogic();
        if(grounded)
            playerStateMachine.ChangeState(player.idleState);
    }

    // Check if player velocity is less than epsilon and rigidbody is touching a ground layer
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        grounded = player.rigidbody.velocity.y < Mathf.Epsilon && player.rigidbody.IsTouchingLayers(groundLayer);
    }

}
