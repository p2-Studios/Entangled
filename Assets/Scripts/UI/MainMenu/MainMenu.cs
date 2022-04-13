using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    
    private GameData data;
    public Button levelsButton;
    public GameObject resetConfirmationPanel;
    
    private void Awake() {
        if (GetGameData().GetUnlockedLevel() < 1) {
            levelsButton.interactable = false;
        }
        PauseMenu.instance.ToggleControlIndicator(false);   // control indicator off in main menu
    }

    private void Update() {
        // unlocks all levels
        if (Input.GetKeyDown(KeyCode.L)) {
                    SaveSystem.SetGameDataLevel(9);
                    SceneManager.LoadSceneAsync("MainMenu");
        }
    }

    public void StartAtLevel(string levelName) {
        SceneManager.LoadSceneAsync(levelName);
    }

	public void Options() {
		SceneManager.LoadSceneAsync("Options", LoadSceneMode.Additive);
	}
	
    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OpenFeedbackLink() {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSduizPyNvgwBKM6RKQIJBMhdP-MfVxOmvlQN4bWaZeT_3VL7Q/viewform");
    }
    
    IEnumerator StartMusicWithDelay() {
        yield return new WaitForSeconds(1f);
        
        AudioManager am = FindObjectOfType<AudioManager>();
        if (am != null) am.Play("music_main");
    }
    
    private GameData GetGameData() {
        if (data == null) 
            data = SaveSystem.LoadGameData();
        return data;
    }

    public void NewGame() {
        if (GetGameData().GetUnlockedLevel() < 1) {
            StartAtLevel("Level0");
        } else {
            resetConfirmationPanel.SetActive(true);
        }
    }

    public void CancelReset() {
        resetConfirmationPanel.SetActive(false);
    }

    public void ConfirmReset() {
        SaveSystem.ClearSaveData();
        resetConfirmationPanel.SetActive(false);
        SceneManager.LoadScene("Level0");
    }
}
