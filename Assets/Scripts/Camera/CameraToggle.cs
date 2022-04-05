using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraToggle : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera switchCam;

    public static CinemachineVirtualCamera activeCamera = null;

    public bool toggleCam = false;

    // Update is called once per frame
    void Update()
    {
        activeCamera = CameraSwitcher.ActiveCamera;
        if (activeCamera != null){
            if(toggleCam){
                CameraSwitcher.SwitchCamera(switchCam);
            }
            else{
                CameraSwitcher.SwitchCamera(activeCamera);
            }
        }
    }
}
