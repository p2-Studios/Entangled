using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string firstLevel;
    public void PlayGame() {
        SceneManager.LoadSceneAsync(firstLevel);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
