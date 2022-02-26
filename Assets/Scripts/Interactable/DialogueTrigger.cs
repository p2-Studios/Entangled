using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Interactable {
    public Dialogue dialogue;
    private DialogueManager dialogueManager;

    public string triggerSound = "";    // the sound to play when the dialogue is triggered
    
    public void Trigger() {
        dialogueManager.StartDialogue(dialogue);
        if (triggerSound.Length != 0) FindObjectOfType<AudioManager>().Play(triggerSound); // play trigger sound
    }

    protected override void Interact() {
        base.Interact();
        if (dialogueManager == null) dialogueManager = FindObjectOfType<DialogueManager>();
        if (!dialogueManager.inDialogue && !dialogueManager.closing) Trigger();
    }
}
