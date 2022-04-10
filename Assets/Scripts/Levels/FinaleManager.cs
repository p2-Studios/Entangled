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
        
        DontDestroyOnLoad(this);
    }
    
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // if the finale manager is brought to a non-level9 scene, destroy it
        if (!scene.name.Contains("Level9")) {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
        Player player = FindObjectOfType<Player>();
        if (player != null && playerPosition != Vector3.zero) {
            player.rigidbody.position = playerPosition;
            ResetPlayerPosition();
        }
    }

    public void SetPlayerPosition(Vector3 pos) {
        playerPosition = pos;
    }

    public void ResetPlayerPosition() {
        playerPosition = Vector3.zero;
    }
    
}
