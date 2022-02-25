using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// tutorial: https://www.youtube.com/watch?v=_nRzoTzeyxU
public class DialogueManager : MonoBehaviour {
    public Text nameText; // text box header - the name of the interactable triggering the text
    public Text dialogueText; // the body of the text box

    public float typingSpeed = 0.01f; // the delay (seconds) between each letter appearing
    
    public Animator animator;   // animator for text box animation

    public Boolean starting, inDialogue, typing; // state booleans
    
    private Queue<string> sentences;    // queue of sentences to display, one at a time
    private string currentSentence;
    
    void Start() {
        sentences = new Queue<string>();
        starting = inDialogue = typing = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            if (inDialogue) {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        starting = true;
        animator.SetBool("IsOpen", true);   // set animator flag to show box
        
        sentences.Clear();  // clear sentences possibly left over from previous dialogue

        foreach (string sentence in dialogue.sentences) {
            // load each sentences from the dialogue
            sentences.Enqueue(sentence);
        }

        StartCoroutine(TypeTitle(dialogue.name)); // type out the dialogue title/program name
        
        starting = false;
        
        inDialogue = true;
        DisplayNextSentence();  // display first sentence
    }

    public void EndDialogue() {
        animator.SetBool("IsOpen", false);  // set animator flag to hide text box
        inDialogue = false;
    }
    
    // displays the next sentence, or skips typing the current sentence
    public void DisplayNextSentence() {
        if (starting) return;
        if (typing) {   // next sentence triggered before typing finished, display full line
            StopAllCoroutines();
            dialogueText.text = "> " + currentSentence;
            typing = false;
            return;
        } 
        
        // if previous sentence is fully typed, move to the next sentence
        if (sentences.Count == 0) { // end dialogue if no sentences remain
            EndDialogue();
            return;
        }
        
        currentSentence = sentences.Dequeue(); // get next sentence from queue
        StartCoroutine(TypeSentence(currentSentence));
    }

    // Types the body of a sentence onto the screen
    IEnumerator TypeSentence(string sentence) {
        typing = true;
        dialogueText.text = "> "; // start with no text
        foreach (char letter in sentence) { // type each letter one-by-one
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        typing = false;
    }
    
    // Types the title/program name
    IEnumerator TypeTitle(string name) {
        nameText.text = "> "; // start with no text
        foreach (char letter in name) { // type each letter one-by-one
            nameText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed/2);
        }
    }
}
