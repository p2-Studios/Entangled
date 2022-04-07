using Game.CustomKeybinds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Audio : MonoBehaviour
{
	public AudioManager sound;
	
	public Slider Effectvolume;
	public Slider Musicvolume;
	public Toggle mute;
	public Slider balance;
	public Button back;
	
	public GameObject Option_screen;
	public GameObject Audio_screen;

	private bool soundFound = false;
	
    // Start is called before the first frame update
    void Start()
    {
		if (FindObjectsOfType<AudioManager>().Length > 0) {
			sound = FindObjectsOfType<AudioManager>()[0];
			soundFound = true;
		}
        back.onClick.AddListener(btnBack);
		if (soundFound) {
			if (PlayerPrefs.HasKey("fxVolume"))
				Effectvolume.value = PlayerPrefs.GetFloat("fxVolume");
			else
				Effectvolume.value = sound.sounds[0].source.volume;
			Effectvolume.onValueChanged.AddListener(delegate {
				change_fxvolume();
			});
			if (PlayerPrefs.HasKey("MusicVolume"))
				Musicvolume.value = PlayerPrefs.GetFloat("MusicVolume");
			else
				Musicvolume.value = sound.music[0].source.volume;
			Musicvolume.onValueChanged.AddListener(delegate {
				change_Musicvolume();
			});
			if (PlayerPrefs.HasKey("mute")) {
				if (PlayerPrefs.GetInt("mute") == 1)
					mute.isOn = true;
				else
					mute.isOn = false;
			}
			else
				mute.isOn = sound.sounds[0].source.mute;
			mute.onValueChanged.AddListener(delegate {
				toggle_mute();
			});
			if (PlayerPrefs.HasKey("balance")) {
				balance.value = PlayerPrefs.GetFloat("balance");
			}
			else
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
	
	// changes fxvolume when value change using slider
	void change_fxvolume() {
		foreach (Sound s in sound.miscSounds) {
            s.source.volume = Effectvolume.value;
        }
		foreach (Sound s in sound.uiSounds) {
			s.source.volume = Effectvolume.value;
		}
		foreach (Sound s in sound.environmentSounds) {
			s.source.volume = Effectvolume.value;
		}
		foreach (Sound s in sound.playerSounds) {
			s.source.volume = Effectvolume.value;
		}
		foreach (Sound s in sound.objectSounds) {
			s.source.volume = Effectvolume.value;
		}
		PlayerPrefs.SetFloat("fxVolume", Effectvolume.value);
		PlayerPrefs.Save();
	}

	// changes music volume
	void change_Musicvolume() {
		foreach (Sound s in sound.music) {
			s.source.volume = Musicvolume.value;
		}
		PlayerPrefs.SetFloat("MusicVolume", Musicvolume.value);
		PlayerPrefs.Save();
	}

	// toggle mute on or off
	void toggle_mute() {
		foreach (Sound s in sound.sounds) {
            s.source.mute = mute.isOn;
		}
		foreach (Sound s in sound.music) {
			s.source.mute = mute.isOn;
		}

		if (mute.isOn)
			PlayerPrefs.SetInt("Mute", 1);
		else
			PlayerPrefs.SetInt("Mute", 0);
		PlayerPrefs.Save();
	}
	
	// changes volume balancing between L and R using slider
	void change_balance() {
		foreach (Sound s in sound.sounds) {
            s.source.panStereo = balance.value;
		}
		foreach (Sound s in sound.music) {
			s.source.panStereo = balance.value;
		}
		PlayerPrefs.SetFloat("Balance", balance.value);
		PlayerPrefs.Save();
	}
	
	// back button
	void btnBack() {
		Option_screen.SetActive(true);
		
		Audio_screen.SetActive(false);
	}
	
}
