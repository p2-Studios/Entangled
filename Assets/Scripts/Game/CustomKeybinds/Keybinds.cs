using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CustomKeybinds
{
    [Serializable]
    public class Keybinds {
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
        public KeyCode reset; // <- Added a variable here for reset key

        public Dictionary<String, KeyCode> keyCodes;

        // -- This is added to easily fetch keycodes by string! --
        public string[] keys = {
                "Space","A", "D",
									"L-Shift", "R", "F", "Q", "", "", "Esc"};

        // Load toggle
        public bool hold;

    private Keybinds() {   
            
            moveLeft = KeyCode.A;
            moveRight = KeyCode.D;
            jump = KeyCode.Space;
            
            grabRelease = KeyCode.LeftShift;
            interact = KeyCode.F;
            
            clearAllEntangled = KeyCode.Q;
            entangle = KeyCode.Mouse0;
            unentangle = KeyCode.Mouse1;
            
            pause = KeyCode.Escape;
            reset = KeyCode.R;

            keyCodes = new Dictionary<string, KeyCode>();
            
            keyCodes.Add("Move Left", moveLeft);
            keyCodes.Add("Move Right", moveRight);
            keyCodes.Add("Jump", jump);

            keyCodes.Add("Grab/Release", grabRelease);
            keyCodes.Add("Interact", interact);

            keyCodes.Add("Unentangle all", clearAllEntangled);
            keyCodes.Add("Entangle", entangle);
            keyCodes.Add("Unentangle", unentangle);
            
            keyCodes.Add("Pause", pause);
            keyCodes.Add("Reset", reset);

            hold = true;

            UpdateControls(SaveLoadKeybinds.LoadControlScheme());
        }

        private void UpdateDictionaryFromAttributes()
        {
            keyCodes["Move Left"] = moveLeft;
            keyCodes["Move Right"] = moveRight;
            keyCodes["Jump"] = jump;

            keyCodes["Grab/Release"] = grabRelease;
            keyCodes["Interact"] = interact;

            keyCodes["Unentangle all"] = clearAllEntangled;
            keyCodes["Entangle"] = entangle;
            keyCodes["Unentangle"] = unentangle;
            
            keyCodes["Pause"] = pause;
            keyCodes["Reset"] = reset;

        }

        private void UpdateAttributesFromDictionary()
        {
            moveLeft = keyCodes["Move Left"];
            moveRight = keyCodes["Move Right"];
            jump = keyCodes["Jump"];

            grabRelease = keyCodes["Grab/Release"];
            interact = keyCodes["Interact"];

            clearAllEntangled = keyCodes["Unentangle all"];
            entangle = keyCodes["Entangle"];
            unentangle = keyCodes["Unentangle"];

            pause = keyCodes["Pause"];
            reset = keyCodes["Reset"];

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
                hold = tempControls.hold;

                UpdateDictionaryFromAttributes();
            }
        }

        public void UpdateKeyBind(string action, KeyCode newKey)
        {
            keyCodes[action] = newKey;
            UpdateAttributesFromDictionary();
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