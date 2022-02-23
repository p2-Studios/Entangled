using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullState : BaseState {

    protected private PlayerStateMachine playerSM;
    private float horzInput;
    private Entanglable obj;
    private bool pulling;
    

    // Constructor
    public PushPullState(PlayerStateMachine playerStateMachine,Player player) : base("Pulling", playerStateMachine, player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        Player = player;
    }

    // Enter calls
    public override void Enter(){
        base.Enter();
        pulling = true;
        horzInput = 0f;
        Player.spriteRenderer.color = Color.red;  // For testing purposes, will be used later for player animations
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");
        // Jump while push/pull
        if (Input.GetKeyDown(KeyCode.Space)){
            playerSM.ChangeState(playerSM.jumpState);
        }
        // Let go of object
        if (Input.GetKeyDown(KeyCode.E))
            pulling = false;
        // return to idle
        if (!pulling)
            playerSM.ChangeState(playerSM.idleState);
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        Vector2 velocity = Player.rigidbody.velocity;
        velocity.x = horzInput * Player.speed;
        Player.rigidbody.velocity = velocity;
        if(obj != null){
            Vector2 pushForce = new Vector2(velocity.x,0f);
            obj.ApplyForce(pushForce);
        }
    }
    
    // Detect if player is colliding with objects
    public override void UpdateTrigger(Collider2D collider){
        base.UpdateTrigger(collider);
        if (collider.gameObject.tag == "Pushable"){
            obj = collider.gameObject.GetComponent<Entanglable>();
        }
        else{
            obj = null;
        }
    }

    // Detect if player is disconected for triggercollider
    public override void ExitTrigger(Collider2D collider){
        if (collider.gameObject.tag == "Pushable"){
            pulling = false;
        }
    }

    // Exit calls (make sure variables don't remain)
    public override void Exit()
    {
        obj = null;
        base.Exit();
    }
}


