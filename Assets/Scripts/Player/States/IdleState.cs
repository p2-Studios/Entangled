using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player Idle state
// Inherits Grounded state
public class IdleState : Grounded {

    // Constructor
    public IdleState(PlayerStateMachine playerSM, Player player, AudioManager audioManager) : base("Idle", playerSM,
        player) {
        this.audioManager = audioManager;
    }

    public override void Enter(){
        base.Enter();
        horzInput = 0f;
        playerSM.player.SetAnimatorState("idle");
    }

    // Detect if horizontal input more than Epsilon (switch to move if true)
    public override void UpdateLogic(){
        base.UpdateLogic();    
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            playerSM.ChangeState(playerSM.moveState);   
    }

    // Updates while player remains within trigger collider
    public override void UpdateTrigger(Collider2D collider){
        base.UpdateTrigger(collider);
        if(collider.gameObject.tag == "Pushable"){
            touchingBox = true;
        }
    }

}
