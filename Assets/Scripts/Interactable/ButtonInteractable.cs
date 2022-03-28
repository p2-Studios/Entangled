using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractable : Interactable {

    public ButtonActivator button;
    
    protected override void Interact() {
        base.Interact();
        PlayInteractionSound();
        if (interactionEnabled) {
            button.Activate();
            StartCoroutine(ButtonActivate());
        }
    }
    
    private IEnumerator ButtonActivate() {
        interactionEnabled = false;
        indicator.gameObject.SetActive(false);
        yield return new WaitForSeconds(button.downTime);
        button.down = false;
        interactionEnabled = true;
        button.animator.SetBool("SwitchOn", false);
        button.indicatorLight.color = Color.red;
    }
}
