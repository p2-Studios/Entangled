using System;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// tutorial: https://www.youtube.com/watch?v=6OT43pvUyfY
public class AudioManager : MonoBehaviour {
    
    public Sound[] miscSounds, music, uiSounds, environmentSounds, playerSounds, objectSounds;
    public Sound[] sounds;
    public ArrayList allSounds;
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
        
        InitializeSounds();

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
            
            case "elevator":   // music while in-level
                song = "music_elevator";
                break;
            
            case "level":   // music while in-level
                if (Random.value > 0.5) {
                    song = "music_3";
                } else {
                    song = "music_4";
                }
                break;
            
            case "level9": // level9 music
                song = "music_level9";
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
        if (sceneName.Equals("Options")) {  // don't change music when opening options scene
            return;
        }
        
        if (sceneName.Equals("MainMenu") || sceneName.Equals("LevelSelection")) {
            TestAndChangeMusic("menu");
        } else if (sceneName.Equals("Credits")) {   
            TestAndChangeMusic("credits");
        } else if (sceneName.Equals("ElevatorTransition")) { 
            TestAndChangeMusic("elevator");
        } else if (sceneName.Contains("Level9")) { 
            TestAndChangeMusic("level9");
        } else { 
            TestAndChangeMusic("level");
        }
    }
    
    private void TestAndChangeMusic(String type) {
        if (!_musicType.Equals(type)) { 
            _musicType = type;
            ChangeMusic();
        }
    }

    public void Play(string soundName) {
        Sound sound = FetchSound(soundName);
        if (sound != null) {
            AudioSource source = GetSource(sound);
            if (source != null) {
                SetSourcePitch(source, sound);
                source.PlayOneShot(source.clip);
            }
        }
    }

    public void PlayLooping(string soundName) {
        Sound sound = FetchSound(soundName);
        if (sound != null) {
            AudioSource source = GetSource(sound);
            if (source != null) {
                SetSourcePitch(source, sound);
                source.loop = true;
                source.Play();
            }
        }
    }

    public void StopLooping(string soundName) {
        Sound sound = FetchSound(soundName);
        if (sound != null) {
            AudioSource source = GetSource(sound);
            if (source != null) {
                source.loop = false;
                source.Stop();
            }
        }
    }

    public void PlayDelayed(string soundName, float delay) {
        Sound sound = FetchSound(soundName);
        if (sound != null) {
            AudioSource source = GetSource(sound);
            if (source != null) {
                SetSourcePitch(source, sound);
                source.PlayDelayed(delay);
            }
        }
    }

    public void SetSourcePitch(AudioSource source, Sound s) {
        source.pitch = Random.Range(s.pitchMin, s.pitchMax);
    }

    public AudioSource GetSource(string soundName) {
        Sound s = FetchSound(soundName);
        if (s == null) return null;
        return s.source;
    }
    
    public AudioSource GetSource(Sound sound) {
        if (sound == null) return null;
        return sound.source;
    }

    public void Play(Sound sound) {
        sound.source.Play();
    }

    private Sound FetchSound(String soundName) {
        if (soundName.Length == 0) return null;
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null) {
            Debug.LogWarning("Error: Sound " + soundName + " not found.");
            return null;
        }

        return s;
    }

    public void InitializeSounds() {
        allSounds = new ArrayList();
        InitializeSoundGroup(miscSounds);
        InitializeSoundGroup(music);
        InitializeSoundGroup(uiSounds);
        InitializeSoundGroup(environmentSounds);
        InitializeSoundGroup(playerSounds);
        InitializeSoundGroup(objectSounds);
        sounds = (Sound[]) allSounds.ToArray(typeof(Sound));    // put all of the sounds into an array
    }

    public void InitializeSoundGroup(Sound[] soundGroup) {
        foreach (Sound s in soundGroup) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            allSounds.Add(s);
        }
    }
}
