using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class FadeManager : MonoBehaviour {

    public static FadeManager instance;
    public Animator animator;
    public Image image;

    private void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }
    
    /*
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneChange;
    }
    private void OnSceneChange(Scene scene, LoadSceneMode arg1) {    // remove fade when loading level
        if (scene.name.Contains("Level")) {
            FadeIn();
        } else {
            animator.SetTrigger("NoFade");
        }
    }
    */

    public void FadeOut() {
        animator.SetTrigger("FadeOut");
    }
    
    public void FadeIn() {
        animator.SetTrigger("FadeIn");
    }

}
