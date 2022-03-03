using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string firstLevel;

    private void Awake() {
        StartCoroutine(StartMusicWithDelay());
    }

    public void PlayGame() {
        SceneManager.LoadSceneAsync(firstLevel);
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
