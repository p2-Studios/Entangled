using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorial: https://www.youtube.com/watch?v=_nRzoTzeyxU

[System.Serializable]
public class Dialogue {
    public string name;
    
    [TextArea(3, 10)]
    public string[] sentences;
}
