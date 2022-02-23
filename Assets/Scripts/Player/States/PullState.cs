using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullState : ApplyingForce {

    private float horzInput;
    private Entanglable obj;
    private bool pulling;
    

    // Constructor
    public PullState(PlayerStateMachine playerStateMachine,Player player) : base("Pulling", playerStateMachine, player){}

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
        if (Input.GetKeyDown(KeyCode.E)){
            pulling = false;
        }
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
            obj.ApplyVelocity(pushForce);
        }
    }
    
    // Detect if player is colliding with objects
    public override void UpdateTrigger(Collider2D collider){
        base.UpdateTrigger(collider);
        if (collider.gameObject.tag == "Pushable"){
            obj = collider.gameObject.GetComponent<Entanglable>();
        }
    }

    // Detect if player is disconected for triggercollider
    public override void ExitTrigger(Collider2D collider){
        if (collider.gameObject.tag == "Pushable"){
            pulling = false;
        }
    }


    public override void Exit()
    {
        pulling = false;
        base.Exit();
    }
}


