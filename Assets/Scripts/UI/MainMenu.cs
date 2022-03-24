using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private void Awake() {
        StartCoroutine(StartMusicWithDelay());
    }

    public void StartAtLevel(string levelName) {
        SceneManager.LoadSceneAsync(levelName);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

    IEnumerator StartMusicWithDelay() {
        yield return new WaitForSeconds(1f);
        
        AudioManager am = FindObjectOfType<AudioManager>();
        if (am != null) am.Play("music_main");
    }
}
