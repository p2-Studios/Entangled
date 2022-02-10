using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorials used: https://youtu.be/-VkezxxjsSE

// StateMachine Parent Class
public class StateMachine : MonoBehaviour{

    BaseState currentState;

    // Initalize states
    void Start(){
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter();
    }

    // Calculate transition logic (state change)
    void Update(){
        if(currentState != null)
            currentState.UpdateLogic();
    }

    // Calculate Physics logic (movement)
    void LateUpdate(){
        if(currentState != null)
            currentState.UpdatePhysics();
    }

    // When changing states, call exit method for current state, change state, them call enter method for new state
    public void ChangeState(BaseState newState){
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // Implemented by children 
    protected virtual BaseState GetInitialState(){
        return null;
    }

    // For testing purposes (displays current state name when running game)
    private void OnGUI(){
        GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
        string content = currentState != null ? currentState.name : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        GUILayout.EndArea();
    }
}
