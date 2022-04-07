using System;
using UnityEngine;


[Serializable]
public class LevelData {
    
    public string label;
    public int totalFlashDrives;
    public string[] foundFlashDrives;
    //public string[] foundOrbs;

    public LevelData(Level level) {
        label = level.label;
        totalFlashDrives = level.GetTotalFlashDriveCount();
        foundFlashDrives = level.GetFoundFlashDriveIDs();
        //foundOrbs = level.GetCollectedOrbIDS();
    }
    
}
