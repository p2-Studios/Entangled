using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/OtUKsjPWzO8

// Player Jumping State
// Currently there is no air pivoting, player remains on trajectory when jumping
// Jump relies on ground layer, meaning that the floor they jump off has to have their layer as ground.
// This helps in State change detection
public class JumpState : BaseState {

    private PlayerStateMachine playerSM;

    private bool grounded;
    private int groundLayer = 1 << 6;   // Bitwise shift for ground layer number (should be 6)
    private int objectsLayer = 1 << 9;   // Bitwise shift for ground layer number (should be 6)

    private float horzInput;

    private CapsuleCollider2D feetCollider;
    
    protected private bool touchingBox;

    public JumpState(PlayerStateMachine playerStateMachine,Player player, AudioManager audioManager) : base("Jumping", playerStateMachine,player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        touchingBox = false;
        this.audioManager = audioManager;
        feetCollider = Player.GetComponent<CapsuleCollider2D>();
    }

    // upon entering state, apply upward velocity to achieve jump
    public override void Enter(){
        base.Enter();
        
        playerSM.player.SetAnimatorState("jumping");
        //audioManager.Play("movement_jump");

        horzInput = 0f;
        Vector2 velocity = Player.rigidbody.velocity;
        velocity.y += Player.jumpForce;
        velocity.x = 0;
        Player.rigidbody.velocity = velocity;
    }

    // Switch states if grounded
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");
        if(grounded)
            playerSM.ChangeState(playerSM.idleState);
    }

    // Check if player velocity is less than epsilon and rigidbody is touching a ground layer
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        
        grounded = Player.rigidbody.velocity.y < Mathf.Epsilon && (feetCollider.IsTouchingLayers(groundLayer) || feetCollider.IsTouchingLayers(objectsLayer));
        if(!touchingBox){
            Vector2 velocity = Player.rigidbody.velocity;
            velocity.x = horzInput * Player.speed / 1.5f;
            Player.rigidbody.velocity = velocity;
        }
    }

    public override void EnterTrigger(Collider2D collider)
    {
        base.EnterTrigger(collider);
        if(collider.gameObject.tag == "Pushable"){
            touchingBox = true;
        }
    }


    public override void Exit()
    {
        base.Exit();
        touchingBox = false;
    }

}
