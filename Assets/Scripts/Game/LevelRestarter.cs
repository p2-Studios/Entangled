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
        if (checkpointPos != Vector3.zero) {
            Player player = FindObjectOfType<Player>();
            if (player != null) {
                player.transform.position = checkpointPos;
            }
        }
    }
    
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            if (!restarting) {
                holdingR = true;
                restarting = true;
                StartCoroutine(Action());
            }
        } if (Input.GetKeyUp(KeyCode.R)) {
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
        AudioManager.instance.restartSong = false;  // don't restart song when restarting level
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
}
