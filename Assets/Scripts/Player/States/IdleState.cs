using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player Idle state (no input at all)
public class IdleState : BaseState {

    private PlayerStateMachine sm;
    private float horzInput;

    public IdleState(PlayerStateMachine stateMachine) : base("Idle", stateMachine){
        sm = (PlayerStateMachine)stateMachine;
    }

    public override void Enter(){
        base.Enter();
        horzInput = 0f;
        sm.spriteRenderer.color = Color.black;
    }

    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horzInput) > Mathf.Epsilon)
            stateMachine.ChangeState(sm.moveState);
    }
}
