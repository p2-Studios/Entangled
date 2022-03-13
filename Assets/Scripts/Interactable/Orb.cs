using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour {
    private LevelDataManager levelDataManager;

    private void Awake() {
        levelDataManager = FindObjectOfType<LevelDataManager>();
        //levelDataManager.addOrb(this);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            CollectOrb();
        }
    }

    private void CollectOrb() {
        gameObject.SetActive(false);
        //levelDataManager.collectOrb(this);
    }
}
