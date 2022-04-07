using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {
    public void LoadMainMenu() {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            LoadMainMenu();
        }
    }
}
