using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// tutorial: https://www.youtube.com/watch?v=_nRzoTzeyxU

// manages the UI components of the terminal, getting the data to display from a given Terminal object
public class TerminalManager : MonoBehaviour {

    public GameObject terminalWindow, fileList, fileButton;
    public ImageFileDisplayer imageFileDisplayer;
    public TextFileDisplayer textFileDisplayer;

    //public float typingSpeed = 0.01f; // the delay (seconds) between each letter appearing
    
    //public Animator animator;   // animator for text box animation

    public Boolean viewingFile, inTerminal, closing; // state booleans

    //private Queue<string> sentences;    // queue of sentences to display, one at a time
    //private string currentSentence;

    private AudioManager audioManager;
    
    public static TerminalManager instance;
    
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        
        
        CloseFileViewers();
        
        terminalWindow.SetActive(false);
    }
    
        
    void Start() {
        //sentences = new Queue<string>();
         inTerminal = viewingFile = false;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            if (inTerminal) {
                // DisplayNextSentence();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (viewingFile) {
                imageFileDisplayer.Close();
                textFileDisplayer.Close();
            } else {
                StartCoroutine(EscapeWindow());
            }
        }
    }

    public void OpenTerminal(Terminal t) {
        terminalWindow.SetActive(true);
        inTerminal = true;
        LoadFiles(t);
    }

    public void CloseTerminal() {
        ClearFiles();
        CloseFileViewers();
        inTerminal = false;
        terminalWindow.SetActive(false);
    }

    public void LoadFiles(Terminal t) {
        //ClearFiles();   // clear out old files
        foreach (TerminalFile file in t.files) {
            TerminalFileButton fb = Instantiate(fileButton, fileList.transform, false).GetComponent<TerminalFileButton>();
            fb.SetFile(file);
        }
    }

    public void ClearFiles() {
        foreach (Transform child in fileList.transform) {
            Destroy(child.gameObject);
        }
    }

    #region old code
    /*
    public void StartDialogue(Dialogue dialogue) {
        Debug.Log("In dialogue");
        starting = inTerminal = true;
        animator.SetBool("IsOpen", true);   // set animator flag to show box

        foreach (string sentence in dialogue.sentences) {
            // load each sentences from the dialogue
            sentences.Enqueue(sentence);
        }
        dialogueText.text = ""; // clear old dialogue text
        StartCoroutine(TypeTitle(dialogue.name)); // type out the dialogue title/program name
    }

    // Types the title/program name
    IEnumerator TypeTitle(string name) {
        nameText.text = "> "; // start with no text
        foreach (char letter in name) { // type each letter one-by-one
            if (inTerminal) {   // make sure we're still in the dialogue
                nameText.text += letter;
                audioManager.Play("text_scroll");
                yield return new WaitForSecondsRealtime(typingSpeed);
            } else { 
                CancelDialogue();
                yield return null;
                
            }
        }

        yield return new WaitForSecondsRealtime(0.3f);  // slight delay after title is typed
        
        starting = false;       // starting setup is finished
        DisplayNextSentence();  // display first sentence
    }
    
    // ends the dialogue, removing it from the screen and setting flags appropriately
    IEnumerator EndDialogue() {
        yield return new WaitForSeconds(0.05f); // wait for a moment to allow escape menu to check if inTerminal is false
        animator.SetBool("IsOpen", false);  // set animator flag to hide text box
        inTerminal = false;
        closing = true;
        StartCoroutine(CloseDialogue());
        yield return null;
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
            StartCoroutine(EndDialogue());
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
            if (inTerminal) {
                // make sure we're still in the dialogue
                dialogueText.text += letter;
                audioManager.Play("text_scroll");
                yield return new WaitForSecondsRealtime(typingSpeed);
            } else {
                CancelDialogue();
                yield return null;
            }
        }
        typing = false;
    }
    
    */
    #endregion

    IEnumerator EscapeWindow() {
        yield return new WaitForEndOfFrame();
        CloseTerminal();
        closing = false;
    }

    public void OpenTextFile(TextFile file) {
        CloseFileViewers();
        viewingFile = true;
        textFileDisplayer.Open(file);
    }
    
    public void OpenImageFile(ImageFile file) {
        CloseFileViewers();
        viewingFile = true;
        imageFileDisplayer.Open(file);
    }

    public void CloseFileViewers() {
        imageFileDisplayer.Close();
        textFileDisplayer.Close();
        viewingFile = false;
    }

    public void SetViewingFile(bool b) {
        viewingFile = b;
    }

}
