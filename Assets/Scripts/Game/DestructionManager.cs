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
        }
    }

    public void Destroy(IDestroyable destroyable, float delay) {
        if (!IsDestroyed(destroyable.GetGameObject())) {
            destroyable.Destroy();
            destroyable.GetGameObject().SetActive(false); // set the game object as inactive (destroyed)
            StartCoroutine(DelayRespawn(destroyable, delay));
        }
    }

    public void Respawn(IDestroyable destroyable) {
        if (!IsDestroyed(destroyable.GetGameObject())) {
            destroyable.GetGameObject().SetActive(true);
            destroyable.Respawn();
        }
    }
    

    private IEnumerator DelayRespawn(IDestroyable destroyable, float delay) {
        yield return new WaitForSeconds(delay);
        if (!IsDestroyed(destroyable.GetGameObject())) {
            Respawn(destroyable);
        }
    }
    
    public static bool IsDestroyed(GameObject gameObject) {
        return gameObject == null && !ReferenceEquals(gameObject, null);
    }

    public void SetRespawnAnimation(float delay, Transform respawnAnimation, Vector3 respawnLocation) {
        StartCoroutine(RespawnAnimation(delay, respawnAnimation, respawnLocation));
    }
    
    public IEnumerator RespawnAnimation(float delay, Transform respawnAnimation, Vector3 respawnLocation) {
        yield return new WaitForSeconds(delay);
        Instantiate(respawnAnimation, respawnLocation, Quaternion.identity);
    }
}
