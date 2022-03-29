using System;
using System.Collections;
using System.Collections.Generic;
using Game.CustomKeybinds;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour {

    [Header( "Camera Setting")]
    public Camera cam;
    private bool camAtTop = true;
    private bool camLocked = true;
    private bool camMoving;
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
    
    
    private GameData data;
    private int unlockedLevel;
    
    private void Awake() {
        nextCameraPosition = cameraTopPosition.position;
        unlockedLevel = GetGameData().GetUnlockedLevel();
        
        scrollDownText.SetActive(false);
        scrollUpText.SetActive(false);
        
        // set all levels unlocked (for testing/debug/until all levels are ready)
        SaveSystem.SetGameDataLevel(10);
        
        InitializeLevelButtons();
        InitializeBuildingVisuals();
        
        if (GetGameData().unlockedLevel > 5) camLocked = false; // unlock the camera if underground level(s) unlocked
    }

    private GameData GetGameData() {
        if (data == null) 
            data = SaveSystem.LoadGameData();
        return data;
    }
    
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
        if (unlockedLevel > 5) {
            buildingImage.sprite = buildingSprites[4];
        } else {
            buildingImage.sprite = buildingSprites[unlockedLevel - 1];
        }
        for (int i = 6; i < fanObjects.Length + 6; i++) {
            if (i > unlockedLevel) {
                fanObjects[i-6].SetActive(false);
            }
        }
    }

}