using System.Collections;
using Game.CustomKeybinds;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LevelResetVisual: MonoBehaviour
{
    private Vignette vignette;
    
    private bool prevCoroutineRunning = false;

    void Start()
    {
        Volume v = GetComponent<Volume>();
        v.profile.TryGet(out vignette);
    }
    
    void Update() {

        if (Input.GetKey(Keybinds.GetInstance().reset))
        {
            if (!prevCoroutineRunning)
            {
                StartCoroutine(IncreaseVignette(0.012f));
            }
                
        }
        if (Input.GetKeyUp(Keybinds.GetInstance().reset))
        {
            StartCoroutine(DecreaseVignette(0.012f));
        }
    }

    IEnumerator IncreaseVignette(float seconds)
    {
        prevCoroutineRunning = true;
        yield return new WaitForSeconds(seconds);
        vignette.intensity.value += 0.005f;
        prevCoroutineRunning = false;
    }

    IEnumerator DecreaseVignette(float seconds)
    {
        while (vignette.intensity.value > 0.0f && !Input.GetKey(Keybinds.GetInstance().reset))
        {
            prevCoroutineRunning = true;
            yield return new WaitForSeconds(seconds);
            vignette.intensity.value -= 0.01f;
            prevCoroutineRunning = false;
        }
    }
}