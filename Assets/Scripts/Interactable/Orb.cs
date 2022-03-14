using UnityEngine;

[System.Serializable]
public class Orb : MonoBehaviour {

    public UniqueId UID;
    
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            CollectOrb();
        }
    }
    private void CollectOrb() {
        gameObject.SetActive(false);
        //levelDataManager.collectOrb(this);
    }
    public string GetID() {
        return UID.uniqueId;
    }
}
