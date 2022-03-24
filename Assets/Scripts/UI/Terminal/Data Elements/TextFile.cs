using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextFile : TerminalFile {

    public TextFile(string name, string body) {
        this.name = name;
        this.body = body;
    }
    
    [TextArea(3, 10)]
    public string body;

    public string GetBodyText() {
        return body;
    }

}
