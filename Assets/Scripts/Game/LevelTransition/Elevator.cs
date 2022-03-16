using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Author: Dakota
// Handles the logic and animation of the elevator
public class Elevator : MonoBehaviour {
    public string nextLevel;    // the level to load upon using this elevator
    public Animator Animator;   // the animator controlling the open/close animations

    // load the defined next level
    public void LoadNextLevel() {
        ElevatorTransition.levelToLoad = nextLevel;
        SceneManager.LoadSceneAsync("ElevatorTransition");
    }

    // open the elevator
    public void Open() {
        Animator.SetBool("IsOpen", true);
    }

    // close the elevator
    public void Close() {
        Animator.SetBool("IsOpen", false);
    }
}