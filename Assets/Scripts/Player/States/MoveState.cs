using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player movement state (for left and right inputs)
public class MoveState : BaseState {

    private PlayerStateMachine sm;
    private float horzInput;

    public MoveState(PlayerStateMachine stateMachine) : base("Moving", stateMachine){
        sm = (PlayerStateMachine)stateMachine;
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
