using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseEntanglable : Entanglable {

    public override void ApplyVelocity(Vector2 vel, bool fromActive) {
        if (fromActive) {
            newActiveVelocity = (-1 * vel) / rb.mass;
            velocityUpdate = true;
        } else {
            newActiveVelocity = vel / rb.mass;
            velocityUpdate = true;
        }
    }
}
