using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Activation_System;
using Activator = Activation_System.Activator;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class LightToggle : Activatable{


    public Activator[] activators;
    public Light2D lightComponent;
    private bool flickering = false;
    public bool flicker = false;
    private float flickerModifier = 1.25f;
    private float flickerMin = 0.05f;
    private float flickerMax = 0.2f;
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

    private void Update() {
        if (base.activated && flicker && !flickering) {
            StartCoroutine(Flicker());
        }
    }

    IEnumerator Flicker() {
        flickering = true;
        lightComponent.enabled = false;
        float timeDelay = Random.Range(0.01f, 0.1f);
        yield return new WaitForSeconds(timeDelay);
        lightComponent.enabled = true;
        timeDelay = Random.Range(flickerMin, flickerMax);
        flickerMin *= flickerModifier;
        flickerMax *= flickerModifier;
        yield return new WaitForSeconds(timeDelay);
        flickering = false;
    }
}
