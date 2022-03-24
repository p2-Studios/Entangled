using System;
using UnityEngine;

namespace Game.CustomKeybinds
{
    [Serializable]
    public class Keybinds
    {
        private static Keybinds _instance;

        public KeyCode moveLeft;
        public KeyCode moveRight;
        public KeyCode jump;

        public KeyCode grabRelease;
        public KeyCode interact;
        
        public KeyCode clearAllEntangled;
        public KeyCode entangle;
        public KeyCode unentangle;

        public KeyCode pause;
		public KeyCode reset;
		
		public string[] keys = { "Space", "A", "D",
									"E", "R", "F", "Q", "", "", "Esc"};

        private Keybinds()
        {   
            
            moveLeft = KeyCode.A;
            moveRight = KeyCode.D;
            jump = KeyCode.Space;
            
            grabRelease = KeyCode.E;
            interact = KeyCode.F;
            
            clearAllEntangled = KeyCode.Q;
            entangle = KeyCode.Mouse0;
            unentangle = KeyCode.Mouse1;
            
            pause = KeyCode.Escape;
			reset = KeyCode.R;
			
            UpdateControls(SaveLoadKeybinds.LoadControlScheme());
        }

        private void UpdateControls(Keybinds tempControls)
        {
            if (tempControls != null)
            {
                moveLeft = tempControls.moveLeft;
                moveRight = tempControls.moveRight;
                jump = tempControls.jump;

                grabRelease = tempControls.grabRelease;
                interact = tempControls.interact;

                clearAllEntangled = tempControls.clearAllEntangled;
                entangle = tempControls.entangle;
                unentangle = tempControls.unentangle;

                pause = tempControls.pause;
				reset = tempControls.reset;
				
				keys = tempControls.keys;
				
            }
        }
        
        public static Keybinds GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Keybinds();
            }
            return _instance;
        }
    }
}