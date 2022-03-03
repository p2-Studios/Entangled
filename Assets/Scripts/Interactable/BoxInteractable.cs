using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInteractable : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Sprite button;
    public Sprite grab;
    private bool toggle;
    private Transform indicator;
    // Start is called before the first frame update
    void Start()
    {
        toggle = false;
        indicator = transform.GetChild(0);
        indicator.gameObject.SetActive(toggle);
    }

    public void toggleIndicator(bool state){
        toggle = state;
        indicator.gameObject.SetActive(toggle);
    }

    public void toggleSprite(bool state){
        if (state)
            spriteRenderer.sprite = grab;
        else
            spriteRenderer.sprite = button;
    }
}
