using System;
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
    }

    public void OpenFile() {
        if (TerminalManager.instance.IsViewingFile()) return;
        if (locked) {
            TerminalManager.instance.DisplayError();
        } else {
            if (file.GetType() == typeof(TextFile)) {
                TerminalManager.instance.OpenTextFile((TextFile) file);
            } else if (file.GetType() == typeof(ImageFile)) {
                TerminalManager.instance.OpenImageFile((ImageFile) file);
            } else {
                Debug.LogWarning("Non- image or text file attempted to be opened.");
            }
        }
    }

    public TerminalFile GetFile() {
        return file;
    }
    
    public void SetFile(TerminalFile f) {
        file = f;
        locked = f.IsEncrypted();
        SetButtonLabel();
    }

    private void SetButtonLabel() {
        if (locked) {
            buttonLabel.text = GetStringSha256Hash(file.fileName);
        } else {
            buttonLabel.text = file.fileName;
        }
    }
    
    internal static string GetStringSha256Hash(string text) {
        if (String.IsNullOrEmpty(text))
            return String.Empty;

        using (var sha = new System.Security.Cryptography.SHA256Managed()) {
            byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] hash = sha.ComputeHash(textData);
            return (BitConverter.ToString(hash).Replace("-", String.Empty)).Substring(0, 16);
        }
    }

    public void Lock() {
        locked = true;
        SetButtonLabel();
    }

    public void Unlock() {
        locked = false;
        SetButtonLabel();
    }
}
