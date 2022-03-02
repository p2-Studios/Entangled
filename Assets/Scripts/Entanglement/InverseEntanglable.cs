using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseEntanglable : Entanglable {

    public override void ApplyVelocity(Vector2 vel) {
        velocity = (-1 * vel) / rb.mass;
        velocityUpdate = true;
    }
}
