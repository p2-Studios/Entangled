using System;
using System.Collections;
using Activation_System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Activator = Activation_System.Activator;

public class ButtonActivator : Activator {
    
    public Activatable[] activatables;								// -- array of activatables, REQUIRED to set the activatables manually! --

    public Light2D indicatorLight;

    [HideInInspector]
    public bool down;

    public float activationTime = 0.1f;
    public float downTime = 1.0f;
    
    public Animator animator;
    
    void Start() {
        foreach (Activatable a in activatables) {
            AddActivatable(a);
        }
    }

    public override void Activate() {
        down = true;
        animator.SetBool("SwitchOn", true);
        indicatorLight.color = Color.green;
        ChangeState(true);
        StartCoroutine(ActivateButton());
    }

    public override void Deactivate() {
        ChangeState(false);
    }


    public void ChangeState(bool state) {
        if (state) {     // activate/deactivate after delay
            base.Activate();
        } else {
            base.Deactivate();
        }
    }

    private IEnumerator ActivateButton() {
        yield return new WaitForSeconds(activationTime);
        Deactivate();
    }
}
