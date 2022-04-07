using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FinaleScreenManager : MonoBehaviour {
    [Header("Uplink Display")]
    public GameObject uplinkDisplay;
    public GameObject progressBar;
    public TextMeshProUGUI progressBarText;
    public TextMeshProUGUI uplinkText;
    
    [Space(10)]
    [Header("Lock Screens")]
    public GameObject[] lockScreens;
    public Image[] lockImages;
    public FlashDrive[] flashDrives;
    public Sprite unlockedScreen, unlockedImage;
    
    [Space(10)] [Header("Middle Screen")] 
    public GameObject countDisplay;
    public TextMeshProUGUI flashDriveCountText;
    private int[] flashDriveCounts = {0,0};
    public Button uplinkButton;
    
    [Space(10)]
    public Level level;
    private bool allKeys;
    
    private void Start() {
        uplinkDisplay.SetActive(false);
        flashDriveCounts = level.GetFlashDriveCounts();
        UpdateFlashDriveDisplay();
    }

    private void Update() {
        if (!level.GetFlashDriveCounts().SequenceEqual(flashDriveCounts)) {
            flashDriveCounts = level.GetFlashDriveCounts();
            UpdateFlashDriveDisplay();
        }
    }

    public void UpdateFlashDriveDisplay() {
        flashDriveCountText.text = flashDriveCounts[1] + "/" + flashDriveCounts[0];

        switch (flashDriveCounts[1]) {
            case 0:
                break;
            case 1:
                flashDriveCountText.color = new Color(255, 85, 0);
                break;
            case 2:
                flashDriveCountText.color = new Color(255, 135, 0);
                break;
            case 3:
                flashDriveCountText.color = new Color(255, 240, 0);
                break;
            case 4:
                flashDriveCountText.color = Color.green;
                uplinkButton.GetComponent<Image>().color = Color.green;
                allKeys = true;
                break;
        }
        
        for (int i = 0; i < 4; i++) {
            if (flashDrives[i].IsCollected()) { // if the flashdrive has been collected, set monitor to green and image to unlocked
                lockScreens[i].GetComponent<Image>().sprite = unlockedScreen;
                lockImages[i].sprite = unlockedImage;
            }
        }
    }

    public void EstablishUplink() {
        countDisplay.SetActive(false);
        uplinkDisplay.SetActive(true);
        AudioManager.instance.Play("finale_terminal_start");
        StartCoroutine(UplinkBar());
    }

    private IEnumerator FileReadError() {
        uplinkButton.interactable = false;
        uplinkButton.GetComponentInChildren<TextMeshProUGUI>().text = "FILE READ ERROR";
        yield return new WaitForSeconds(2);
        uplinkButton.interactable = true;
        uplinkButton.GetComponentInChildren<TextMeshProUGUI>().text = "ESTABLISH UPLINK";
    }

    private IEnumerator UplinkBar() {
        float failPercent = flashDriveCounts[1] * 0.25f;

        uplinkText.color = Color.magenta;
        uplinkText.fontSize = 20;
        uplinkText.text = "SENDING DATA TO SURFACE STATION...";
        progressBarText.text = "0%";
        AudioManager.instance.PlayLooping("finale_data_transmit");
        
        float percent = 0;
        Slider bar = progressBar.GetComponent<Slider>();
        bar.value = 0f;
        while (percent < failPercent) { // progress bar
            percent += Random.Range(0.001f, 0.01f);
            bar.value = percent;
            if (percent >= 1f) {
                progressBarText.text = "100%";
            } else {
                progressBarText.text = Math.Round(percent*100.0, 2) + "%";
            }
            yield return new WaitForSeconds(0.05f);
        }
        
        AudioManager.instance.StopLooping("finale_data_transmit");
        
        if (percent < 1f) { // incomplete - show error
            uplinkText.color = Color.red;
            uplinkText.text = "FILE READ ERROR: FILE(S) ENCRYPTED";
            AudioManager.instance.Play("terminal_error");
            yield return new WaitForSeconds(5.0f);
            uplinkDisplay.SetActive(false);
            countDisplay.SetActive(true);
        } else {
            AudioManager.instance.Play("finale_terminal_success");
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(TransmitData());
        }
    }
    
    private IEnumerator TransmitData() {
        AudioManager.instance.Play("finale_terminal_start");
        float failPercent = 0.9833f;
        float percent = 0;
        uplinkText.color = Color.cyan;
        uplinkText.fontSize = 18;
        uplinkText.text = "TRANSMITTING DATA TO LOW-ORBIT SATELLITE...";
        AudioManager.instance.PlayLooping("finale_data_transmit");
        Slider bar = progressBar.GetComponent<Slider>();
        bar.value = 0f;
        while (percent < failPercent) {
            percent += Random.Range(0.001f, 0.01f);
            if (percent > failPercent) percent = failPercent;
            bar.value = percent;
            progressBarText.text = Math.Round(percent*100.0, 2) + "%";
            yield return new WaitForSeconds(0.05f);
        }
        AudioManager.instance.StopLooping("finale_data_transmit");
        yield return new WaitForSeconds(3.25f);
        uplinkText.color = Color.red;
        uplinkText.text = "TRANSMISSION FAILED: FATAL ERROR";
        AudioManager.instance.Play("terminal_error");
        yield return new WaitForSeconds(3);
        // do camera transition stuff
        FindObjectOfType<CameraToggle>().TransitionToTop();
    }

    public void SatelliteEntangled() {
        StartCoroutine(FinishTransmission());
    }
    
    private IEnumerator FinishTransmission() {
        yield return new WaitForSeconds(3.0f);
        float percent = 0.9833f;
        uplinkText.color = Color.cyan;
        AudioManager.instance.PlayLooping("finale_data_transmit");
        uplinkText.fontSize = 18;
        uplinkText.text = "TRANSMITTING DATA TO LOW-ORBIT SATELLITE...";
        Slider bar = progressBar.GetComponent<Slider>();
        bar.value = 0f;
        while (percent < 1) {
            percent += Random.Range(0.0001f, 0.001f);
            if (percent > 1f) {
                percent = 1f;
            }
            bar.value = percent;
            progressBarText.text = Math.Round(percent*100.0, 2) + "%";
            yield return new WaitForSeconds(0.25f);
        }
        AudioManager.instance.StopLooping("finale_data_transmit");
        yield return new WaitForSeconds(1.0f);
        AudioManager.instance.Play("finale_terminal_success");
        uplinkText.color = Color.green;
        uplinkText.text = "TRANSMISSION DELIVERED TO EARTH.";
    }
}
