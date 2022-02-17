using System.Collections;
using UnityEngine;

namespace Activation_System
{
    public class Activator : MonoBehaviour
    {
        [SerializeField] private ArrayList activatables;
        private bool activated;
        private ActivationManager activationManager;

        public Activator()
        {
            activated = false;
			activatables = new ArrayList();
            activationManager = ActivationManager.GetInstance();
        }

        public void Activate()
        {
            activated = true;
            activationManager.OnActivate(this);
        }

        public void Activate(float timer)
        {
            // TODO: wait for timer
            Activate();
        }

        public void Deactivate()
        {
            activated = false;
            activationManager.OnDeactivate(this);
        }

        public void Deactivate(float timer)
        {
            // TODO: wait for timer
            Deactivate();
        }
        
        public void ToggleState()
        {
            activated = !activated;
            if (activated)
                activationManager.OnActivate(this);
            else
                activationManager.OnDeactivate(this);
        }
        
        public void ToggleState(float timer)
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
    }
}