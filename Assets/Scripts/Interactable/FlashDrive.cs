using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDrive : Interactable {
    public string label;
    public TerminalFile file;
    public UniqueId UID;
    private Level level;

    private void Awake() {
        level = FindObjectOfType<Level>();
        if (file == null) {
            Debug.LogWarning("Flash drive has no file, destroying!");
            Destroy(gameObject);
        }
    }

    public void Collect() {
        level.FlashDriveFound(this); 
    }

    public string GetID() {
        return UID.uniqueId;
    }
    
    protected override void Interact() {
        base.Interact();
        Collect();
    }
}
