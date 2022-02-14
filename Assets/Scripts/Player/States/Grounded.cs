using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/OtUKsjPWzO8

// Parent Class for states that can transition to jumping
// Inherits BaseState properties
// Used to abstract transition to jump state away from idle and move (since both use it)
public class Grounded : BaseState {

    protected private PlayerStateMachine playerSM;

    // Constructor, sets sm to active stateMachine
    public Grounded(string name, PlayerStateMachine playerStateMachine, Player player) : base(name, playerStateMachine, player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        Player = player;
    }

    // Detect any Space input, transition to jumpstate
    public override void UpdateLogic(){
        base.UpdateLogic();
        if(Input.GetKeyDown(KeyCode.Space))
            playerSM.ChangeState(playerSM.jumpState);
    }
}
