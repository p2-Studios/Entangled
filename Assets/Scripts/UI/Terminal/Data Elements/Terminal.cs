using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : Interactable {
    [SerializeField]
    public TerminalFile[] files;
    public Dialogue dialogue;
    private TerminalManager terminalManager;
    public Animator animator;


    public TerminalFile[] GetFiles() {
        return files;
    }
    
    
    public void Trigger() {
        PlayInteractionSound();
        terminalManager.OpenTerminal(this);
        //animator.SetBool("messageRead", true);
    }

    protected override void Interact() {
        base.Interact();
        if (terminalManager == null) terminalManager = TerminalManager.instance;
        if (!terminalManager.inTerminal && !terminalManager.closing) {
            Trigger();
        }
    }
}
