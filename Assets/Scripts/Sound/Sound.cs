using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tutorial: https://www.youtube.com/watch?v=6OT43pvUyfY
[System.Serializable]
public class Sound {

    public string name;
    
    public AudioClip clip;
    [HideInInspector]
    public AudioSource source;
    [Range(0f,1f)]
    public float volume = 1.0f;
    [Range(0.1f,3f)]
    public float pitch = 1.0f;
    
}
