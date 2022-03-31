using Game.CustomKeybinds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestarter : MonoBehaviour {
    // Start is called before the first frame update

    private bool restarting = false;
    private bool holdingR = false;

    public static LevelRestarter instance;
    public Vector3 checkpointPos;
    private string sceneName = "";   // keep track of the current scene name
    
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (!sceneName.Equals("") && !scene.name.Equals(sceneName)) {    // if loading a different scene, reset checkpoint location
            checkpointPos = Vector3.zero;
        }
        
        if (checkpointPos != Vector3.zero) {    // checkpointPos 
            print("Checkpoint found: " + checkpointPos);
            Player player = FindObjectOfType<Player>();
            if (player != null) {
                player.transform.position = checkpointPos;
                player.respawnLocation = checkpointPos;
                print("Spawning player at " + checkpointPos);
                print("Player's respawnLocation: " + player.respawnLocation);
            }
        }

        sceneName = scene.name; // save the name of the current scene
    }
    
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(Keybinds.GetInstance().reset)) {
            if (!restarting) {
                holdingR = true;
                restarting = true;
                StartCoroutine(Action());
            }
        } if (Input.GetKeyUp(Keybinds.GetInstance().reset)) {
            holdingR = false;
        }
    }

    IEnumerator Action() {
        yield return new WaitForSeconds(2);
        if (holdingR) {
            RestartLevel();
        }

        restarting = false;
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void SetCheckpointPosition(Vector3 pos) {
        checkpointPos = pos;
    }
    
    public void SetCheckpointPosition(Transform t) {
        checkpointPos = t.position;
    }

    public void ClearCheckpointPosition() {
        checkpointPos = Vector3.zero;
    }

    public Vector3 GetCheckpointPosition() {
        return checkpointPos;
    }
}
