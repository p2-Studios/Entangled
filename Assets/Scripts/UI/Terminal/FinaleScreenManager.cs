using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinaleScreenManager : MonoBehaviour {
    public GameObject[] lockScreens;
    public Image[] lockImages;
    public FlashDrive[] flashDrives;
    public Sprite unlockedScreen, unlockedImage;
    public TextMeshProUGUI flashDriveCountText;
    private int[] flashDriveCounts = {0,0};

    public Level level;
    private void Start() {
        flashDriveCounts = level.GetFlashDriveCounts();
        UpdateFlashDriveDisplay();
    }

    private void Update() {
        if (level.GetFlashDriveCounts() != flashDriveCounts) {
            flashDriveCounts = level.GetFlashDriveCounts();
        }
    }

    public void UpdateFlashDriveDisplay() {
        int[] count = level.GetFlashDriveCounts();
        flashDriveCountText.text = count[1] + "/" + count[0];

        for (int i = 0; i < 4; i++) {
            print(flashDrives[i].IsCollectd());
            if (flashDrives[i].IsCollectd()) { // if the flashdrive has been collected, set monitor to green and image to unlocked
                lockScreens[i].GetComponent<Image>().sprite = unlockedScreen;
                lockImages[i].sprite = unlockedImage;
            }
        }
    }
}
