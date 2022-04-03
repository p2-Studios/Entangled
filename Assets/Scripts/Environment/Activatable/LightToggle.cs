using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Activation_System;
using Activator = Activation_System.Activator;
using UnityEngine.Experimental.Rendering.Universal;

public class LightToggle : Activatable{


    public Activator[] activators;
    public Light2D lightComponent;

    void Start() {
        if (lightComponent == null) lightComponent = GetComponent<Light2D>();
        foreach (Activator a in activators) {
            AddActivator(a);
        }

        if (activateByDefault) {
            playSound = false;
            Activate();
        } else { Deactivate();}
    }

    public override void Activate(){
        base.Activate();
        lightComponent.enabled = true;
    }

    public override void Deactivate(){
        base.Deactivate();
        lightComponent.enabled = false;
    }
}
