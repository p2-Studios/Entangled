using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        // old terminal conversion - convert dialogue to a terminal file
        if (localFiles.Length == 0 && dialogue != null) {
            localFiles = new TerminalFile[1];
            localFiles[0] = ConvertDialogueToFile(dialogue);
            print(localFiles[0].fileName);
        }
        
        if (level == null) {
            Debug.LogWarning("No Level object found - can't load remote terminal messages.");
        } else {
            FlashDrive[] flashDrives = FindObjectsOfType<FlashDrive>();  // find all flashdrives in the scene

            foreach (FlashDrive f in flashDrives) {
                if (f.file != null) {
                    f.file.encrypted = true;    // make sure the file is encrypted
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

    // close terminal when player exits range
    protected override void OnRangeExit() {
        base.OnRangeExit();
        if (terminalManager == null) terminalManager = TerminalManager.instance;
        if (terminalManager.IsTerminalOpen()) terminalManager.CloseTerminal();
    }

    public TextFile ConvertDialogueToFile(Dialogue d) {
        string body = "";
        foreach (string s in d.sentences) {
            body += (s + "\n");
        }

        // create the gameobject and set the file's data
        GameObject g = new GameObject();
        TextFile file = g.AddComponent<TextFile>();
        print(d.name);
        file.name = d.name;
        file.body = body;
        
        return file;
    }
}
