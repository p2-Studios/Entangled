using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// tutorial used: https://www.youtube.com/watch?v=nkL-VvMmWHg

// deals with registering and priorities of cameras when switched
public static class CameraSwitcher {

    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();   // all cams are stored in a list
    public static CinemachineVirtualCamera ActiveCamera = null;

    // checks if cam is active
    public static bool IsActiveCamera(CinemachineVirtualCamera camera){
        return camera == ActiveCamera;
    }

    // Increases active cams priority and decreases all other cams priority
    public static void SwitchCamera(CinemachineVirtualCamera camera){
        camera.Priority = 10;
        ActiveCamera = camera;

        foreach (CinemachineVirtualCamera c in cameras){
            if (c != camera && c.Priority != 0){
                c.Priority = 0;
            }
        }
    }

    // registers cam in cameras list
    public static void Register(CinemachineVirtualCamera camera){
        cameras.Add(camera);
        Debug.Log("Camera Registered: "+ camera);
    }

    // removes cam from cameras list
    public static void Unregister(CinemachineVirtualCamera camera){
        cameras.Remove(camera);
        Debug.Log("Camera Unregistered: "+ camera);
    }
}
