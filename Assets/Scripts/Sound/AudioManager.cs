using System;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

// tutorial: https://www.youtube.com/watch?v=6OT43pvUyfY
public class AudioManager : MonoBehaviour {
    
    public Sound[] sounds;
    private ArrayList loopingSounds;
    private Sound currentSong;
    public bool restartSong = true;

    public static AudioManager instance;
    
    private void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        loopingSounds = new ArrayList();

        
        String sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Equals("MainMenu")) {
            PlayMusic("music_main");
        } else if (!sceneName.Equals("ElevatorTransition")) { // don't play sound in elevator transition
            if (UnityEngine.Random.value > 0.5) {
                PlayMusic("music_3");
            } else {
                PlayMusic("music_4");
            }
        }
    }
    
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneChange;
    }

    private void OnSceneChange(Scene scene, LoadSceneMode mode) {
        if (!restartSong) { // if restartSong flag is false, don't restart song on scene load/reload
            restartSong = true; // set to true again so that the next scene change will restart the song, unless told not to
            return; 
        }
        String sceneName = scene.name;
        
        if (sceneName.Equals("MainMenu")) {
            PlayMusic("music_main");
        } else if (!sceneName.Equals("ElevatorTransition") && !sceneName.Equals("LevelSelection")) { // don't play sound in elevator transition
            if (UnityEngine.Random.value > 0.5) {
                PlayMusic("music_3");
            } else {
                PlayMusic("music_4");
            }
        }

    }

    public void Play(string soundName) {
        Sound s = FetchSound(soundName);
        if (s == null) return;
        if (s.source == null) return;
        
        s.source.Play();
    }

    public void PlayMusic(string songName) {
        if (currentSong != null) {  // stop other song playing
            currentSong.source.loop = false;
            currentSong.source.Stop();
        }
        
        Sound s = FetchSound(songName);
        if (s == null) return;
        if (s.source == null) return;

        currentSong = s;
        s.source.loop = true;
        s.source.Play(); 
    }
    
    public void PlayLooping(string soundName) {
        Sound s = FetchSound(soundName);
        if (s == null) return;
        if (s.source == null) return;

        s.source.loop = true;
        s.source.Play();
    }

    
    public void Play(Sound sound) {
        sound.source.Play();
    }

    public void StartLoopingSound(String soundName, float delay) {
        if (delay < 0.01) { // don't allow extremely short delays
            Debug.LogWarning("Looping delays less than 0.01s not allowed.");
            return;
        }
        
        Sound s = FetchSound(soundName);
        if (s == null) return;

        loopingSounds.Add(s);
        StartCoroutine(LoopSound(s, delay));
    }

    public void StopLoopingSound(String soundName) {
        Sound s = FetchSound(soundName);
        if (s == null) return;
        loopingSounds.Remove(s);
    }

    private IEnumerator LoopSound(Sound sound, float delay) {
        while (loopingSounds.Contains(sound)) {
            Play(sound);
            yield return new WaitForSeconds(delay);
        }
    }

    private Sound FetchSound(String soundName) {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null) {
            Debug.LogWarning("Error: Sound " + soundName + " not found.");
            return null;
        }

        return s;
    }

    public Boolean IsLooping(String soundName) {
        Sound s = FetchSound(soundName);
        if (s == null) return false;
        foreach (var sound in loopingSounds) {
            Debug.Log(sound.ToString());
        }
        return loopingSounds.Contains(s);
    }
}
