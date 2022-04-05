using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Floater : MonoBehaviour {
    public float distance = 1.0f;
    public float speed = 0.01f;
    private Vector3 posStart, posEnd, nextPos;

    private void Awake() {
        Vector3 pos = transform.position;
        posStart = new Vector3(pos.x, pos.y + distance, pos.z);
        posEnd = new Vector3(pos.x, pos.y - distance, pos.z);

        transform.position = new Vector3(pos.x,
            pos.y + Random.Range(distance * -1f, distance));
        
        nextPos = posEnd;
    }

    void Update() {
            if (transform.position == posStart) {
                nextPos = posEnd;
            }
            if (transform.position == posEnd) {
                nextPos = posStart;
            }
            transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    } 
}

