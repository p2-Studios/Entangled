﻿using System;
using System.Collections;
using UnityEngine;

namespace Activation_System
{
    public class Activator : MonoBehaviour
    {
        [SerializeField] private ArrayList activatables;
        private bool activated;
        public bool silent, muted;
        private ActivationManager activationManager;

        public string activateSound, deactivateSound;

        public void Start() {
            StartCoroutine(Mute(1f));
        }

        private IEnumerator Mute(float time) {
            muted = true;
            yield return new WaitForSeconds(time);
            muted = false;
        }
        
        public Activator()
        {
            activated = false;
			activatables = new ArrayList();
            activationManager = ActivationManager.GetInstance();
        }

        public virtual void Activate()
        {
            activated = true;
            activationManager.OnActivate(this);
            if (!(muted || silent)) {
                AudioManager.instance.Play(activateSound);
            }
        }

        public virtual void Activate(float timer)
        {
            // TODO: wait for timer
            Activate();
        }

        public virtual void Deactivate()
        {
            activated = false;
            activationManager.OnDeactivate(this);
            if (!(muted || silent)) {
                AudioManager.instance.Play(deactivateSound);
            }
        }

        public virtual void Deactivate(float timer)
        {
            // TODO: wait for timer
            Deactivate();
        }
        
        public virtual void ToggleState()
        {
            activated = !activated;
            if (activated)
                activationManager.OnActivate(this);
            else
                activationManager.OnDeactivate(this);
        }
        
        public virtual void ToggleState(float timer)
        {
            // TODO: Wait for timer
            ToggleState();
        }

        public bool IsActivated()
        {
            return activated;
        }

        public ArrayList GetActivatables()
        {
            return activatables;
        }

        public void AddActivatable(Activatable activatable)
        {
            activatables.Add(activatable);
        }

        public void RemoveActivatable(Activatable activatable)
        {
            activatables.Remove(activatable);
        }
        
        // draw lines between activator and its activatables
        /*
        private void OnDrawGizmos() {
            foreach (Activatable a in activatables) {
                Gizmos.DrawLine(transform.position, a.transform.position);
            }
        }
        */
    }
}