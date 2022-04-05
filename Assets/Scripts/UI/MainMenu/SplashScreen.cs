
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreen : MonoBehaviour {
    public VideoPlayer videoPlayer;
    void Start() {
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp) {
            vp.playbackSpeed /= 10.0F;
            SceneManager.LoadScene("MainMenu");
    }
}
