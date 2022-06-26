using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorTransition : MonoBehaviour {

    public static string levelToLoad = "MainMenu";
    
    public Transform endPos;
    public float speed = 1.0f;

    private bool atEnd = false;

    protected SpriteRenderer neonSign;
    [SerializeField] Material Level1, Level2, Level3, Level4, Level5, Level6, Level7, Level8, Level9;
    [SerializeField] Sprite sprLevel1, sprLevel2, sprLevel3, sprLevel4, sprLevel5, sprLevel6, sprLevel7, sprLevel8, sprLevel9;
    private GameObject sign;
    void Start(){
        PauseMenu.instance.ToggleControlIndicator(false);
        // change transition sign based on upcoming level
        sign = GameObject.Find("Level_Sign");
        neonSign = sign.GetComponent<SpriteRenderer>();
        neonSign.enabled = true;
        switch (levelToLoad){
            case "Level1":
                neonSign.sprite = sprLevel1;
                neonSign.material = Level1;
                break;
            case "Level2":
                neonSign.sprite = sprLevel2;
                neonSign.material = Level2;
                break;
            case "Level3":
                neonSign.sprite = sprLevel3;
                neonSign.material = Level3;
                break;
            case "Level4":
                neonSign.sprite = sprLevel4;
                neonSign.material = Level4;
                break;
            case "Level5":
                neonSign.sprite = sprLevel5;
                neonSign.material = Level5;
                break;
            case "Level6":
                neonSign.sprite = sprLevel6;
                neonSign.material = Level6;
                break;
            case "Level7":
                neonSign.sprite = sprLevel7;
                neonSign.material = Level7;
                break;
            case "Level8":
                neonSign.sprite = sprLevel8;
                neonSign.material = Level8;
                break;
            case "Level9":
                neonSign.sprite = sprLevel9;
                neonSign.material = Level9;
                break;
            default:
                neonSign.enabled = false;
                break;
        }
    }

    void Update() {
        transform.position = Vector2.MoveTowards(transform.position, endPos.position, speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)) {
            LoadNextScene();
        }
        
        if (!atEnd && transform.position == endPos.position) {
            LoadNextScene();
        }
    }

    public void LoadNextScene() {
        atEnd = true;
        Scene scene = SceneManager.GetSceneByName(levelToLoad);
        SceneManager.LoadSceneAsync(levelToLoad);
        if (!scene.IsValid()) { // check if scene was found
            SceneManager.LoadSceneAsync(levelToLoad);
        } else {
            Debug.LogWarning("Scene " + levelToLoad + " attempted to load, but wasn't found!");
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
    
}
