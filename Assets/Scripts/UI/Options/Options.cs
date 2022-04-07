using Game.CustomKeybinds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
	// The main screen of options
	public GameObject main;
	public Button display;
	public Button controls;
	public Button sound;
	public Button back;
	
	// Display
	public GameObject display_menu;
	
	// Sound
	public GameObject sound_menu;
	
	// Controls
	public GameObject control_menu;
	
	
	
    // Start is called before the first frame update
    void Start()
    {
		// main
		back.onClick.AddListener(btnBack);
		display.onClick.AddListener(btnDisplay);
		sound.onClick.AddListener(btnSound);
		controls.onClick.AddListener(btnControl);
		
    }
	
	 // Update is called once per frame
    void Update() {
		if(main.activeSelf && Input.GetKeyDown(Keybinds.GetInstance().pause)) {
			btnBack();
		}
	}
	
	// button display.onclick function
	void btnDisplay() {
		// Hide main
		main.SetActive(false);
		
		// Show display
		display_menu.SetActive(true);
	}
	
	// button sound.onclick function
	void btnSound() {
		// Hide main
		main.SetActive(false);
		
		// Show sound
		sound_menu.SetActive(true);
	}
	
	// button control.onclick function
	void btnControl() {
		// Hide main
		main.SetActive(false);
		
		// Show control
		control_menu.SetActive(true);
	}
	
	// button exit.onclick function
	void btnBack() {
		SceneManager.UnloadSceneAsync("Options");
	}
	
}
