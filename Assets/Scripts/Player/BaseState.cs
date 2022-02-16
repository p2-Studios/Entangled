using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// State parent class
// Used for making individual state classes e.g. Idle, Moving
public class BaseState {

    public string name;
    protected PlayerStateMachine playerStateMachine;
    protected Player Player;

    //Constructor, sets name and statemachine
    public BaseState(string name, PlayerStateMachine playerSM, Player player){
        this.name = name;
        this.playerStateMachine = playerSM;
        this.Player = player;
    }

    // methods to be used by children state classes
    public virtual void Enter(){}                               // Runs when first entering a state
    public virtual void UpdateLogic(){}                         // Runs in Update()
    public virtual void UpdatePhysics(){}                       // Runs in lateUpdate()
    public virtual void UpdateCollision(Collision2D collision){}// Runs in collision events
    public virtual void Exit(){}                                // Run when Exiting a state
}
