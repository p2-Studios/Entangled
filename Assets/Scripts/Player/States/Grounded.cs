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
    private CapsuleCollider2D feetCollider;

    protected private BoxInteractable lastTouchedBox;

    private int groundLayer = 1 << 6;   // Bitwise shift for ground layer number (should be 6)
    private int objectsLayer = 1 << 9; 

    public void Initialize(string name, PlayerStateMachine psm, Player player,AudioManager am){
        this.Name = name;
        playerSM = psm;
        Player = player;
        touchingBox = false;    // checks if player is touching a pushable object
        haltMovement = false;   // checks if player should be allowed to move
        feetCollider = Player.GetComponent<CapsuleCollider2D>();
        this.audioManager = am;
    }

    // Constructor, sets sm to active stateMachine
    /*public Grounded(string name, PlayerStateMachine playerStateMachine, Player player) : base(name, playerStateMachine, player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        Player = player;
        touchingBox = false;    // checks if player is touching a pushable object
        haltMovement = false;   // checks if player should be allowed to move
        feetCollider = Player.GetComponent<CapsuleCollider2D>();
    }*/

    // Update Logic changes (key pressses)
    public override void UpdateLogic(){
        base.UpdateLogic();

        checkNonPushingCollision();

        // check if player can latch onto object      
        if (Player.hit.collider != null && Player.hit.collider.gameObject.tag == "Pushable" ){
            BoxInteractable touchedBox = Player.hit.collider.gameObject.GetComponent<BoxInteractable>();
            touchedBox.ToggleControlSprite(true);
            
            if (lastTouchedBox != null && !touchedBox.Equals(lastTouchedBox)) {   // if now looking at a differernt box, disable control sprite of old box
                lastTouchedBox.ToggleControlSprite(false);
            }

            lastTouchedBox = touchedBox;
            
            if(Input.GetKeyDown(Keybinds.GetInstance().grabRelease)){
                lastTouchedBox.ToggleGrabbingSprite(true);
                Player.pushedObject = Player.hit.collider.gameObject.GetComponent<Entanglable>();
                playerSM.ChangeState(playerSM.pushpullState);
            }
        }
        else {
            if (lastTouchedBox != null){
                lastTouchedBox.ToggleGrabbingSprite(false);
                lastTouchedBox.ToggleControlSprite(false);
            }
        }

        // jump
        if (!haltMovement && Input.GetKeyDown(Keybinds.GetInstance().jump))
            playerSM.ChangeState(playerSM.jumpState);

        if(!(feetCollider.IsTouchingLayers(groundLayer) || feetCollider.IsTouchingLayers(objectsLayer))){
            playerSM.ChangeState(playerSM.fallState); 
        }
    }

    // checks to see if player is colliding with a box while not actively
    // in pushpull state (should halt player movement)
    public void checkNonPushingCollision(){
         if (Player.hit.collider != null && Player.hit.collider.gameObject.tag == "Pushable" && touchingBox){
            haltMovement = true;
            Player.rigidbody.velocity = Vector2.zero;
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
