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
        LevelData data;

        if (File.Exists(path) && stream.Length > 0) {
            data = formatter.Deserialize(stream) as LevelData;

        } else {
            Debug.LogWarning("No save file found at " + path);
            data = null;
        }

        stream.Close();

        return data;
    }
    
}
