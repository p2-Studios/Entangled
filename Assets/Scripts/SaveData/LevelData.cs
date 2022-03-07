using System;
using UnityEngine;


[Serializable]
public class LevelData {
    
    public string label;
    public FlashDrive[] flashDrives;
    public FlashDrive[] foundFlashDrives;
    public int totalOrbs, orbsFound;

    public LevelData(Level level) {
        label = level.label;
        flashDrives = level.flashDrives;
        foundFlashDrives = level.foundFlashDrives;
        totalOrbs = level.totalOrbs;
        orbsFound = level.orbsFound;
    }
    
}
