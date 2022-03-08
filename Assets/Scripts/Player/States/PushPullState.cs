using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullState : BaseState {

    protected private PlayerStateMachine playerSM;
    private float horzInput;
    private float objMass;



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
        objMass = Player.pushedObject.GetComponent<Rigidbody2D>().mass;
        horzInput = 0f;
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
            Player.pushedObject.GetComponent<FixedJoint2D>().enabled = false;
            playerSM.ChangeState(playerSM.idleState);
        }
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        float strengthDif = (objMass - Player.pushStrength) * 4;
        Vector2 velocity = Player.rigidbody.velocity;
        velocity.x = horzInput * (Player.speed - strengthDif) ;
        Player.rigidbody.velocity = velocity;
    }


    // Exit calls (make sure variables don't remain)
    public override void Exit(){
        base.Exit();
        Player.grabbing = false;
        Player.pushedObject = null;
    }
}


