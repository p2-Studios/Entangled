using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Author: Dakota
// Handles the logic and animation of the elevator
public class Elevator : MonoBehaviour {
    public string nextLevel;    // the level to load upon using this elevator
    public int nextLevelNum = 0;
    public Animator Animator;   // the animator controlling the open/close animations
    public SpriteRenderer Player;

    private void Start() {
        Player = GameObject.Find("Player").GetComponent<SpriteRenderer>();
    }

    // load the defined next level
    public void LoadNextLevel() {
        StartCoroutine(FadeAndTransition());
    }

    // open the elevator
    public void Open() {
        AudioManager.instance.Play("elevator_open");
        Animator.SetBool("IsOpen", true);
    }

    // close the elevator
    public void Close() {
        AudioManager.instance.Play("elevator_close");
        Animator.SetBool("IsOpen", false);
    }

    public void Exit() {
        Animator.SetTrigger("Exit");
    }

    private IEnumerator FadeAndTransition() {
        yield return new WaitForSeconds(1);
        LevelRestarter.instance.ClearCheckpointPosition();  // clear checkpoint location
        ElevatorTransition.levelToLoad = nextLevel;
        if (SaveSystem.LoadGameData().GetUnlockedLevel() < nextLevelNum) {
            SaveSystem.SetGameDataLevel(nextLevelNum);
        }
        SceneManager.LoadSceneAsync("ElevatorTransition");
    }
}
