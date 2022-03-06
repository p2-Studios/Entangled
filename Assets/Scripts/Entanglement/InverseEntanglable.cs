using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseEntanglable : Entanglable {

    public override void ApplyVelocity(Vector2 vel, bool fromActive) {
        if (fromActive) {
            velocity += (-1 * vel) / rb.mass;
            velocityUpdate = true;
        } else {
            velocity += vel / rb.mass;
            velocityUpdate = true;
        }
    }
}
