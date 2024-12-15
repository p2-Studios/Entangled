using Activation_System;
using UnityEngine;

using Activator = Activation_System.Activator;

public class DestructionField : Activatable {
    public bool destroyPlayer;
    public bool destroyObjects;
    public bool clearEntanglement;

    public Animator animator;
    public Activator[] activators;
    public UnityEngine.Rendering.Universal.Light2D glowLight;
    public AudioSource audioSource;
    
    private AudioManager audioManager;

    public string deathSoundPlayer, deathSoundObject;
    
    void Start() {
        foreach (Activator a in activators) {
            AddActivator(a);
        }

        if (activateByDefault) {
            Activate();
        } else {
            Deactivate();
        }

        audioManager = FindObjectOfType<AudioManager>();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Activate() {
        base.Activate();
        if (glowLight != null) glowLight.enabled = true;
        if (animator != null) animator.SetBool("isActive", true);
        if (audioSource != null) audioSource.mute = false;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Deactivate() {
        base.Deactivate();
        if (glowLight != null) glowLight.enabled = false;
        if (animator != null) animator.SetBool("isActive", false);
        if (audioSource != null) audioSource.mute = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!activated) return;
        Player p = other.GetComponent<Player>();
        Entanglable e = other.GetComponent<Entanglable>();
        
        if (p != null) {
            if (clearEntanglement) {
                p.entangleComponent.ClearEntangled();
            }
            if (destroyPlayer) {
                p.Kill();
                audioManager.Play(deathSoundPlayer);
            } else {
                p.entangleComponent.ClearEntangled();
            }
        }

        if (e != null) {
            if (destroyObjects) {
                e.Kill();
                audioManager.Play(deathSoundObject);
            }
            if (clearEntanglement) {
                Player pl = FindObjectOfType<Player>();
                if (pl != null) pl.entangleComponent.ClearEntangled();
            }
        }
    }
}