using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {
    public GameObject start;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public Vector3 GetStartLocation() {
        return start.transform.position;
    }
}
