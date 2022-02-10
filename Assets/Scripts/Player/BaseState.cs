using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// State parent class
// Used for making unique individual e.g. Idle, Moving
public class BaseState {
    public string name;
    
    protected StateMachine stateMachine;

    public BaseState(string name, StateMachine stateMachine){
        this.name = name;
        this.stateMachine = stateMachine;
    }

    // methods to be used by state classes
    public virtual void Enter(){}
    public virtual void UpdateLogic(){}     // Runs in Update() once per frame calculations
    public virtual void UpdatePhysics(){}   // Runs in FixedUpdate() Once per constant time ticks
    public virtual void Exit(){}
}
