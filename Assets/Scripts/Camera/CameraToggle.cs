using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraToggle : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera switchCam;

    public static CinemachineVirtualCamera activeCamera = null;

    public Transform playerTopLoc;
    public Transform playerBottomLoc;


    private Player player;

    private void Start() {
        player = FindObjectOfType<Player>();
    }

    public void TransitionToTop() {
        activeCamera = CameraSwitcher.ActiveCamera;
        player.transform.position = playerTopLoc.position;
        CameraSwitcher.SwitchCamera(switchCam);
    }
    
    public void TransitionToBottom(float delay) {
        StartCoroutine(WaitAndToggle(delay));
    }

    public IEnumerator WaitAndToggle(float delay) {
        yield return new WaitForSeconds(delay);
        player.transform.position = playerBottomLoc.position;
        CameraSwitcher.SwitchCamera(activeCamera);
        FindObjectOfType<FinaleScreenManager>().SatelliteEntangled();
    }
}
