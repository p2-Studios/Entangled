using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageFileDisplayer : FileDisplayer {
    public GameObject imageField;
    private Image image;
    public TextMeshProUGUI descriptionField;


    public void Awake() {
        image = imageField.GetComponent<Image>();
        btnClose.onClick.AddListener(CloseWithButton);
    }
    
    public void Open(ImageFile file) {
        fileNameField.text = file.GetFileName();
        descriptionField.text = file.GetDescription();
        image.sprite = file.GetImage();
        SetVisible(true);
    }
    public void CloseWithButton() {
        FindObjectOfType<AudioManager>().Play("terminal_click");
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
