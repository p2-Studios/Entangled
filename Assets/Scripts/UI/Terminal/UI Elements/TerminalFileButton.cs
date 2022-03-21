using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalFileButton : MonoBehaviour {
    public bool locked;
    public TextMeshProUGUI buttonLabel;
    public Button btn;
    private TerminalFile file;


    private void Awake() {
        btn.onClick.AddListener(OpenFile);
        Debug.Log(btn.onClick.ToString());
    }

    public void OpenFile() {
        if (file.GetType() == typeof(TextFile)) {
            TerminalManager.instance.OpenTextFile((TextFile) file);
        } else if (file.GetType() == typeof(ImageFile)) {
            TerminalManager.instance.OpenImageFile((ImageFile) file);
        } else {
            Debug.LogWarning("Non- image or text file attempted to be opened.");
        }
    }

    public TerminalFile GetFile() {
        return file;
    }
    
    public void SetFile(TerminalFile f) {
        file = f;
        buttonLabel.text = file.fileName;
    }
}
