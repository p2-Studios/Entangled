using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// tutorial: https://www.youtube.com/watch?v=_nRzoTzeyxU
public class DialogueManager : MonoBehaviour {
    public Text nameText; // text box header - the name of the interactable triggering the text
    public Text dialogueText;

    public float typingSpeed = 0.01f;
    
    public Animator animator;

    private Boolean inDialogue, typing;
    
    
    private Queue<string> sentences;
    
    void Start() {
        sentences = new Queue<string>();
        inDialogue = typing = false;
    }

    public void StartDialogue(Dialogue dialogue) {
        animator.SetBool("IsOpen", true);
        
        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        nameText.text = dialogue.name;
        
        DisplayNextSentence();
    }

    public void EndDialogue() {
        animator.SetBool("IsOpen", false);
        Debug.Log("End of conversation");
    }
    
    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }
}
