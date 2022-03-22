using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : Interactable {
    [SerializeField]
    public TerminalFile[] localFiles;

    private ArrayList remoteFiles;
    public Dialogue dialogue;
    private TerminalManager terminalManager;
    public Animator animator;


    private void Awake() {
        remoteFiles = new ArrayList();
        Level level = FindObjectOfType<Level>();
        if (level == null) {
            Debug.LogWarning("No Level object found - can't load remote terminal messages.");
        } else {
            FlashDrive[] flashDrives = FindObjectsOfType<FlashDrive>();  // find all flashdrives in the scene
            foreach (FlashDrive f in flashDrives) {
                if (f.file != null) {
                    remoteFiles.Add(f.file);
                }
            }
        }
    }

    public TerminalFile[] GetLocalFiles() {
        return localFiles;
    }

    public ArrayList GetRemoteFiles() {
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
