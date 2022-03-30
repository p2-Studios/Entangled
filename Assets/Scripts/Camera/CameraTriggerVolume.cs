using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

// tutorial used: https://www.youtube.com/watch?v=nkL-VvMmWHg

// Script is attached to gameobject with rigibody and trigger collider
// Uses attached trigger field to detect if attached vcam should be switched too
public class CameraTriggerVolume : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private Vector2 boxSize;

    BoxCollider2D box;
    Rigidbody2D rb;

    // gets attached components and initializes
    private void Awake(){
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        box.isTrigger = true;
        box.size = boxSize;
        rb.isKinematic = true;
    }

    // detect trigger enters
    public void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player")){
            if (CameraSwitcher.ActiveCamera != cam){
                CameraSwitcher.SwitchCamera(cam);
                cam.Priority = 100; // set high priority so that the camera doesn't start at the initial camera when resetting level
            }
        }
    }

    public void OnTriggerStay2D(Collider2D other){
        if (other.gameObject.CompareTag("Player")){
            if (CameraSwitcher.ActiveCamera != cam){
                CameraSwitcher.SwitchCamera(cam);
            }
        }
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
