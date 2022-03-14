using UnityEngine;

public class Orb : MonoBehaviour {

    public UniqueId UID;
    private Level level;
    
    private void Awake() {
        level = FindObjectOfType<Level>();
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            Collect();
        }
    }

    public void Collect() {
        FindObjectOfType<AudioManager>().Play("orb_collect");
        level.OrbCollected(this); 
    }
    public string GetID() {
        return UID.uniqueId;
    }
}
