using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MouseHover - Controls mouse hover change **(Instance class - place in empty gameobject in scene)**
/// </summary>
public class MouseHover : MonoBehaviour
{
    public Texture2D defaultCursor, clickableCursor;
    public static MouseHover instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    /*
    public void Clickable() {
        Cursor.SetCursor(clickableCursor, Vector2.zero, CursorMode.Auto);
    }

    public void Default() {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
    */
}
