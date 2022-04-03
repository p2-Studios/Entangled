using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TextFileDisplayer : FileDisplayer {
    public TextMeshProUGUI textField;

    private void Awake() {
        btnClose.onClick.AddListener(CloseWithButton);
    }

    public void Open(TextFile file) {
        fileNameField.text = file.GetFileName();
        textField.text = file.GetBodyText();
        SetVisible(true);
    }
    
    public void CloseWithButton() {
        FindObjectOfType<AudioManager>().Play("terminal_click");
        Close();
    }

    public override void Close() {
        fileNameField.text = "";    // clear content
        textField.text = "";
        TerminalManager.instance.SetViewingFile(false);
        SetVisible(false);
    }
    
}
