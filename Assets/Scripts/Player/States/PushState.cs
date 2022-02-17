using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : ApplyingForce {

    private float horzInput;
    private Entanglable obj;
    private bool pushing;
    

    // Constructor
    public PushState(PlayerStateMachine playerStateMachine,Player player) : base("Pushing", playerStateMachine, player){}

    public override void Enter(){
        base.Enter();
        pushing = true;
        horzInput = 0f;
        Player.spriteRenderer.color = Color.blue;  // For testing purposes, will be used later for player animations
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");
        if (!pushing || Mathf.Abs(horzInput) < Mathf.Epsilon)
            playerSM.ChangeState(playerSM.idleState);
        if (Input.GetKeyDown(KeyCode.E)){
            playerSM.ChangeState(playerSM.pullState);
        }
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        Vector2 velocity = Player.rigidbody.velocity;
        velocity.x = horzInput * Player.speed;
        Player.rigidbody.velocity = velocity;
        if(obj != null){
            Vector2 pushForce = new Vector2(velocity.x,0f);
            obj.ApplyForce(pushForce*.01f);
        }
    }

    // Detect if player is colliding with objects
    public override void UpdateCollision(Collider2D collider){
        base.UpdateCollision(collider);
        if (collider.gameObject.tag == "Pushable"){
            obj = collider.gameObject.GetComponent<Entanglable>();
        }
    }

    // Detect if player is disconected for triggercollider
    public override void ExitCollision(Collider2D collider){
        base.ExitCollision(collider);
        if (collider.gameObject.tag == "Pushable"){
            pushing = false;
        }
    }

    public override void Exit()
    {
        pushing = false;
        base.Exit();
    }

}
