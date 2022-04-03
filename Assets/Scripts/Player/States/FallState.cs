using System.Collections;
using System.Collections.Generic;
using Game.CustomKeybinds;
using UnityEngine;

// tutorials used: https://youtu.be/OtUKsjPWzO8

// Player Jumping State
// Currently there is no air pivoting, player remains on trajectory when jumping
// Jump relies on ground layer, meaning that the floor they jump off has to have their layer as ground.
// This helps in State change detection
public class FallState : BaseState {

    private PlayerStateMachine playerSM;

    private bool grounded;
    private int groundLayer = 1 << 6;   // Bitwise shift for ground layer number (should be 6)
    private int objectsLayer = 1 << 9;   // Bitwise shift for ground layer number (should be 6)
    bool jcheck = false;


    private float horzInput;

    private CapsuleCollider2D feetCollider;
    
    protected private bool touchingBox;

    public void Initialize(string name, PlayerStateMachine psm, Player player,AudioManager am){
        this.Name = name;
        playerSM = psm;
        Player = player;
        touchingBox = false;    // checks if player is touching a pushable object
        this.audioManager = am;
        feetCollider = Player.GetComponent<CapsuleCollider2D>();
    }

    /*public FallState(PlayerStateMachine playerStateMachine,Player player, AudioManager audioManager) : base("Jumping", playerStateMachine,player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        touchingBox = false;
        this.audioManager = audioManager;
        feetCollider = Player.GetComponent<CapsuleCollider2D>();
    }*/

    // upon entering state, apply upward velocity to achieve jump
    public override void Enter(){
        base.Enter();
        playerSM.player.SetAnimatorState("falling");
        //audioManager.Play("movement_jump");
        horzInput = 0f;
        jcheck = true;
    }

    void JumpLeniencyCheck(float time){
        StartCoroutine(JumpLeniency(time));
    }

    IEnumerator JumpLeniency(float waitTime){
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(waitTime);
        jcheck = false;
    }


    // Switch states if grounded
    public override void UpdateLogic(){
        base.UpdateLogic();

        // check to see if player is allowed to jump via coroutine
        if(jcheck){
            if(Input.GetKeyDown(Keybinds.GetInstance().jump)){
                playerSM.ChangeState(playerSM.jumpState);
            }
            JumpLeniencyCheck(0.1f);   //passed value is the window of time jumping is allowed
        }
            

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
