using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Game.CustomKeybinds
{
    public static class SaveLoadKeybinds
    {
        // Copied from SaveSystem.cs
        public static void SaveControlScheme(Level level)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + "/keybinds.config";
            FileStream stream = new FileStream(path, FileMode.Create);

            // Keybinds data = Camera.GetComponent<Keybinds>().GetInstance();
            Keybinds data = Keybinds.GetInstance();
            
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static Keybinds LoadControlScheme()
        {
            string path = Application.persistentDataPath + "/keybinds.config";

            if (!File.Exists(path))
            {
                return null;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Keybinds data = formatter.Deserialize(stream) as Keybinds;

            stream.Close();

            return data;
        }
    }
}