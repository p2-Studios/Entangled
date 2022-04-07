using UnityEngine;

public class RespawnSetter : MonoBehaviour {
    private Player player;
    public Transform respawnPoint;

    private void Awake() {
        player = FindObjectOfType<Player>();
    }
    
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            player.respawnLocation = respawnPoint;
            LevelRestarter.instance.SetCheckpointPosition(respawnPoint);
        }
    }
}
