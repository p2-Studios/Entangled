using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : ApplyingForce {

    private float horzInput;
    private Entanglable obj;
    private bool triggerRange;
    private bool colliderRange;
    

    // Constructor
    public PushState(PlayerStateMachine playerStateMachine,Player player) : base("Pushing", playerStateMachine, player){}

    public override void Enter(){
        base.Enter();
        colliderRange = true;
        triggerRange = true;
        horzInput = 0f;
        Player.spriteRenderer.color = Color.blue;  // For testing purposes, will be used later for player animations
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horzInput) < Mathf.Epsilon)
            playerSM.ChangeState(playerSM.idleState);
        if (triggerRange && Input.GetKeyDown(KeyCode.E)){
            playerSM.ChangeState(playerSM.pullState);
        }
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        Vector2 velocity = Player.rigidbody.velocity;
        velocity.x = horzInput * Player.speed;
        Player.rigidbody.velocity = velocity;
        /*
        if(obj != null){
            Vector2 pushForce = new Vector2(velocity.x,0f);
            obj.ApplyVelocity(pushForce);
        }
        */
    }

    // Detect if player is colliding with objects
    public override void UpdateCollision(Collision2D collision){
        base.UpdateCollision(collision);
        if (collision.gameObject.tag == "Pushable" && triggerRange){
            obj = collision.gameObject.GetComponent<Entanglable>();
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

    public override void EnterTrigger(Collider2D collider){
        base.EnterTrigger(collider);
        if (collider.gameObject.tag == "Pushable"){
            triggerRange = true;
        }
    }

    public override void EnterCollision(Collision2D collision){
        base.EnterCollision(collision);
        if (collision.gameObject.tag == "Pushable"){
            colliderRange = true;
        }
    }

    // Detect if player is disconected for triggercollider
    public override void ExitTrigger(Collider2D collider){
        base.ExitTrigger(collider);
        triggerRange = false;
    }

    public override void ExitCollision(Collision2D collision){
        base.ExitCollision(collision);
        colliderRange = false;
    }

    public override void Exit()
    {
        triggerRange = false;
        base.Exit();
    }

}
