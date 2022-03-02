using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorInteractable : Interactable {
    public Elevator elevator;
    
    protected override void Interact() {
        base.Interact();
        PlayInteractionSound();
        elevator.LoadNextLevel();
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
