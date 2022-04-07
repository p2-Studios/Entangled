using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {
    
    public GameObject player;
    void Start() {
        player.SetActive(false);
        StartCoroutine(EnablePlayer());
    }

    IEnumerator EnablePlayer()
     {         
         yield return new WaitForSeconds(8.5f);
  
         player.SetActive(true);
     }

    public void PlayPipeSound() {
        AudioManager.instance.Play("glass_break");
    }

    public void PlayCrashSound() {
    }
    
}
