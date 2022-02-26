using System;
using UnityEngine.Audio;
using UnityEngine;

// tutorial: https://www.youtube.com/watch?v=6OT43pvUyfY
public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;
    
    private void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        
        // don't destroy audio manager when switching scenes
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string soundName) {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null) {
            Debug.LogWarning("Error: Sound " + soundName + " not found.");
            return;
        }
        s.source.Play();
    }
}
