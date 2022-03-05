using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionManager : MonoBehaviour {

    public static DestructionManager instance;
    
    private void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        
        // don't destroy DestructionManager when switching scenes
        DontDestroyOnLoad(gameObject);
    }

    public void Destroy(IDestroyable destroyable, float delay) {
        destroyable.Destroy();
        destroyable.GetGameObject().SetActive(false);   // set the game object as inactive (destroyed)
        StartCoroutine(DelayRespawn(destroyable, delay));
    }

    public void Respawn(IDestroyable destroyable) {
        destroyable.GetGameObject().SetActive(true);
        destroyable.Respawn();
    }

    private IEnumerator DelayRespawn(IDestroyable destroyable, float delay) {
        yield return new WaitForSeconds(delay);
        Respawn(destroyable);
    }
}
