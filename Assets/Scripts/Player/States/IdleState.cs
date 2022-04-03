using System.Collections;
using System.Collections.Generic;
using Game.CustomKeybinds;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player Idle state
// Inherits Grounded state
public class IdleState : Grounded {


    // Constructor
    /*public IdleState(PlayerStateMachine playerSM, Player player, AudioManager audioManager) : base("Idle", playerSM,
        player) {
        this.audioManager = audioManager;
    }*/

    public override void Enter(){
        base.Enter();
        Player.rigidbody.velocity = Vector3.zero;
        horzInput = 0f;
        playerSM.player.SetAnimatorState("idle");
    }

    // Detect if horizontal input more than Epsilon (switch to move if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxisRaw("Horizontal");

        if(haltMovement && Input.GetKeyDown(Keybinds.GetInstance().jump))
            playerSM.ChangeState(playerSM.jumpState);    
        if (Input.GetKeyDown(Keybinds.GetInstance().moveLeft) || Input.GetKeyDown(Keybinds.GetInstance().moveRight))
            playerSM.player.SetAnimatorState("running");
        if (Mathf.Abs(horzInput) > Mathf.Epsilon)
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
