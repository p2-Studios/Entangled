using System.Collections;
using System.Collections.Generic;
using Game.CustomKeybinds;
using UnityEngine;

public class PushPullState : BaseState {

    protected private PlayerStateMachine playerSM;
    private float horzInput;
    private float objMass;
    private CapsuleCollider2D feetCollider;

    private int groundLayer = 1 << 6;

    bool gcheck = false;

    public void Initialize(string name, PlayerStateMachine psm, Player player,AudioManager am){
        this.Name = name;
        playerSM = psm;
        Player = player;
        feetCollider = Player.GetComponent<CapsuleCollider2D>();
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

        if (Keybinds.GetInstance().hold) {
            // grab key released when hold is true
            if (Input.GetKeyUp(Keybinds.GetInstance().grabRelease)) {
                Release();
            }
        } else {
            // grab key pressed when hold is false
            if(Input.GetKeyDown(Keybinds.GetInstance().grabRelease)){
                Release();
            }
        }

        if(Player.hit.collider == null){
            Player.pushedObject.GetComponent<FixedJoint2D>().enabled = false;
            playerSM.ChangeState(playerSM.idleState);
        }
        // check for if player is not touching the ground while pushing box
        if(!(feetCollider.IsTouchingLayers(groundLayer))){
            GroundLeniencyCheck(1f);  //change paramater to adjust wait time
        }
        else{
            gcheck = false;
        }
    }

    private void Release() {
        Player.pushedObject.GetComponent<FixedJoint2D>().enabled = false;
        BoxInteractable lastTouchedBox = Player.hit.collider.gameObject.GetComponent<BoxInteractable>();
        if (lastTouchedBox != null) lastTouchedBox.ToggleGrabbingSprite(false);
        playerSM.ChangeState(playerSM.idleState);
    }

    //ground check coroutine
    void GroundLeniencyCheck(float time){
        StartCoroutine(GroundLeniency(time));
        if(gcheck){
            print("detach box");
            Player.pushedObject.GetComponent<FixedJoint2D>().enabled = false;
            playerSM.ChangeState(playerSM.idleState);
        } 
    }

    IEnumerator GroundLeniency(float waitTime){
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(waitTime);
        gcheck = true;
    }

    // Apply velocity to player for movement
    public override void UpdatePhysics(){
        base.UpdatePhysics();
        Vector2 velocity = Player.rigidbody.velocity;
        float mass = Player.pushedObject.GetComponent<Rigidbody2D>().mass;
        float strengthDif = (-Mathf.Log(mass,Player.pushStrength) + 0.75f);
        velocity.x = horzInput * (Player.speed * strengthDif);
        Player.rigidbody.velocity = velocity;
    }


    // Exit calls (make sure variables don't remain)
    public override void Exit(){
        base.Exit();
        Player.pushedObject.GetComponent<FixedJoint2D>().connectedBody = null;
        Player.pushedObject.GetComponent<FixedJoint2D>().enabled = false;
        Player.grabbing = false;
        Player.pushedObject = null;
    }
}


