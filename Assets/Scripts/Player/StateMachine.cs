using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorial used for statemachine: https://youtu.be/-VkezxxjsSE

public class StateMachine : MonoBehaviour
{
    BaseState currentState;

    void Start(){
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter();
    }

    void Update(){
        if(currentState != null)
            currentState.UpdateLogic();
    }

    void LateUpdate(){
        if(currentState != null)
            currentState.UpdatePhysics();
    }

    public void ChangeState(BaseState newState){
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    protected virtual BaseState GetInitialState(){
        return null;
    }

    private void OnGUI(){
        GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
        string content = currentState != null ? currentState.name : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        GUILayout.EndArea();
    }

}
