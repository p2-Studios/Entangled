using System;
using UnityEngine;


[Serializable]
public class LevelData {
    
    public string label;
    public FlashDrive[] flashDrives;
    public FlashDrive[] foundFlashDrives;
    public Orb[] orbs;
    public Orb[] orbsFound;

    public LevelData(Level level) {
        label = level.label;
        flashDrives = level.flashDrives;
        foundFlashDrives = level.foundFlashDrives;
        orbs = level.orbs;
        orbsFound = level.orbsFound;
    }
    
}
