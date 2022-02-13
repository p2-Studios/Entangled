using System.Collections;
using UnityEngine;

namespace Activation_System
{
    public class Activatable : MonoBehaviour
    {
        [SerializeField] private ArrayList activators; // this is only required if we choose to keep 'requireAllActivators'
        private bool activated;
        private bool requireAllActivators; // TODO: Keep 'requireAllActivators'? 
        

        public Activatable()
        {
            activators = new ArrayList();
            activated = false;
            requireAllActivators = false;
        }

        public void AddActivator(Activator activator)
        {
            activators.Add(activator);
        }

        public void RemoveActivator(Activator activator)
        {
            activators.Remove(activator);
        }

        public ArrayList GetActivators()
        {
            return activators;
        }

        public void Activate()
        {
            if (requireAllActivators)
            {
                if (AreAllActivatorsActivated())
                    activated = true;
            }
            else 
                activated = true;
                
        }

        public void Activate(float timer)
        {
            // TODO: Wait for timer
            Activate();
        }

        public void Deactivate()
        {
            if (requireAllActivators)
            {
                if (AreAllActivatorsDeactivated())
                    activated = false;
            }
            else
                activated = false;
        }

        public void Deactivate(float timer)
        {
            // TODO: Wait for timer
            Deactivate();
        }

        public void ToggleState()
        {
            if (activated)
                Deactivate();
            else
                Activate();
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

        public bool AreAllActivatorsActivated()
        {
            foreach (Activator activator in activators)
            {
                if (!activator.IsActivated())
                    return false;
            }
            return true;
        }
        
        public bool AreAllActivatorsDeactivated()
        {
            foreach (Activator activator in activators)
            {
                if (activator.IsActivated())
                    return false;
            }
            return true;
        }
    }
}