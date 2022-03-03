using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestarter : MonoBehaviour
{
    // Start is called before the first frame update

    private bool restarting = false;
    private bool holdingR = false;
    void Start()
    {
        print("This scene has been loaded.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            if (!restarting) {
                holdingR = true;
                restarting = true;
                StartCoroutine(Action());
            }
        } if (Input.GetKeyUp(KeyCode.R)) {
            holdingR = false;
        }
    }

    IEnumerator Action() {
        yield return new WaitForSeconds(2);
        if (holdingR) {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        restarting = false;
    }
}
