using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState {

    private MovementStateMachine sm;
    private float horzInput;

    public IdleState(MovementStateMachine stateMachine) : base("Idle", stateMachine){
        sm = (MovementStateMachine)stateMachine;
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
