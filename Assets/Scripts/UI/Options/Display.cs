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
	List<Dropdown.OptionData> res_Options;

	// Start is called before the first frame update
	void Start() {
		
		back.onClick.AddListener(btnBack);

		resolution = new List<Resolution>();
		res_Options = new List<Dropdown.OptionData>();
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
		
		print("Option value: " + res_dropdown.value + "Res:" + r.width + " x " + r.height);
		Screen.SetResolution(r.width, r.height, fullscreen.isOn);
		res_dropdown.RefreshShownValue();
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
			if (checkNotContains(r)) {
				resolution.Add(r);
				Dropdown.OptionData newData = new Dropdown.OptionData();
				newData.text = r.width + " x " + r.height;
				res_Options.Add(newData);
			}
		}

		res_dropdown.ClearOptions();
		res_dropdown.AddOptions(res_Options);

		res_dropdown.value = findIndex(Screen.currentResolution);
		res_dropdown.RefreshShownValue();
    }

	bool checkNotContains(Resolution r) {
		foreach (Resolution res in resolution) {
			if (res.width == r.width && res.height == r.height)
				return false;
        }
		return true;
    }

	int findIndex(Resolution r) {
		int value = 0;
		foreach (Resolution res in resolution) {
			if (res.width == r.width && res.height == r.height)
				return value;
			value++;
		}
		return 0;
	}

	// back button
	void btnBack() {
		Option_screen.SetActive(true);
		
		Display_screen.SetActive(false);
	}
	
}
