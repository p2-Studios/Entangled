using System.Collections;
using System.Collections.Generic;
using Game.CustomKeybinds;
using UnityEngine;

// tutorials used: https://youtu.be/OtUKsjPWzO8

// Parent Class for Idle and Move states
// Inherits BaseState properties
// Used to abstract transition jump and collision states changes away from children
public class Grounded : BaseState {

    protected private PlayerStateMachine playerSM;
    protected private float horzInput;
    protected private bool touchingBox;
    protected private bool haltMovement;

    protected private BoxInteractable lastTouchedBox;

    // Constructor, sets sm to active stateMachine
    public Grounded(string name, PlayerStateMachine playerStateMachine, Player player) : base(name, playerStateMachine, player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        base.player = player;
        touchingBox = false;    // checks if player is touching a pushable object
        haltMovement = false;   // checks if player should be allowed to move
    }

    // Update Logic changes (key pressses)
    public override void UpdateLogic(){
        base.UpdateLogic();

        checkNonPushingCollision();

        // check if player can latch onto object      
        if (player.hit.collider != null && player.hit.collider.gameObject.tag == "Pushable" ){
            lastTouchedBox = player.hit.collider.gameObject.GetComponent<BoxInteractable>();
            lastTouchedBox.toggleIndicator(true);
            if(Input.GetKeyDown(Keybinds.GetInstance().grabRelease)){
                lastTouchedBox.toggleSprite(true);
                player.pushedObject = player.hit.collider.gameObject.GetComponent<Entanglable>();
                playerSM.ChangeState(playerSM.pushpullState);
            }
        }
        else{
            if(lastTouchedBox != null){
                lastTouchedBox.toggleSprite(false);
                lastTouchedBox.toggleIndicator(false);
            }
        }

        // jump
        if (!haltMovement && Input.GetKeyDown(Keybinds.GetInstance().jump) && player.IsGrounded())
            playerSM.ChangeState(playerSM.jumpState);

        
    }


    // checks to see if player is colliding with a box while not actively
    // in pushpull state (should halt player movement)
    public void checkNonPushingCollision(){
         if (player.hit.collider != null && player.hit.collider.gameObject.tag == "Pushable" && touchingBox){
            haltMovement = true;
            player.rigidbody.velocity = Vector2.zero;
         } else {
            haltMovement = false;
         }
    }

    // Checks when player enters trigger collider
    public override void EnterTrigger(Collider2D collider){
        base.EnterTrigger(collider);
        if(collider.gameObject.tag == "Pushable"){
            touchingBox = true;
        }
    }

    // Updates while player remains within trigger collider
    public override void UpdateTrigger(Collider2D collider){
        base.UpdateTrigger(collider);
    }

    // Checks when player exits trigger collider
    public override void ExitTrigger(Collider2D collider){
        base.ExitTrigger(collider);
        if(collider.gameObject.tag == "Pushable"){
            touchingBox = false;
        }
    }



}
