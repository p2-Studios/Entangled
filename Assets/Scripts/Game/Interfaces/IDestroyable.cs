// Interface for objects that can be destroyed and respawned.

using UnityEngine;

public interface IDestroyable {
    GameObject GetGameObject();
    void Destroy();
    void Respawn();
}
