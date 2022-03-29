using System;
using System.Collections;
using System.Collections.Generic;
using Game.CustomKeybinds;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour {

    public Camera cam;
    private bool camAtTop = true;
    private bool camMoving;
    public Transform cameraTopPosition, cameraBottomPosition;
    private Vector3 nextCameraPosition;
    public float cameraSpeed = 1f;

    private void Awake() {
        nextCameraPosition = cameraTopPosition.position;
    }

    public void StartAtLevel(string levelName) {
        SceneManager.LoadSceneAsync(levelName);
    }

    private void Update() {
        // move the camera to the top when scrolling up
        if (Input.GetAxis("Mouse ScrollWheel") > 0f ) { // scroll up
            if (!camAtTop) {
                print("scroll up");
                MoveToTop();
            }    
        }
        
        // move the camera to the bottom when scrolling down
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) { // scroll down
            if (camAtTop) {
                print("scroll down");
                MoveToBottom();
            }    
        }

        print(cam.transform.position + " - " + nextCameraPosition);
        
        // if the camera isn't where it's supposed to be, move it towards that position
        if (cam.transform.position != nextCameraPosition) {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, 
                nextCameraPosition, cameraSpeed * Time.deltaTime);
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
    
}