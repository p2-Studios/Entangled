using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState {

    private MovementStateMachine sm;
    private float horzInput;

    public MoveState(MovementStateMachine stateMachine) : base("Moving", stateMachine){
        sm = (MovementStateMachine)stateMachine;
    }

    public override void Enter(){
        base.Enter();
        horzInput = 0f;
        sm.spriteRenderer.color = Color.green;
    }

    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horzInput) < Mathf.Epsilon)
            stateMachine.ChangeState(sm.idleState);
    }

    public override void UpdatePhysics(){
        base.UpdatePhysics();
        Vector2 velocity = sm.rigidbody.velocity;
        velocity.x = horzInput * sm.speed;
        sm.rigidbody.velocity = velocity;
    }
}
