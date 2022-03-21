using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerminalFile : MonoBehaviour {

    public string fileName;

    public string GetFileName() {
        return fileName;
    }
}
