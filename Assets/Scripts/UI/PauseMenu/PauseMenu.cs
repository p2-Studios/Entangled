using System;
using System.Linq;
using Game.CustomKeybinds;
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
	public Button feedback;
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

	// Paused?
	public bool paused = false;

	// Click Timer
	private DateTime clickTime;
	private bool buttonClick = false;

	public String[] exemptScenes;
	
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
		feedback.onClick.AddListener(btnFeedback);
		exit.onClick.AddListener(btnExit);
		
		yes.onClick.AddListener(btnYes);
		no.onClick.AddListener(btnNo);
		
		timescale = Time.timeScale;

		loadSettings();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(Keybinds.GetInstance().pause)) {
	        if (!exemptScenes.Contains(SceneManager.GetActiveScene().name)) { // don't allow opening in main menu
		        HintManager hm = HintManager.instance;
		        if (hm != null) hm.CloseHint();
		        
		        TerminalManager dm = TerminalManager.instance;
		        if (!(dm == null) && !dm.IsTerminalOpen()) { // don't allow opening while in a dialogue (Escape exits dialogue)
			        menuState();
		        }
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
		LevelRestarter.instance.RestartLevel();
		menuState();
		buttonClick = false;
		//}
	}
	
	// button options.onclick function
	void btnOptions() {
		SceneManager.LoadSceneAsync("Options", LoadSceneMode.Additive);
	}
	
	void btnFeedback() {
		Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSduizPyNvgwBKM6RKQIJBMhdP-MfVxOmvlQN4bWaZeT_3VL7Q/viewform");
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
		Time.timeScale = timescale;
		paused = false;
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
		if (paused) {
			// Pause
			Time.timeScale = 0.0f;
		}
		else {
			// Resume
			Time.timeScale = timescale;
		}
	}
	
	void menuState() {

		if (paused) {
			menu.SetActive(false);
			confirm.SetActive(false);
		}
		else {
			menu.SetActive(true);
		}

		paused = !paused;

		resumeState();
	}


	public void OpenOptions() {
		btnOptions();
	}
	
	void loadSettings() {
		AudioManager sound = null;
		if (FindObjectsOfType<AudioManager>().Length > 0) {
			sound = FindObjectsOfType<AudioManager>()[0];
		}
		if (PlayerPrefs.HasKey("fxVolume")) {
			if (sound != null) {
				foreach (Sound s in sound.miscSounds) {
					s.source.volume = PlayerPrefs.GetFloat("fxVolume");
				}
				foreach (Sound s in sound.uiSounds) {
					s.source.volume = PlayerPrefs.GetFloat("fxVolume");
				}
				foreach (Sound s in sound.environmentSounds) {
					s.source.volume = PlayerPrefs.GetFloat("fxVolume");
				}
				foreach (Sound s in sound.playerSounds) {
					s.source.volume = PlayerPrefs.GetFloat("fxVolume");
				}
				foreach (Sound s in sound.objectSounds) {
					s.source.volume = PlayerPrefs.GetFloat("fxVolume");
				}
			}
        }
		if (PlayerPrefs.HasKey("MusicVolume")) {
			if (sound != null) {
				foreach (Sound s in sound.music) {
					s.source.volume = PlayerPrefs.GetFloat("MusicVolume");
				}
			}
		}
		if (PlayerPrefs.HasKey("Mute")) {
			bool mute = false;
			if (PlayerPrefs.GetInt("Mute") == 1)
				mute = true;
			if (sound != null) {
				foreach (Sound s in sound.sounds) {
					s.source.mute = mute;
				}
				foreach (Sound s in sound.music) {
					s.source.mute = mute;
				}
			}
		}
		if (PlayerPrefs.HasKey("Balance")) {
			if (sound != null) {
				foreach (Sound s in sound.sounds) {
					s.source.panStereo = PlayerPrefs.GetFloat("Balance");
				}
				foreach (Sound s in sound.music) {
					s.source.panStereo = PlayerPrefs.GetFloat("Balance");
				}
			}
		}
	}

}
