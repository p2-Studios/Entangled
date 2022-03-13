
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    public string label;
    public FlashDrive[] flashDrives, foundFlashDrives;
    public Orb[] orbs, orbsFound;

    public void Awake() {
        LoadLevelData();
    }

    private void OnDestroy() {
        SaveLevelData();
    }

    public void LoadLevelData() {
        LevelData data = SaveSystem.LoadLevel(label);
        if (data == null) return;

        foundFlashDrives = data.foundFlashDrives;
        orbsFound = data.orbsFound;
    }

    public void SaveLevelData() {
        SaveSystem.SaveLevel(this);
    }
}
