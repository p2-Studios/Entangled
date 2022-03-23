using System.Collections;
using System.Collections.Generic;
using Game.CustomKeybinds;
using UnityEngine;

public class PushPullState : BaseState {

    protected private PlayerStateMachine playerSM;
    private float horzInput;
    private float objMass;



    // Constructor
    public PushPullState(PlayerStateMachine playerStateMachine,Player player,AudioManager audioManager) : base("Pulling", playerStateMachine, player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        this.audioManager = audioManager;
        base.player = player;
    }

    // Enter calls
    public override void Enter(){
        base.Enter();

        playerSM.player.SetAnimatorState("pushing");

        player.grabbing = true;
        player.pushedObject.GetComponent<FixedJoint2D>().enabled = true;
        player.pushedObject.GetComponent<FixedJoint2D>().connectedBody = player.rigidbody;
        objMass = player.pushedObject.GetComponent<Rigidbody2D>().mass;
        horzInput = 0f;
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");

        // Let go of object
        if (Input.GetKeyDown(Keybinds.GetInstance().grabRelease)){
            player.pushedObject.GetComponent<FixedJoint2D>().enabled = false;
            playerSM.ChangeState(playerSM.idleState);
        }
        if(player.hit.collider == null){
            player.pushedObject.GetComponent<FixedJoint2D>().enabled = false;
            playerSM.ChangeState(playerSM.idleState);
        }
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        float strengthDif = (player.pushStrength - .25f);
        Vector2 velocity = player.rigidbody.velocity;
        float mass = player.pushedObject.GetComponent<Rigidbody2D>().mass;
        velocity.x = horzInput * (player.speed * strengthDif) * (1f/mass);
        player.rigidbody.velocity = velocity;
    }


    // Exit calls (make sure variables don't remain)
    public override void Exit(){
        base.Exit();
        player.grabbing = false;
        player.pushedObject = null;
    }
}


