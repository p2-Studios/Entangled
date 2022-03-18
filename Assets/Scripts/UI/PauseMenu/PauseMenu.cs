using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	// Screen 1: Main pause menu
	public GameObject menu;
	public Button resume;
	public Button reset;
	public Button options;
	public Button exit;
	
	// Screen 2: confirmation screen for exit
	public GameObject confirm;
	public Text confirmation;
	public Button yes;
	public Button no;
	
	// Screen 3: Option screen
	// TODO : When options are available implement them here
	public GameObject Options;
	public Button back;
	
	// Timescale for pause
	private float timescale;
	
	// Click Timer
	private DateTime clickTime;
	private bool buttonClick = false;

	public static PauseMenu instance;

	private void Awake() {
		// don't destroy pause menu when switching scenes
		DontDestroyOnLoad(gameObject);
		
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	// Start is called before the first frame update
    void Start() {
        resume.onClick.AddListener(btnResume);
		reset.onClick.AddListener(btnReset);
		options.onClick.AddListener(btnOptions);
		exit.onClick.AddListener(btnExit);
		
		yes.onClick.AddListener(btnYes);
		no.onClick.AddListener(btnNo);
		
		back.onClick.AddListener(btnBack);
		
		timescale = Time.timeScale;
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
	        if (SceneManager.GetActiveScene().buildIndex != 0) {
		        // don't allow opening in main menu
		        menuState();
	        }
        }
		if (Input.GetMouseButton(0)) {
			if (menu.activeSelf) {
				// Fetch click location
				Vector3 mousePos = Input.mousePosition;
				Vector3 buttonLoc = reset.transform.position;
				float sizeX = Screen.width * 0.2f;
				float sizeY = Screen.height * 0.0375f;
					
				// Check if mouse is clicking button
				if (mousePos.x >= (buttonLoc.x - sizeX) && mousePos.x <= (buttonLoc.x + sizeX)
				&& mousePos.y >= (buttonLoc.y - sizeY) && mousePos.y <= (buttonLoc.y + sizeY)) {
					if (!buttonClick) {
						clickTime = System.DateTime.Now;
						buttonClick = true;
					}
				}
				// If detected mouse held outside of reset button zone
				else {
					buttonClick = false;
				}
			}
		}
		else {
			// Mouse released
			if (buttonClick)
				buttonClick = false;
		}
		
		if (buttonClick) {
			// Change color based on time held
			float colorVal = ((float) System.DateTime.Now.Subtract(clickTime).TotalMilliseconds)/2000.0f;
			if (colorVal > 1.0f) {
				colorVal = 1.0f;
			}
			reset.gameObject.GetComponent<Image>().color = new Color(colorVal, 1.0f, 20/255, 1.0f);
		}
		else {
			// change color back to normal
			reset.gameObject.GetComponent<Image>().color = new Color(0.0f, 1.0f, 20/255, 1.0f);
		}
    }
	
	// button resume.onclick function
	void btnResume() {
		menuState();
	}
	
	// button reset.onclick function
	void btnReset() {
		// Check button is held on for atleast 2 seconds
		//if (System.DateTime.Now.Subtract(clickTime).TotalSeconds >= 2) {
		LevelRestarter.instance.RestartLevel();
		//SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		menuState();
		buttonClick = false;
		//}
	}
	
	// button options.onclick function
	void btnOptions() {
		// Hide Screen 1
		menu.SetActive(false);
		
		// Show Screen 3
		Options.SetActive(true);
	}
	
	// button exit.onclick function
	void btnExit() {
		// Hide Screen 1
		menu.SetActive(false);
		
		// Show Screen 2
		confirm.SetActive(true);
		
	}
	
	// button yes.onclick function
	void btnYes() {
		// TODO : Set Main menu screen here when implemented
		Time.timeScale = timescale;
		confirm.SetActive(false);
		menu.SetActive(false);
		SceneManager.LoadSceneAsync("MainMenu",LoadSceneMode.Single);
	}
	
	// button no.onclick function
	void btnNo() {
		// Show Screen 1
		menu.SetActive(true);
		
		// Hide Screen 2
		confirm.SetActive(false);
	}
	
	void resumeState() {
		if (menu.activeSelf) {
			// Pause
			Time.timeScale = 0.0f;
		}
		else {
			// Resume
			Time.timeScale = timescale;
		}
	}
	
	// button back.onclick function
	void btnBack() {
		// Show Screen 1
		menu.SetActive(true);
		
		// Hide Screen 3
		Options.SetActive(false);
	}
	
	void menuState() {
		menu.SetActive(!menu.activeSelf);
		
		confirm.SetActive(false);
		
		resumeState();
	}
	

	
}
