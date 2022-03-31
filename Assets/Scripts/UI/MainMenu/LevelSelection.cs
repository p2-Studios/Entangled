using System;
using System.Collections;
using System.Collections.Generic;
using Game.CustomKeybinds;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour {

    [Header( "Camera Setting")]
    public Canvas canvas;
    public Camera cam;
    private bool camAtTop = true;
    private bool camLocked = true;
    public Transform cameraTopPosition, cameraBottomPosition;
    private Vector3 nextCameraPosition;
    public float cameraSpeed = 1f;
    public GameObject scrollUpText, scrollDownText;
    
    [Space(5)] 
    [Header( "Level Unlocking Settings")]
    public Button[] levelButtons;
    public Image buildingImage;
    public Sprite[] buildingSprites;
    public GameObject[] fanObjects;

    [Space(5)] [Header("Level Data Settings")]
    public TextMeshProUGUI[] flashDriveTexts;
    
    private GameData data;
    private int unlockedLevel;
    
    private void Awake() {
        nextCameraPosition = cameraTopPosition.position;
        unlockedLevel = GetGameData().GetUnlockedLevel();

        // both scroll indicators disables by default (will be enabled in a moment if needed)
        scrollDownText.SetActive(false);
        scrollUpText.SetActive(false);
        
        // set all levels unlocked (for testing/debug/until all levels are ready)
        SaveSystem.SetGameDataLevel(10);
        
        // initializes the visuals/interactables based on the user's unlocked levels
        InitializeLevelButtons();
        InitializeBuildingVisuals();
        InitializeFlashDriveTexts();
        
        // unlock the camera if underground level(s) unlocked
        if (GetGameData().unlockedLevel > 5) camLocked = false;
    }

    // Get the player's GameData (right now just their latest level unlocked) from the save system
    private GameData GetGameData() {
        if (data == null) 
            data = SaveSystem.LoadGameData();
        return data;
    }
    
    // load a scene
    public void StartAtLevel(string levelName) {
        SceneManager.LoadSceneAsync(levelName);
    }

    private void Update() {
        // move the camera to the top when scrolling up
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && !camLocked) { // scroll up
            if (!camAtTop) {
                MoveToTop();
            }    
        }
        
        // move the camera to the bottom when scrolling down
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && !camLocked) { // scroll down
            if (camAtTop) {
                MoveToBottom();
            }    
        }
        
        // if the camera isn't where it's supposed to be, move it towards that position
        if (cam.transform.position != nextCameraPosition) {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, 
                nextCameraPosition, cameraSpeed * Time.deltaTime);
            scrollDownText.SetActive(false);
            scrollUpText.SetActive(false);
        } else {
            if (cam.transform.position == cameraTopPosition.position) {
                scrollDownText.SetActive(true);
            } else {
                scrollUpText.SetActive(true);
            }
        }
    }

    public void MoveToTop() {
        nextCameraPosition = cameraTopPosition.position;
        camAtTop = true;
    }

    public void MoveToBottom() {
        nextCameraPosition = cameraBottomPosition.position;
        camAtTop = false;
    }

    // initializes the button for each level
    //      - disables a button if the player hasn't reached that level
    public void InitializeLevelButtons() {
        for (int i = 0; i < levelButtons.Length; i++) {
            if (i+1 > unlockedLevel) {
                levelButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // set the image for the above-ground part of the building based on the unlocked level
    public void InitializeBuildingVisuals() {
        // above-ground window lighting
        if (unlockedLevel > 5) {
            buildingImage.sprite = buildingSprites[4];
        } else {
            buildingImage.sprite = buildingSprites[unlockedLevel - 1];
        }
        
        // underground fans
        for (int i = 6; i < fanObjects.Length + 6; i++) {
            if (i > unlockedLevel) {
                fanObjects[i-6].SetActive(false);
            }
        }
    }

    // fill the flash drives found per level, by grabbing the data from each levels' 
    public void InitializeFlashDriveTexts() {
        // retrieve level data for level corresponding to each text box
        for (int i = 0; i < flashDriveTexts.Length; i++) {
            TextMeshProUGUI text = flashDriveTexts[i]; // get the flash drive text box for this level
            String levelName = "Level" + (i + 1);
            LevelData levelData = SaveSystem.LoadLevel(levelName); // levels start at 1, add 1 to i
            if (levelData == null) { // level data doesn't exit
                text.text = "?/?";                           
                print("No data for " + levelName);
            } else {    // level data exists, use values                           
                print("Data found for " + levelName);
                int totalFlashDrives = levelData.totalFlashDrives;
                int flashDrivesFound = levelData.foundFlashDrives.Length;
                text.text = flashDrivesFound + "/" + totalFlashDrives;
            }
        }
    }

}