using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Interactable {
    public Dialogue dialogue;
    private TerminalManager terminalManager;
    public Animator animator;

    public void Trigger() {
        PlayInteractionSound();
        terminalManager.StartDialogue(dialogue);
        animator.SetBool("messageRead", true);
    }

    protected override void Interact() {
        base.Interact();
        if (terminalManager == null) terminalManager = FindObjectOfType<TerminalManager>();
        if (!terminalManager.inTerminal && !terminalManager.closing) {
            Trigger();
        }
    }
}
