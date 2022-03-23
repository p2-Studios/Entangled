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
	
	// Start is called before the first frame update
	void Start() {
		
		back.onClick.AddListener(btnBack);
		res_dropdown.value = find_res(Screen.width, Screen.height);
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
		Screen.SetResolution(get_res(res_dropdown.value)[0],get_res(res_dropdown.value)[1], fullscreen.isOn);
	}
	
	// Changes screen brightness when slider changes
	void change_brightness() {
		Screen.brightness = brightness.value;
	}
	
	// Toggle fullscreen
	void change_fullScreen() {
		Screen.fullScreen = !Screen.fullScreen;
	}
	
	// fetches resolution of screen by value
	int[] get_res(int val) {
		
		switch (val) {
			case 0 : return new int [] {1920,1080};
			case 1 : return new int [] {1366, 768};
			case 2 : return new int [] {1280, 720};
			case 3 : return new int [] {800 , 600};
			case 4 : return new int [] {640 , 360};
			default : return new int [] {1920,1080};
		}
		
	}
	
	// fetches corresponding value by resolution
	int find_res(int width, int height) {
		
		switch ((width, height)) {
			case (1920,1080) : return 0;
			case (1366, 768) : return 1;
			case (1280, 720) : return 2;
			case (800, 600) : return 3;
			case (640 , 360) : return 4;
			default : return 0;
		}
		
	}
	
	// back button
	void btnBack() {
		Option_screen.SetActive(true);
		
		Display_screen.SetActive(false);
	}
	
}
