using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Player Object Components/Variables
    public new Rigidbody2D rigidbody;
    public float speed = 6f;
    public float jumpForce = 12f;
    public SpriteRenderer spriteRenderer;

    private EntangleComponent entangleComponent;
    PlayerStateMachine stateMachine;

    private float horzInput;

    private void Start(){
        // Initialize entangleComponent
        entangleComponent = gameObject.AddComponent<EntangleComponent>();
        // Add and initialize PlayerStateMachine
        stateMachine = gameObject.AddComponent<PlayerStateMachine>() as PlayerStateMachine;
        stateMachine.Initialize(this);
    }

    private void Update(){

        // Having input here causes issues with state transitions

        /*horzInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horzInput) > Mathf.Epsilon)
            stateMachine.ChangeState(stateMachine.moveState);

        if (Mathf.Abs(horzInput) < Mathf.Epsilon)
            stateMachine.ChangeState(stateMachine.idleState);

        if(Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(stateMachine.jumpState);
        */
    }

}
