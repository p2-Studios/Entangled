using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour {
    public string sceneName;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            Player player = col.GetComponent<Player>();
            if (player != null) {
                FinaleManager.instance.SetPlayerPosition(player.rigidbody.position);
                SceneManager.LoadSceneAsync(sceneName);
            }
        }
    }
}
