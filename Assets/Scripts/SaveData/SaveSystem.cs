using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem {

    public static void SaveLevel(Level level) {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/" + level.label + ".level";
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(level);
        
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LevelData LoadLevel(string label) {
        string path = Application.persistentDataPath + "/" + label + ".level";

        if (!File.Exists(path)) {
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        LevelData data = formatter.Deserialize(stream) as LevelData;
        
        stream.Close();

        return data;
    }

    // overwrites the saved game data with the given GameData
    public static void SaveGameData(GameData data) {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/gamedata.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, data);
        stream.Close();
    }
    
    // retrieves the game data, if available. If not available, creates a default GameData
    public static GameData LoadGameData() {
        string path = Application.persistentDataPath + "/gamedata.data";

        if (!File.Exists(path)) {
            GameData d = new GameData(1);
            
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        GameData data = formatter.Deserialize(stream) as GameData;
        
        stream.Close();

        return data;
    }

    // writes a new GameData with the given levelNum
    public static void SetGameDataLevel(int levelNum) {
        GameData data = new GameData(levelNum);
        SaveGameData(data);
    }
    
}
