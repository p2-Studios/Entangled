namespace Activation_System
{
    public class ActivationManager
    {
        private static ActivationManager instance;

        private ActivationManager() {}
        
        public static ActivationManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ActivationManager();
            }
            return instance;
        }
        public void OnActivate(Activator a)
        {
            foreach (Activatable activatable in a.GetActivatables())
            {
                activatable.Activate();
            }
        }

        public void OnDeactivate(Activator a)
        {
            foreach (Activatable activatable in a.GetActivatables())
            {
                activatable.Deactivate();
            }
        }
    }
}