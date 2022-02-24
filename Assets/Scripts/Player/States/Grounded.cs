using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/OtUKsjPWzO8

// Parent Class for Idle and Move states
// Inherits BaseState properties
// Used to abstract transition jump and collision states changes away from children
public class Grounded : BaseState {

    protected private PlayerStateMachine playerSM;
    protected private bool triggerRange;

    // Constructor, sets sm to active stateMachine
    public Grounded(string name, PlayerStateMachine playerStateMachine, Player player) : base(name, playerStateMachine, player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        Player = player;
        triggerRange = false;
    }

    // Update Logic changes (key pressses)
    public override void UpdateLogic(){
        base.UpdateLogic();
        if (Input.GetKeyDown(KeyCode.Space))
            playerSM.ChangeState(playerSM.jumpState);
        if (triggerRange && Input.GetKeyDown(KeyCode.E)){
            playerSM.ChangeState(playerSM.pushpullState);
        }
    }

    // Checks when player enters trigger collider
    public override void EnterTrigger(Collider2D collider){
        base.EnterTrigger(collider);
        if (collider.gameObject.tag == "Pushable"){
            triggerRange = true;
        }
    }

    // Updates while player remains within trigger collider
    public override void UpdateTrigger(Collider2D collider){
        base.UpdateTrigger(collider);
        if(collider.gameObject.tag == "Pushable"){
            triggerRange = true;
        }
    }

    // Checks when player exits trigger collider
    public override void ExitTrigger(Collider2D collider){
        base.ExitTrigger(collider);
        if(collider.tag == "Pushable")
            triggerRange = false;
    }

}
