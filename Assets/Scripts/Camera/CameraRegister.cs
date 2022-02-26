using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// tutorial used: https://www.youtube.com/watch?v=nkL-VvMmWHg


// Attached to virtual cams, registers/unregeisters cams on scene load/unload
public class CameraRegister : MonoBehaviour
{
    private void OnEnable(){
        CameraSwitcher.Register(GetComponent<CinemachineVirtualCamera>());
    }

    private void OnDisable(){
        CameraSwitcher.Unregister(GetComponent<CinemachineVirtualCamera>());
    }
}
