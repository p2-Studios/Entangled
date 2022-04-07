using System;
using UnityEngine;


[Serializable]
public class LevelData {
    
    public string label;
    public string[] foundFlashDrives;
    public string[] foundOrbs;

    public LevelData(Level level) {
        label = level.label;
        foundFlashDrives = level.GetFoundFlashDriveIDs();
        foundOrbs = level.GetCollectedOrbIDS();
    }
    
}
