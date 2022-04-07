using UnityEngine;

public class EventSystemDestroyer : MonoBehaviour {
    
    public static EventSystemDestroyer instance;
    
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

}
