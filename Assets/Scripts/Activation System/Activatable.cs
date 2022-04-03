using System;
using System.Collections;
using UnityEngine;

namespace Activation_System {
    public class Activatable : MonoBehaviour {
        [SerializeField]
        private ArrayList activators; // this is only required if we choose to keep 'requireAllActivators'

        protected bool activated;
        public bool requireAllActivators = false; // TODO: Keep 'requireAllActivators'? 
        public bool activateByDefault;

        protected bool playSound = true;
        public float soundDelay = 0.0f;
        
        public string activateSound, deactivateSound;
        
        //public bool invertable; // if invertable, being activated will turn it off, and vice versa

        public Activatable() {
            activators = new ArrayList();
            activated = activateByDefault;
            requireAllActivators = false;
            if (activated) Activate();
        }

        public virtual void AddActivator(Activator activator) {
            activators.Add(activator);
        }

        public virtual void RemoveActivator(Activator activator) {
            activators.Remove(activator);
        }

        public virtual ArrayList GetActivators() {
            return activators;
        }

        public virtual void Activate() {
            if (requireAllActivators) {
                if (AreAllActivatorsActivated()) {
                    activated = true;
                    PlaySound(activateSound);
                }
            } else {
                activated = true;
                PlaySound(activateSound);
            }
        }

        public virtual void Deactivate() {
            if (requireAllActivators) {
                if (AreAllActivatorsDeactivated()) {
                    activated = false;
                    PlaySound(deactivateSound);
                }
            } else {
                activated = false;
                PlaySound(deactivateSound);
            }
        }

        public virtual void ToggleState() {
            if (activated)
                Deactivate();
            else {
                if (requireAllActivators && !AreAllActivatorsActivated()) return; 
                Activate();
            }
        }

        public virtual void ToggleState(float timer) {
            // TODO: Wait for timer
            ToggleState();
        }

        public virtual bool IsActivated() {
            return activated;
        }

        public virtual bool AreAllActivatorsActivated() {
            foreach (Activator activator in activators) {
                if (!activator.IsActivated())
                    return false;
            }

            return true;
        }

        public virtual bool AreAllActivatorsDeactivated() {
            foreach (Activator activator in activators) {
                if (activator.IsActivated())
                    return false;
            }

            return true;
        }
        
        public void PlaySound(String soundName) {
            if (playSound) {
                AudioManager.instance.PlayDelayed(soundName, soundDelay);
            } else {    // if sound was not enabled, enable it now
                playSound = true;
            }
        }
    }
}