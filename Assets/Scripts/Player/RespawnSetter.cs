using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnSetter : MonoBehaviour {

    public static Transform resetLocation;
    private Player player;
    public Transform respawnPoint;

    private void Awake() {
        player = FindObjectOfType<Player>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (resetLocation != null) {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            player.respawnLocation = respawnPoint;
            resetLocation = respawnPoint;
        }
    }
}
