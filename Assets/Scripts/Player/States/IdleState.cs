using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// Player Idle state
// Inherits Grounded state
public class IdleState : Grounded {

    private float horzInput;

    // Constructor
    public IdleState(Player player) : base("Idle", player){}

    public override void Enter(){
        base.Enter();
        horzInput = 0f;
        player.spriteRenderer.color = Color.black;  // For testing purposes, will be used later for player animations
    }

    // Detect if horizontal input more than Epsilon (switch to move if true)
    public override void UpdateLogic(){
        base.UpdateLogic();    
        horzInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horzInput) > Mathf.Epsilon)
            playerStateMachine.ChangeState(player.moveState);
    }
}
