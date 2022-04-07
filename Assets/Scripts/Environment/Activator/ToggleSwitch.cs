using System;
using System.Collections;
using Activation_System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Activator = Activation_System.Activator;

public class ToggleSwitch : Activator {
    
    public Activatable[] activatables;								// -- array of activatables, REQUIRED to set the activatables manually! --

    public Light2D indicatorLight;

    public Animator animator;
    
    void Start() {
        foreach (Activatable a in activatables) {
            AddActivatable(a);
        }
    }

    public override void Activate() {
        animator.SetBool("SwitchOn", true);
        indicatorLight.color = Color.green;
        ChangeState(true);
    }

    public override void Deactivate() {
        animator.SetBool("SwitchOn", false);
        indicatorLight.color = Color.red;
        ChangeState(false);
    }


    public void ChangeState(bool state) {
        if (state) {     // activate/deactivate after delay
            base.Activate();
        } else {
            base.Deactivate();
        }
    }
    
    public override void ToggleState() {
        base.ToggleState();
    }

    // toggling is handled in the Interactable/SwitchInteractable.cs script!
}
