using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullState : BaseState {

    protected private PlayerStateMachine playerSM;
    private float horzInput;

    // Constructor
    public PushPullState(PlayerStateMachine playerStateMachine,Player player) : base("Pulling", playerStateMachine, player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        Player = player;
    }

    // Enter calls
    public override void Enter(){
        base.Enter();
        Player.box.GetComponent<FixedJoint2D>().enabled = true;
        Player.box.GetComponent<FixedJoint2D>().connectedBody = Player.rigidbody;
        //Player.box.GetComponent<boxpull> ().beingPushed = true;

        horzInput = 0f;
        Player.spriteRenderer.color = Color.red;  // For testing purposes, will be used later for player animations
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");

        // Let go of object
        if (Input.GetKeyDown(KeyCode.E)){
            Player.box.GetComponent<FixedJoint2D>().enabled = false;
			//Player.box.GetComponent<boxpull> ().beingPushed = false;
            playerSM.ChangeState(playerSM.idleState);
        }
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        Vector2 velocity = Player.rigidbody.velocity;
        velocity.x = horzInput * Player.speed;
        Player.rigidbody.velocity = velocity;
    }
    

    // Exit calls (make sure variables don't remain)
    public override void Exit(){
        base.Exit();
    }
}


