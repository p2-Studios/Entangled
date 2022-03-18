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
        StartCoroutine(switchDelay(true));
    }

    public override void Deactivate() {
        animator.SetBool("SwitchOn", false);
        indicatorLight.color = Color.red;
        StartCoroutine(switchDelay(false));
    }

    // activates/deactivates based on activate bool after short delay to let animation finish
    private IEnumerator switchDelay(bool activate) {
        yield return new WaitForSeconds(0.3f);      // delay to let animation finish
        if (activate) {     // activate/deactivate after delay
            base.Activate();
        }
        else {
            base.Deactivate();
        }
    }

    public override void ToggleState() {
        base.ToggleState();
    }

    // toggling is handled in the Interactable/SwitchInteractable.cs script!
    
    void Update() {
        
    }
}
