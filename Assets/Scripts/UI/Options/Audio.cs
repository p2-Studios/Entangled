using Game.CustomKeybinds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Audio : MonoBehaviour
{
	public AudioManager sound;
	
	public Slider volume;
	public Toggle mute;
	public Slider balance;
	public Button back;
	
	public GameObject Option_screen;
	public GameObject Audio_screen;

	public bool soundFound = false;
	
    // Start is called before the first frame update
    void Start()
    {
		if (FindObjectsOfType<AudioManager>().Length > 0) {
			sound = FindObjectsOfType<AudioManager>()[0];
			soundFound = true;
		}
        back.onClick.AddListener(btnBack);
		if (soundFound) {
			volume.value = sound.sounds[0].source.volume;
			volume.onValueChanged.AddListener(delegate {
				change_volume();
			});
			mute.isOn = sound.sounds[0].source.mute;
			mute.onValueChanged.AddListener(delegate {
				toggle_mute();
			});
			balance.value = sound.sounds[0].source.panStereo;
			balance.onValueChanged.AddListener(delegate {
				change_balance();
			});
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Keybinds.GetInstance().pause)) {
			btnBack();
		}
    }
	
	// changes volume when value change using slider
	void change_volume() {
		 foreach (Sound s in sound.sounds) {
            s.source.volume = volume.value;
        }
	}
	
	// toggle mute on or off
	void toggle_mute() {
		foreach (Sound s in sound.sounds) {
            s.source.mute = !s.source.mute;
        }
	}
	
	// changes volume balancing between L and R using slider
	void change_balance() {
		foreach (Sound s in sound.sounds) {
            s.source.panStereo = balance.value;
        }
	}
	
	// back button
	void btnBack() {
		Option_screen.SetActive(true);
		
		Audio_screen.SetActive(false);
	}
	
}
