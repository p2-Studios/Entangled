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
        StartCoroutine(StartSoundWithDelay());
    }
    
    IEnumerator StartSoundWithDelay() {
        yield return new WaitForSeconds(0.5f);
        
        //AudioManager am = FindObjectOfType<AudioManager>();
        //if (am != null) am.PlayDelayed("elevator_descend", 3f);
    }

    void Update() {
        transform.position = Vector2.MoveTowards(transform.position, endPos.position, speed * Time.deltaTime);
        
        if (!atEnd && transform.position == endPos.position) {
            atEnd = true;
            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene() {
        FadeManager.instance.FadeOut();
        yield return new WaitForSeconds(1f);
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
