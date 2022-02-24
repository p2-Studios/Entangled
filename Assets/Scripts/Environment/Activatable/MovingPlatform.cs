using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingPlatform : MonoBehaviour {
    // Start is called before the first frame update

    public Transform posStart, posEnd;
    public float speed;
    public Transform startPos;

    private Vector2 nextPos;
    void Start() {
        nextPos = startPos.position;
    }

    // Update is called once per frame
    void Update() {
        if (transform.position == posStart.position) {
            Debug.Log("at start pos");
            nextPos = posEnd.position;
        } 
        
        if (transform.position == posEnd.position) {
            Debug.Log("at end pos");
            nextPos = posStart.position;
        }

        transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(posStart.position, posEnd.position);
    }
}
