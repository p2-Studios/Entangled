using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// manages the UI components of the terminal, getting the data to display from a given Terminal object
public class TerminalManager : MonoBehaviour {
    private AudioManager audioManager;
    private Boolean viewingFile, inTerminal, errorVisible; // state booleans
    
    [Space(10)]
    [Header("Terminal UI Elements")]     
    public Button exitButton;
    public GameObject terminalWindow, localFileList, remoteFileList, errorWindow, fileButton, encryptedFileButton, unlockedFileButton;
    
    [Space(10)]
    [Header("Terminal Displayers")] 
    public ImageFileDisplayer imageFileDisplayer;
    public TextFileDisplayer textFileDisplayer;
    
    [Space(10)]
    [Header("Flashdrive Notification")] 
    public GameObject flashdriveNotification;
    public Image flashDriveNotificationPanel;
    public TextMeshProUGUI flashDriveNotificationHeader, flashDriveNotificationBody;
    public float flashDriveNotifyTime = 3.0f;
    public float flashdriveNotifyFade = 1.0f;

    public static TerminalManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        CloseSubWindows();
        
        terminalWindow.SetActive(false);
        flashdriveNotification.SetActive(false);
    }

    void Start() {
        //sentences = new Queue<string>();
         inTerminal = viewingFile = false;
        audioManager = FindObjectOfType<AudioManager>();
        exitButton.onClick.AddListener(CloseTerminal);
    }

    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (viewingFile) {
                imageFileDisplayer.Close();
                textFileDisplayer.Close();
            } else {
                StartCoroutine(EscapeWindow());
            }
        }
    }

    // opens and displays the terminal gui, with the files of the given terminal
    public void OpenTerminal(Terminal t) {
        terminalWindow.SetActive(true);
        inTerminal = true;
        LoadFiles(t);
    }

    // closes the entire terminal
    public void CloseTerminal() {
        ClearFiles();
        CloseFileViewers();
        inTerminal = false;
        terminalWindow.SetActive(false);
    }

    // loads all local and remote files from the given Terminal object, and puts them in the correct categories
    public void LoadFiles(Terminal t) {
        
        Level level = FindObjectOfType<Level>(); // get level data
        ArrayList unlockedRemoteFiles = new ArrayList();
        if (level != null) {
            foreach (FlashDrive fd in level.foundFlashDrives) {
                // get unlocked files (found flashdrives)
                unlockedRemoteFiles.Add(fd.file);
            }
        }


        foreach (TerminalFile file in t.GetLocalFiles()) {  // load local files from terminal object
            TerminalFileButton fb = Instantiate(fileButton, localFileList.transform, false).GetComponent<TerminalFileButton>();
            fb.SetFile(file);
        }
        
        foreach (TerminalFile file in t.GetRemoteFiles()) { // load remote files from terminal object
            TerminalFileButton fb;
            bool locked = !unlockedRemoteFiles.Contains(file);
            if (file.IsEncrypted() && locked) { // file is encrypted and corresponding flashdrive hasn't been found
                fb = Instantiate(encryptedFileButton, remoteFileList.transform, false).GetComponent<TerminalFileButton>();
            } else {  // file is unlocked
                fb = Instantiate(unlockedFileButton, remoteFileList.transform, false).GetComponent<TerminalFileButton>();
            }
            fb.SetFile(file);
            
            if (!locked) fb.Unlock();
        }
    }

    // clears all files in the terminal file lists
    public void ClearFiles() {
        foreach (Transform child in localFileList.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in remoteFileList.transform) {
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

    // closes the frontmost terminal window
    IEnumerator EscapeWindow() {
        yield return new WaitForEndOfFrame();
        CloseTerminal();
    }

    // opens the given text file, displaying it on the textFileDisplayer
    public void OpenTextFile(TextFile file) {
        CloseFileViewers();
        viewingFile = true;
        textFileDisplayer.Open(file);
    }
    
    // opens the given image file, displaying it on the imageFileDisplayer
    public void OpenImageFile(ImageFile file) {
        CloseFileViewers();
        viewingFile = true;
        imageFileDisplayer.Open(file);
    }

    // closes ALL sub-windows of the terminal
    public void CloseSubWindows() {
        CloseFileViewers();
        errorWindow.SetActive(false);
    }
    
    // Closes all potentially open file viewer windows
    public void CloseFileViewers() {
        imageFileDisplayer.Close();
        textFileDisplayer.Close();
        viewingFile = false;
    }

    public bool IsTerminalOpen() {
        return inTerminal;
    }
    
    public bool IsViewingFile() {
        return viewingFile;
    }
    
    // set whether a file is currently being viewed in the terminal
    public void SetViewingFile(bool b) {
        viewingFile = b;
    }

    public void DisplayError() {
        if (!errorVisible) StartCoroutine(ErrorDisplay());
    }
    
    IEnumerator ErrorDisplay() {
        errorWindow.SetActive(true);
        errorVisible = true;
        yield return new WaitForSecondsRealtime(3.0f);
        errorWindow.SetActive(false);
        errorVisible = false;
    }

    public void FlashDriveFound() {
        StartCoroutine(DisplayFlashDriveNotification());
    }
    
    IEnumerator DisplayFlashDriveNotification() {
        flashdriveNotification.SetActive(true);
        yield return new WaitForSeconds(flashDriveNotifyTime);
        Color colorToFadeTo = new Color (1f, 1f, 1f, 0f);
        flashDriveNotificationPanel.CrossFadeColor(colorToFadeTo, flashdriveNotifyFade, true, true);
        flashDriveNotificationHeader.CrossFadeColor(colorToFadeTo, flashdriveNotifyFade, true, true);
        flashDriveNotificationBody.CrossFadeColor(colorToFadeTo, flashdriveNotifyFade, true, true);
        yield return new WaitForSeconds(flashdriveNotifyFade);
        flashdriveNotification.SetActive(false);
        yield return null;
    }

}
