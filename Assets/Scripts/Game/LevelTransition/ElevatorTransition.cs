using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorTransition : MonoBehaviour {

    public static string levelToLoad = "MainMenu";
    
    public Transform endPos;
    public float speed = 1.0f;

    private bool atEnd = false;

    private void Awake() {
        FindObjectOfType<AudioManager>().Play("elevator_descend");
    }

    void Update() {
        transform.position = Vector2.MoveTowards(transform.position, endPos.position, speed * Time.deltaTime);
        
        if (!atEnd && transform.position == endPos.position) {
            atEnd = true;
            LoadNextScene();
        }
    }

    private void LoadNextScene() {
        Debug.Log(levelToLoad);
        Scene scene = SceneManager.GetSceneByName(levelToLoad);
        SceneManager.LoadSceneAsync(levelToLoad);
        if (!scene.IsValid()) { // check if scene was found
            SceneManager.LoadSceneAsync(levelToLoad);
        } else {
            Debug.LogWarning("Scene " + levelToLoad + " attempted to load, but wasn't found!");
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
