using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ImageFile : TerminalFile {
    
    public string description;
    public Image image;

    public string GetDescription() {
        return description;
    }
    
    public Image GetImage() {
        return image;
    }
    
}
