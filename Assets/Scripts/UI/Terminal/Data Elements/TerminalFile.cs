using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerminalFile : MonoBehaviour {

    public bool encrypted = false;
    public string fileName;

    public string GetFileName() {
        return fileName;
    }

    public bool IsEncrypted() {
        return encrypted;
    }
}
