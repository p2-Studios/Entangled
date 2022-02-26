using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Interactable {
    public Dialogue dialogue;
    private DialogueManager dialogueManager;
    
    public void Trigger() {
        dialogueManager.StartDialogue(dialogue);
    }

    protected override void Interact() {
        base.Interact();
        if (dialogueManager == null) dialogueManager = FindObjectOfType<DialogueManager>();
        if (!dialogueManager.inDialogue && !dialogueManager.closing) Trigger();
    }
}
