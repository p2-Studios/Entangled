using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinaleManager : MonoBehaviour {
    
    public static FinaleManager instance;
    public static Vector3 playerPosition = Vector3.zero;
    
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
        Player player = FindObjectOfType<Player>();
        if (player != null && playerPosition != Vector3.zero) {
            player.rigidbody.position = playerPosition;
            print("Spawning player at " + playerPosition);
        }
    }

    public void SetPlayerPosition(Vector3 pos) {
        playerPosition = pos;
        print("Set player position to " + playerPosition);
    }
    
}
