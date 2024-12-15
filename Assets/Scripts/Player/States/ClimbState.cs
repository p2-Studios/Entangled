using System.Collections;
using System.Collections.Generic;
using Game.CustomKeybinds;
using UnityEngine;

// tutorials used: https://youtu.be/OtUKsjPWzO8

// Player Jumping State
// Currently there is no air pivoting, player remains on trajectory when jumping
// Jump relies on ground layer, meaning that the floor they jump off has to have their layer as ground.
// This helps in State change detection
public class ClimbState : BaseState {

    private PlayerStateMachine playerSM;

    private bool grounded, onClimable;
    Ladder climable;
    private int groundLayer = 1 << 6;   // Bitwise shift for ground layer number (should be 6)
    private int objectsLayer = 1 << 9;   // Bitwise shift for ground layer number (should be 6)
    protected private bool haltMovement = false;
    private float horzInput;
    private float vertInput;

    private CapsuleCollider2D feetCollider;

    public void Initialize(string name, PlayerStateMachine psm, Player player, AudioManager am) {
        this.Name = name;
        playerSM = psm;
        Player = player;
        onClimable = false;
        feetCollider = Player.GetComponent<CapsuleCollider2D>();
        this.audioManager = am;
    }

    public override void Enter() {
        base.Enter();
        Debug.Log("entering climbstate");
        playerSM.player.SetAnimatorState("jumping");
        // place player in middle of ladder
        Vector3 loc = Player.transform.position;
        // Vector3 climableLoc = climable.GetStartLocation();
        // Player.transform.position = new Vector3(climableLoc.x, loc.y, loc.z);
        //audioManager.Play("movement_jump");
        Vector2 velocity = Player.rigidbody.velocity;
        velocity.y = Player.jumpForce;
        velocity.x = 0;
        Player.rigidbody.gravityScale = 0f;
    }

    // Switch states if grounded
    public override void UpdateLogic() {
        base.UpdateLogic();

        if (grounded) {
            playerSM.ChangeState(playerSM.idleState);
        }

        if (!haltMovement && Input.GetKeyDown(Keybinds.GetInstance().jump)) {
            playerSM.ChangeState(playerSM.jumpState);
        }

        vertInput = Input.GetAxis("Vertical");
        horzInput = Input.GetAxis("Horizontal");
    }

    // Check if player velocity is less than epsilon and rigidbody is touching a ground layer
    public override void UpdatePhysics() {
        base.UpdatePhysics();
        // grounded = Player.rigidbody.velocity.y < 0.1f && (feetCollider.IsTouchingLayers(groundLayer) || feetCollider.IsTouchingLayers(objectsLayer));
        Vector2 velocity = Player.rigidbody.velocity;
        velocity.y = vertInput * Player.speed * 0.5f;
        velocity.x = horzInput * Player.speed * 0.25f;
        Player.rigidbody.velocity = velocity;
    }

    public override void Exit() {
        Debug.Log("exiting climb state");
        Player.rigidbody.gravityScale = 1f;
        base.Exit();
    }

    public override void EnterTrigger(Collider2D collider)
    {
        base.EnterTrigger(collider);
        // if (collider.CompareTag("Climbable")) {
        //     climable = collider.gameObject.GetComponent<Ladder>();
        // }
    }

    public override void ExitTrigger(Collider2D collider) {
        base.ExitTrigger(collider);
        if (collider.gameObject.tag == "Climable") {
            playerSM.ChangeState(playerSM.fallState);
        }
    }

}
