using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Player Object Components/Variables
    public new Rigidbody2D rigidbody;
    public float speed = 6f;
    public float jumpForce = 12f;
    public SpriteRenderer spriteRenderer;


    // create states
    private void Start(){
        PlayerStateMachine psm = gameObject.AddComponent<PlayerStateMachine>() as PlayerStateMachine;
        psm.Initialize(this);
    }

}
