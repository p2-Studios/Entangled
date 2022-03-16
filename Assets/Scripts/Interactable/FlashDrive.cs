using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDrive : Interactable {
    public string label;
    public string[] texts;
    public UniqueId UID;
    private Level level;

    private void Awake() {
        level = FindObjectOfType<Level>();
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
