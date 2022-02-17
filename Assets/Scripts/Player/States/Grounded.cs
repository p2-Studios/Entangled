using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/OtUKsjPWzO8

// Parent Class for states that can transition to jumping
// Inherits BaseState properties
// Used to abstract transition to jump state away from idle and move (since both use it)
public class Grounded : BaseState {

    protected private PlayerStateMachine playerSM;
    protected private bool triggerRange;
    protected private bool colliderRange;

    // Constructor, sets sm to active stateMachine
    public Grounded(string name, PlayerStateMachine playerStateMachine, Player player) : base(name, playerStateMachine, player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        Player = player;
        triggerRange = false;
        colliderRange = false;
    }

    // Detect any Space input, transition to jumpstate
    public override void UpdateLogic(){
        base.UpdateLogic();
        if (Input.GetKeyDown(KeyCode.Space))
            playerSM.ChangeState(playerSM.jumpState);
    }

    // Detect if player is colliding with objects
    public override void EnterTrigger(Collider2D collider){
        base.EnterTrigger(collider);
        if (collider.gameObject.tag == "Pushable"){
            triggerRange = true;
        }
    }

    public override void EnterCollision(Collision2D collision){
        base.EnterCollision(collision);
        if (collision.gameObject.tag == "Pushable"){
            Debug.Log("HERE");
            colliderRange = true;
        }
    }

    public override void UpdateCollision(Collision2D collision){
        base.UpdateCollision(collision);
        if (collision.gameObject.tag == "Pushable" && colliderRange && triggerRange){
            playerSM.ChangeState(playerSM.pushState);
        }
        else{
            playerSM.ChangeState(playerSM.idleState);
        }
    }

    public override void UpdateTrigger(Collider2D collider){
        base.UpdateTrigger(collider);
        if (collider.gameObject.tag == "Pushable"){
            triggerRange = true;
        }
        else{
            triggerRange = false;
        }
    }

    public override void ExitTrigger(Collider2D collider)
    {
        base.ExitTrigger(collider);
        if (collider.gameObject.tag == "Pushable"){
            triggerRange = false;
        }
    }

    public override void ExitCollision(Collision2D collision){
        base.ExitCollision(collision);
        if (collision.gameObject.tag == "Pushable"){
            colliderRange = false;
        }
    }
}
