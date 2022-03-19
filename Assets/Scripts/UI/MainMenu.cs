using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private void Awake() {
        //StartCoroutine(StartMusicWithDelay());
    }

    public void StartAtLevel(string levelName) {
        SceneManager.LoadSceneAsync(levelName);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OpenFeedbackLink() {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSduizPyNvgwBKM6RKQIJBMhdP-MfVxOmvlQN4bWaZeT_3VL7Q/viewform");
    }
    
    IEnumerator StartMusicWithDelay() {
        yield return new WaitForSeconds(1f);
        
        AudioManager am = FindObjectOfType<AudioManager>();
        if (am != null) am.Play("music_main");
    }
}
