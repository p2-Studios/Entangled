using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroVideo : MonoBehaviour {

    public VideoPlayer intro;
    public Canvas videoCanvas;

    void Update() {
        if (!(intro.isPlaying)) {
            videoCanvas.gameObject.SetActive(false);
    }
    }
}
