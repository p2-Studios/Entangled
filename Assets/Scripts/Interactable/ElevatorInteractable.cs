using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorInteractable : Interactable {
    public Elevator elevator;
    
    protected override void Interact() {
        base.Interact();
        PlayInteractionSound();
        indicator.gameObject.SetActive(false);
        elevator.player.enabled = false;
        elevator.Exit();
        // elevator.LoadNextLevel();
    }

    protected override void OnRangeEnter() {
        base.OnRangeEnter();
        elevator.Open();
    }

    protected override void OnRangeExit() {
        base.OnRangeExit();
        elevator.Close();
    }
}
