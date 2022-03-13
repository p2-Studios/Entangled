using System;
using UnityEngine;
using System.Collections;
using Random = System.Random;

public class Floater : MonoBehaviour {
    public AnimationCurve myCurve;

    public float distance = 1.0f;
    public float speed = 0.01f;
    private Vector3 posStart, posEnd, nextPos;

    private void Awake() {
        Vector3 pos = transform.position;
        posStart = new Vector3(pos.x, pos.y + distance, pos.z);
        posEnd = new Vector3(pos.x, pos.y - distance, pos.z);

        nextPos = posEnd;
    }

    void Update() {
            if (transform.position == posStart) {
                nextPos = posEnd;
            } else if (transform.position == posEnd) {
                nextPos = posStart;
            }
            transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    } 
}

