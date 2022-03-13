using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlashDrive : Interactable {
    public string label;
    public string[] texts;

    protected override void Interact() {
        base.Interact();
        CollectFlashDrive();
    }

    private void CollectFlashDrive() {
        this.gameObject.SetActive(false);
    }
}
