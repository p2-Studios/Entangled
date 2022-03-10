using UnityEngine;
using System.Collections;
 
public class HintNotifier : MonoBehaviour {
    public AnimationCurve myCurve;
 
    void Update() {
        transform.position = new Vector3(transform.position.x, myCurve.Evaluate((Time.time % myCurve.length)) - 2.2f, transform.position.z);
    }
}

