using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInteractable : Interactable {

    public ToggleSwitch toggleSwitch;
    
    protected override void Interact() {
        base.Interact();
        PlayInteractionSound();
        if (toggleSwitch.IsActivated()) {
            toggleSwitch.Deactivate();
        } else {
            toggleSwitch.Activate();
        }
    }
}
