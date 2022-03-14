using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDrive : Interactable {
    public string label;
    public string[] texts;
    public UniqueId UID;

    public void Collect() {
        FindObjectOfType<Level>().FlashDriveFound(this); 
    }

    public string GetID() {
        return UID.uniqueId;
    }
    
    protected override void Interact() {
        base.Interact();
        Collect();
    }
}
