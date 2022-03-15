using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSetter : MonoBehaviour {

    private Player player;
    public Transform respawnPoint;

    private void Awake() {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        player.respawnLocation = respawnPoint;
    }
}
