using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullState : BaseState {

    protected private PlayerStateMachine playerSM;
    private float horzInput;
    private int groundLayer = 1 << 6;

    // Constructor
    public PushPullState(PlayerStateMachine playerStateMachine,Player player,AudioManager audioManager) : base("Pulling", playerStateMachine, player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        this.audioManager = audioManager;
        Player = player;
    }

    // Enter calls
    public override void Enter(){
        base.Enter();
        Player.grabbing = true;
        Player.pushedObject.GetComponent<FixedJoint2D>().enabled = true;
        Player.pushedObject.GetComponent<FixedJoint2D>().connectedBody = Player.rigidbody;

        horzInput = 0f;
        //Player.spriteRenderer.color = Color.red;  // For testing purposes, will be used later for player animations
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");

        // Let go of object
        if (Input.GetKeyDown(KeyCode.E)){
            Player.pushedObject.GetComponent<FixedJoint2D>().enabled = false;
            playerSM.ChangeState(playerSM.idleState);
        }
        if(Player.hit.collider == null){
            Debug.Log("HERER");
            Player.pushedObject.GetComponent<FixedJoint2D>().enabled = false;
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
        Player.grabbing = false;
        Player.pushedObject = null;
    }
}


