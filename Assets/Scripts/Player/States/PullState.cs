using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullState : ApplyingForce {

    private float horzInput;
    private bool pulling;
    

    // Constructor
    public PullState(PlayerStateMachine playerStateMachine,Player player) : base("Pulling", playerStateMachine, player){}

    public override void Enter(){
        base.Enter();
        pulling = true;
        horzInput = 0f;
        Player.spriteRenderer.color = Color.red;  // For testing purposes, will be used later for player animations
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.E)){
            pulling = false;
        }
        if (!pulling)
            playerSM.ChangeState(playerSM.idleState);
        
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        Vector2 velocity = Player.rigidbody.velocity;
        velocity.x = horzInput * Player.speed;
        Player.rigidbody.velocity = velocity;
    }
    
    // Detect if player is disconected for triggercollider
    public override void ExitCollision(Collider2D collider){
        if (collider.gameObject.tag == "Pushable"){
            pulling = false;
        }
    }


    public override void Exit()
    {
        pulling = false;
        base.Exit();
    }
}


