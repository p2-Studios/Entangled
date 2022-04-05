using UnityEngine;

public class BouncyFloor : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D col) {
        print("bounce");
        if (col.gameObject.CompareTag("Player")) {
            AudioManager.instance.Play("Bounce");
        }
    }
}
