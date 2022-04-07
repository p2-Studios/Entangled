using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {
    
    public GameObject player;
    void Start() {
        StartCoroutine(EnablePlayer());
    }

    IEnumerator EnablePlayer()
     {         
         yield return new WaitForSeconds(8.5f);
  
         player.SetActive(true);
     }

}
