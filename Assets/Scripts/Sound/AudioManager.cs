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

    public static AudioManager instance;
    
    private void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        
        // don't destroy audio manager when switching scenes
        // todo: fix changing music between levels
        // DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        loopingSounds = new ArrayList();


        String scene = SceneManager.GetActiveScene().name;
        if (name.Equals("MainMenu")) {
            PlayLooping("music_main");
        } else if (!name.Equals("ElevatorTransition")) { // don't play sound in elevator transition
            if (UnityEngine.Random.value > 0.5) {
                PlayLooping("music_3");
            } else {
                PlayLooping("music_4");
            }
        }
    }

    public void Play(string soundName) {
        Sound s = FetchSound(soundName);
        if (s == null) return;
        if (s.source == null) return;
        
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
