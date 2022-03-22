using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : Interactable {
    [SerializeField]
    public TerminalFile[] localFiles, remoteFiles;
    public Dialogue dialogue;
    private TerminalManager terminalManager;
    public Animator animator;


    public TerminalFile[] GetLocalFiles() {
        return localFiles;
    }

    public TerminalFile[] GetRemoteFiles() {
        return remoteFiles;
    }
    
    
    public void Trigger() {
        PlayInteractionSound();
        terminalManager.OpenTerminal(this);
        //animator.SetBool("messageRead", true);
    }

    protected override void Interact() {
        base.Interact();
        if (terminalManager == null) terminalManager = TerminalManager.instance;
        if (!terminalManager.IsTerminalOpen()) {
            Trigger();
        }
    }
}
