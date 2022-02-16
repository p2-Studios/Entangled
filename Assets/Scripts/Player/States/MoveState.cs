using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player movement state (for left and right inputs)
public class MoveState : Grounded {

    private float horzInput;
    private int pushLayer = 1 << 7; 

    // Constructor
    public MoveState(PlayerStateMachine playerSM,Player player) : base("Moving", playerSM, player){}

    public override void Enter(){
        base.Enter();
        horzInput = 0f;
        Player.spriteRenderer.color = Color.green;  // For testing purposes, will be used later for player animations
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horzInput) < Mathf.Epsilon)
            playerStateMachine.ChangeState(playerSM.idleState);  
    }

    // Detect if player is colliding with objects
    public override void UpdateCollision(Collision2D collision){
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Pushable"){
            playerStateMachine.ChangeState(playerSM.pushState);
        }
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        Vector2 velocity = Player.rigidbody.velocity;
        velocity.x = horzInput * Player.speed;
        Player.rigidbody.velocity = velocity;
    }
}
