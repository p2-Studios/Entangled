using System;
using UnityEngine;

namespace Entanglement {
    public class VelocityManager
    {
        public static VelocityManager instance;

        private VelocityManager() {}

        public static VelocityManager GetInstance() {
            if (instance == null) {
                instance = new VelocityManager();
            }
            return instance;
        }

        public event Action<Entanglable, Vector2> onActiveMoved;
        /// <summary>
        /// Notifies EntangleComponent of velocities applied
        /// </summary>
        public void ActiveMoved(Entanglable e, Vector2 velocity) {
            if (onActiveMoved != null) {
                onActiveMoved(e, velocity);
            }
        }
    }
}
