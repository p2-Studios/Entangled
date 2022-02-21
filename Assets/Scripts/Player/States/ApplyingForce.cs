using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyingForce : BaseState {

    protected private PlayerStateMachine playerSM;

    // Constructor, sets sm to active stateMachine
    public ApplyingForce(string name, PlayerStateMachine playerStateMachine, Player player) : base(name, playerStateMachine, player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        Player = player;
    }

    // Detect any Space input, transition to jumpstate
    public override void UpdateLogic(){
        base.UpdateLogic();
        if (Input.GetKeyDown(KeyCode.Space))
            playerSM.ChangeState(playerSM.jumpState);
            
    }

}
