using UnityEngine;
 
// Automatically destroys the game object after its animation finishes
public class AnimationAutoDestroy : MonoBehaviour {
    public float delay = 0f;
    
    void Start () {
        Destroy (gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay); 
    }
}
