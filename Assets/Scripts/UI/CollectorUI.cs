using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectorUI : MonoBehaviour
{

    [SerializeField] private Text orbsText;
    private Level levelData;
    private string collected;
    private string total;
    private int colNum;
    private int totNum;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        changeText();
    }

    private void Awake() {
        levelData = FindObjectOfType<Level>();
        if (levelData == null) {
           orbsText.enabled = false;
        }
        else {
            orbsText.enabled = true;
            changeText();
        }
    }

    void changeText() {
        colNum = levelData.GetFoundOrbsCount();
        totNum = levelData.GetTotalOrbCount();
        collected = "Orbs Collected: " + colNum.ToString();
        total = "  ||  Total Orbs: " + totNum.ToString();
        orbsText.text = collected + total;
    }
}
