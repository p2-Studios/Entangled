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
	public Button graphics;
	public Button controls;
	public Button sound;
	public Button back;
	
	// Graphics
	
	// Controls
	public GameObject control;
	
	// Sound
	
    // Start is called before the first frame update
    void Start()
    {
		// main
		back.onClick.AddListener(btnBack);
		controls.onClick.AddListener(btnControl);
		
    }
	
	 // Update is called once per frame
    void Update() {
		if(main.activeSelf && Input.GetKeyDown(Keybinds.GetInstance().pause)) {
			btnBack();
		}
	}
	
	// button control.onclick function
	void btnControl() {
		// Hide main
		main.SetActive(false);
		
		// Show control
		control.SetActive(true);
	}
	
	// button exit.onclick function
	void btnBack() {
		SceneManager.UnloadSceneAsync("Options");
	}
	
}
