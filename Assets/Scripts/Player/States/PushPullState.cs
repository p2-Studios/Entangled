using System.Collections;
using System.Collections.Generic;
using Game.CustomKeybinds;
using UnityEngine;

public class PushPullState : BaseState {

    protected private PlayerStateMachine playerSM;
    private float horzInput;
    private float objMass;

    public void Initialize(string name, PlayerStateMachine psm, Player player,AudioManager am){
        this.Name = name;
        playerSM = psm;
        Player = player;
        this.audioManager = am;
    }

    // Constructor
    /*public PushPullState(PlayerStateMachine playerStateMachine,Player player,AudioManager audioManager) : base("Pulling", playerStateMachine, player){
        playerSM = (PlayerStateMachine)playerStateMachine;
        this.audioManager = audioManager;
        Player = player;
    }*/

    // Enter calls
    public override void Enter(){
        base.Enter();

        playerSM.player.SetAnimatorState("pushing");

        Player.grabbing = true;
        Player.pushedObject.GetComponent<FixedJoint2D>().enabled = true;
        Player.pushedObject.GetComponent<FixedJoint2D>().connectedBody = Player.rigidbody;
        objMass = Player.pushedObject.GetComponent<Rigidbody2D>().mass;
        horzInput = 0f;
    }

    // Detect if horizontal input less than Epsilon (switch to idle if true)
    public override void UpdateLogic(){
        base.UpdateLogic();
        horzInput = Input.GetAxis("Horizontal");

        // Let go of object
        if (Input.GetKeyUp(Keybinds.GetInstance().grabRelease)){
            Player.pushedObject.GetComponent<FixedJoint2D>().enabled = false;
            BoxInteractable lastTouchedBox = Player.hit.collider.gameObject.GetComponent<BoxInteractable>();
            if (lastTouchedBox != null) lastTouchedBox.ToggleGrabbingSprite(false);
            playerSM.ChangeState(playerSM.idleState);
        }
        if(Player.hit.collider == null){
            Player.pushedObject.GetComponent<FixedJoint2D>().enabled = false;
            playerSM.ChangeState(playerSM.idleState);
        }
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        float strengthDif = (Player.pushStrength - .25f);
        Vector2 velocity = Player.rigidbody.velocity;
        float mass = Player.pushedObject.GetComponent<Rigidbody2D>().mass;
        velocity.x = horzInput * (Player.speed * strengthDif) * (1f/mass);
        Player.rigidbody.velocity = velocity;
    }


    // Exit calls (make sure variables don't remain)
    public override void Exit(){
        base.Exit();
        Player.grabbing = false;
        Player.pushedObject.GetComponent<FixedJoint2D>().enabled = false;
        Player.pushedObject = null;
    }
}


