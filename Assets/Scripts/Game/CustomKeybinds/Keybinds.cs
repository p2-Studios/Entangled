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
        public KeyCode moveUp;
        public KeyCode moveDown;
        public KeyCode jump;

        public KeyCode grabRelease;
        public KeyCode interact;
        
        public KeyCode clearAllEntangled;
        public KeyCode entangle;
        public KeyCode unentangle;

        public KeyCode pause;

        private Keybinds()
        {   
            
            moveLeft = KeyCode.A;
            moveRight = KeyCode.D;
            moveUp = KeyCode.W;
            moveDown = KeyCode.S;
            jump = KeyCode.Space;
            
            grabRelease = KeyCode.E;
            interact = KeyCode.F;
            
            clearAllEntangled = KeyCode.Q;
            entangle = KeyCode.Mouse0;
            unentangle = KeyCode.Mouse1;
            
            pause = KeyCode.Escape;

            UpdateControls(SaveLoadKeybinds.LoadControlScheme());
        }

        private void UpdateControls(Keybinds tempControls)
        {
            if (tempControls != null)
            {
                moveLeft = tempControls.moveLeft;
                moveRight = tempControls.moveRight;
                moveUp = tempControls.moveUp;
                moveDown = tempControls.moveDown;
                jump = tempControls.jump;

                grabRelease = tempControls.grabRelease;
                interact = tempControls.interact;

                clearAllEntangled = tempControls.clearAllEntangled;
                entangle = tempControls.entangle;
                unentangle = tempControls.unentangle;

                pause = tempControls.pause;
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