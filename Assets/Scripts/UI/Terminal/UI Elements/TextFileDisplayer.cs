using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TextFileDisplayer : FileDisplayer {
    public TextMeshProUGUI textField;
    private AudioManager audioManager;

    private void Awake() {
        btnClose.onClick.AddListener(CloseWithButton);
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void Open(TextFile file) {
        fileNameField.text = file.GetFileName();
        textField.text = file.GetBodyText();
        SetVisible(true);
    }
    
    public void CloseWithButton() {
        audioManager.Play("terminal_click");
        audioManager.PlayDelayed("terminal_close_file", 0.1f);
        Close();
    }

    public override void Close() {
        fileNameField.text = "";    // clear content
        textField.text = "";
        TerminalManager.instance.SetViewingFile(false);
        SetVisible(false);
    }
    
}
