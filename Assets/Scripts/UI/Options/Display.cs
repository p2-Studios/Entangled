using Game.CustomKeybinds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Display : MonoBehaviour
{
	public Dropdown res_dropdown;
	public Slider brightness;
	public Toggle fullscreen;
	public Button back;
	
	public GameObject Option_screen;
	public GameObject Display_screen;

	List<Resolution> resolution;
	List<string> res_string;

	// Start is called before the first frame update
	void Start() {
		
		back.onClick.AddListener(btnBack);

		resolution = new List<Resolution>();
		res_string = new List<string>();
		add_res();
		res_dropdown.onValueChanged.AddListener(delegate {
            change_res();
        });

		brightness.value = Screen.brightness;
		brightness.onValueChanged.AddListener(delegate {
            change_brightness();
        });

		fullscreen.isOn = Screen.fullScreen;
		fullscreen.onValueChanged.AddListener(delegate {
            change_fullScreen();
        });
	}
	
	void Update() {
		if (Input.GetKeyDown(Keybinds.GetInstance().pause)) {
			btnBack();
		}
	}
	
	// Sets resolution when dropdown changes
	void change_res() {
		Resolution r = resolution[res_dropdown.value];
		Screen.SetResolution(r.width, r.height, fullscreen.isOn);
	}
	
	// Changes screen brightness when slider changes
	void change_brightness() {
		Screen.brightness = brightness.value;
	}
	
	// Toggle fullscreen
	void change_fullScreen() {
		Screen.fullScreen = !Screen.fullScreen;
	}
	
	void add_res() {

		Resolution[] resolutions = Screen.resolutions;

		foreach (Resolution r in resolutions) {
			if (!res_string.Contains(r.width + " x " + r.height)) {
				resolution.Add(r);
				res_string.Add(r.width + " x " + r.height);
			}
		}

		res_dropdown.AddOptions(res_string);

		res_dropdown.value = res_string.IndexOf(Screen.width + " x " + Screen.height);

    }

	// back button
	void btnBack() {
		Option_screen.SetActive(true);
		
		Display_screen.SetActive(false);
	}
	
}
