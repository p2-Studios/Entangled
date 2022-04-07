using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player movement state (for left and right inputs)
public class MoveState : Grounded {

    private bool pushRange;

    // Constructor
    public MoveState(PlayerStateMachine playerSM,Player player, AudioManager audioManager) : base("Moving", playerSM, player){
        this.audioManager = audioManager;
    }

    public override void Enter(){
        base.Enter();
        touchingBox = false; 
        horzInput = 0f;
        playerSM.player.SetAnimatorState("running");
        //if (!audioManager.IsLooping("movement_run"))    // don't start another loop if already looping sound
        //    audioManager.StartLoopingSound("movement_run", 0.2f);
    }

    public override void Exit() {
        base.Exit();
        //audioManager.StopLoopingSound("movement_run");
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)){
            playerSM.player.SetAnimatorState("idle");
            //playerSM.ChangeState(playerSM.idleState);
        }
        if (Mathf.Abs(horzInput) < Mathf.Epsilon)
            playerSM.ChangeState(playerSM.idleState);
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        if(!haltMovement){
            Vector2 velocity = Player.rigidbody.velocity;
            velocity.x = horzInput * Player.speed;
            Player.rigidbody.velocity = velocity;
        }
    }

    // Updates while player remains within trigger collider
    public override void UpdateTrigger(Collider2D collider){
        base.UpdateTrigger(collider);
        if(collider.gameObject.tag == "Pushable"){
            touchingBox = true;
        }
    }

    
}
