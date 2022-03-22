
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour {
    public string label;
    [HideInInspector] public FlashDrive[] flashDrives;
    public ArrayList foundFlashDrives;

    [HideInInspector] public Orb[] orbs;
    public ArrayList collectedOrbs;

    public void Awake() {
        flashDrives = FindObjectsOfType<FlashDrive>();  // find all flashdrives in the scene
        
        LoadLevelData();    // load any possible existing level data
    }

    private void OnDestroy() {
        SaveLevelData();
    }

    public void LoadLevelData() {
        LevelData data = SaveSystem.LoadLevel(label);
        if (data == null) { // no level data found, create data
            flashDrives = (FlashDrive[]) GameObject.FindObjectsOfType(typeof(FlashDrive));
            orbs = (Orb[]) GameObject.FindObjectsOfType(typeof(Orb));
            
            foundFlashDrives = new ArrayList();
            collectedOrbs = new ArrayList();
        } else {
            // load any orbs and flashdrives the player has already found 
            foundFlashDrives = GetFoundFlashDrivesFromIDs(data.foundFlashDrives);
            collectedOrbs = GetCollectedOrbsFromIDS(data.foundOrbs);

            foreach (FlashDrive f in foundFlashDrives) {    // disable already found flashdrives
                if (flashDrives.Contains(f)) f.gameObject.transform.position = new Vector3(1000, 1000, 1000);   // move far away to make inaccessible
            }
        }
    }

    public void SaveLevelData() {
        SaveSystem.SaveLevel(this);
    }

    public ArrayList GetFoundFlashDrivesFromIDs(string[] ids) {
        ArrayList found = new ArrayList();
        foreach (String s in ids) {
            foreach (FlashDrive f in flashDrives) {
                if (f.GetID().Equals(s)) found.Add(f);
            }
        }

        return found;
    }
    
    public ArrayList GetCollectedOrbsFromIDS(string[] ids) {
        ArrayList collected = new ArrayList();
        foreach (String s in ids) {
            foreach (Orb o in orbs) {
                if (o.GetID().Equals(s)) collected.Add(o);
            }
        }

        return collected;
    }

    
    public string[] GetFoundFlashDriveIDs() {
        ArrayList IDs = new ArrayList();
        foreach (FlashDrive f in foundFlashDrives) {
            IDs.Add(f.GetID());
        }
        return (string[]) IDs.ToArray(typeof(string));
    }
    
    public string[] GetCollectedOrbIDS() {
        ArrayList IDs = new ArrayList();
        foreach (Orb o in collectedOrbs) {
            IDs.Add(o.GetID());
        }
        return (string[]) IDs.ToArray(typeof(string));
    }

    public void FlashDriveFound(FlashDrive f) {
        foundFlashDrives.Add(f);
        f.gameObject.SetActive(false);
    }

    public void OrbCollected(Orb o) {
        collectedOrbs.Add(o);
        o.gameObject.SetActive(false);
    }

    public int GetTotalOrbCount() {
        return orbs.Length;
    }

    public int GetFoundOrbsCount() {
        return collectedOrbs.Count;
    }

    public FlashDrive[] GetFlashDrives() {
        return flashDrives;
    }
}
