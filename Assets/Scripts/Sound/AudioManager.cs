using System;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

// tutorial: https://www.youtube.com/watch?v=6OT43pvUyfY
public class AudioManager : MonoBehaviour {
    
    public Sound[] sounds;
    public Sound[] music;
    public Sound[] uiSounds;
    private ArrayList loopingSounds;
    private Sound currentSong;
    private AudioSource musicPlayer;
    private static string _musicType = "menu";

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
        SetMusicType(sceneName);
        ChangeMusic();
    }
    

    private void Update() {
        if (musicPlayer != null && !musicPlayer.isPlaying) {    // start new song if last song has ended
            ChangeMusic();
            
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneChange;
    }

    private void ChangeMusic() {
        AudioSource source;
        String song;
        switch (_musicType) {
            case "menu":    // menu music (main menu, level selection, etc.)
                song = "music_main";
                break;
            
            case "level":   // music while in-level
                if (UnityEngine.Random.value > 0.5) {
                    song = "music_3";
                } else {
                    song = "music_4";
                }
                break;
            
            case "credits": // credits music
                song = "music_main";    // TODO: change to credit music
                break;
            
            default:
                song = "music_main";
                break;
        }
        
        if (musicPlayer != null) musicPlayer.Stop();
        source = GetSource(song);
        if (source != null) {
            musicPlayer = source;
            musicPlayer.ignoreListenerPause = true;
            source.Play();
        }
    }

    private void OnSceneChange(Scene scene, LoadSceneMode mode) {
        SetMusicType(scene.name);
    }

    private void SetMusicType(string sceneName) {
        if (sceneName.Equals("MainMenu") || sceneName.Equals("LevelSelection")) {
            TestAndChangeMusic("menu");
        } else if (sceneName.Equals("Credits")) { 
            TestAndChangeMusic("credits");
        } else if (!sceneName.Equals("ElevatorTransition") && !sceneName.Equals("LevelSelection")) { // don't play sound in elevator transition
            TestAndChangeMusic("level");  // only change is not already playing level music
        }
    }
    
    private void TestAndChangeMusic(String type) {
        if (!_musicType.Equals(type)) { 
            _musicType = type;
            ChangeMusic();
        }
    }

    public void Play(string soundName) {
        AudioSource source = GetSource(soundName);
        if (source != null) source.PlayOneShot(source.clip);
    }

    public AudioSource GetSource(string soundName) {
        Sound s = FetchSound(soundName);
        if (s == null) return null;
        return s.source;
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

    public void InitializeSounds() {
        InitializeSoundGroup(sounds);
        InitializeSoundGroup(music);
        InitializeSoundGroup(uiSounds);
    }

    public void InitializeSoundGroup(Sound[] soundGroup) {
        foreach (Sound s in soundGroup) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }
}
