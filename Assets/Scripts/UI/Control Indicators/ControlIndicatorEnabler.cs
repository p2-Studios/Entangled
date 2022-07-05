using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlIndicatorEnabler : MonoBehaviour {
    void Start() {
        PauseMenu.instance.ToggleControlIndicator(true);
    }

}
