using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour {
    public Text hintText; // text box hint
    
    public Animator animator;   // animator for text box animation

    private AudioManager audioManager;
    
    void Start() {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OpenHint(Hint hint) {
        animator.SetBool("isOpen", true);   // set animator flag to show box
        hintText.text = hint.text;
    }
    
    // closes the hint, removing it from the screen
    public void CloseHint() {
        animator.SetBool("isOpen", false);  // set animator flag to hide text box
    }
}
