using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/OtUKsjPWzO8

// Parent Class for Idle and Move states
// Inherits BaseState properties
// Used to abstract transition jump and collision states changes away from children
public class Grounded : BaseState {

    protected private PlayerStateMachine playerSM;
    protected private float horzInput;



    // Constructor, sets sm to active stateMachine
    public Grounded(string name, PlayerStateMachine playerStateMachine, Player player) : base(name, playerStateMachine, player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        Player = player;
    }

    // Update Logic changes (key pressses)
    public override void UpdateLogic(){
        base.UpdateLogic();

        //Physics2D.queriesStartInColliders = false;        
        if (Player.hit.collider != null && Player.hit.collider.gameObject.tag == "Pushable" && Input.GetKeyDown(KeyCode.E)){
            //Player.box = Player.hit.collider.gameObject;
            Player.pushedObject = Player.hit.collider.gameObject.GetComponent<Entanglable>();
            playerSM.ChangeState(playerSM.pushpullState);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            playerSM.ChangeState(playerSM.jumpState);
        
    }

    // Checks when player enters trigger collider
    public override void EnterTrigger(Collider2D collider){
        base.EnterTrigger(collider);
    }

    // Updates while player remains within trigger collider
    public override void UpdateTrigger(Collider2D collider){
        base.UpdateTrigger(collider);
    }

    // Checks when player exits trigger collider
    public override void ExitTrigger(Collider2D collider){
        base.ExitTrigger(collider);

    }



}
