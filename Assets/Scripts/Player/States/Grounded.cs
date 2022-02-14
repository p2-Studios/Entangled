using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/OtUKsjPWzO8

// Parent Class for states that can transition to jumping
// Inherits BaseState properties
// Used to abstract transition to jump state away from idle and move (since both use it)
public class Grounded : BaseState {

    protected private PlayerStateMachine sm;   

    // Constructor, sets sm to active stateMachine
    public Grounded(string name, PlayerStateMachine stateMachine) : base(name, stateMachine){
        sm = (PlayerStateMachine)stateMachine;
    }

    // Detect any Space input, transition to jumpstate
    public override void UpdateLogic(){
        base.UpdateLogic();
        if(Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(sm.jumpState);
    }
}