using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileDisplayer : MonoBehaviour {

    public Canvas canvas;
    public TextMeshProUGUI fileNameField;
    public Button btnClose;
    
    private void Awake() {
        btnClose.onClick.AddListener(Close);
    }
    
    protected void SetVisible(bool v) {
        canvas.gameObject.SetActive(v);
    }

    public virtual void Close() {
        SetVisible(false);
    }
    
}
