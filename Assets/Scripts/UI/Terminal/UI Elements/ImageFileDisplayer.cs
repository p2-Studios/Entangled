using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageFileDisplayer : FileDisplayer {
    public GameObject imageField;
    private Image image;
    public TextMeshProUGUI descriptionField;
    private AudioManager audioManager;


    public void Awake() {
        image = imageField.GetComponent<Image>();
        btnClose.onClick.AddListener(CloseWithButton);
        audioManager = FindObjectOfType<AudioManager>();
    }
    
    public void Open(ImageFile file) {
        fileNameField.text = file.GetFileName();
        descriptionField.text = file.GetDescription();
        image.sprite = file.GetImage();
        SetVisible(true);
    }
    public void CloseWithButton() {
        audioManager.Play("terminal_click");
        audioManager.PlayDelayed("terminal_close_file", 0.1f);
        Close();
    }

    public override void Close() {
        fileNameField.text = "";    // clear content
        descriptionField.text = "";
        //imageField.GetComponent<Image>().image = null;
        TerminalManager.instance.SetViewingFile(false);
        SetVisible(false);
    }
}
