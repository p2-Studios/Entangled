using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData {
    public int unlockedLevel = 1;

    public GameData(int unlockedLevel) {
        this.unlockedLevel = unlockedLevel;
    }

    public int GetUnlockedLevel() {
        return unlockedLevel;
    }
}

