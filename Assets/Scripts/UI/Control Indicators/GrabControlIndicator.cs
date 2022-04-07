using Game.CustomKeybinds;
using TMPro;
using UnityEngine;

public class GrabControlIndicator : MonoBehaviour {
    public TextMeshPro text;
    public SpriteRenderer spriteRenderer;
    
    void Start() {
        string controlText = Keybinds.GetInstance().grabRelease.ToString();
        if (controlText.Contains("Left")) controlText = controlText.Substring(4, controlText.Length - 4);
        if (controlText.Contains("Right")) controlText = controlText.Substring(5, controlText.Length - 5);
        text.text = controlText;
        text.ForceMeshUpdate();
        float width = (controlText.Length * 0.25f);
        spriteRenderer.size = new Vector2(width, spriteRenderer.size.y);
    }

    public void updatestring() {
        string controlText = Keybinds.GetInstance().grabRelease.ToString();
        text.text = controlText;
        float width = (controlText.Length * 0.25f);
        if (controlText.Length == 1) width += 0.25f;
        spriteRenderer.size = new Vector2(width, spriteRenderer.size.y);
    }
}
