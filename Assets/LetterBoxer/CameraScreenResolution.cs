using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScreenResolution : MonoBehaviour
{

    float defaultWidth;
    public CinemachineVirtualCamera ActiveCamera;

    void Start() {
        Debug.Log(Camera.main.aspect);
        ActiveCamera = this.GetComponent<CinemachineVirtualCamera>();
        defaultWidth = ActiveCamera.m_Lens.OrthographicSize*(Camera.main.aspect);     //get width of camera       //get attached vcam
        Debug.Log(ActiveCamera.name+ " : "+ defaultWidth);
    }

    void Update() {
        //sets active cams orthographic size to the aspect ratio
        Debug.Log(ActiveCamera.name+ " : "+ defaultWidth);
        ActiveCamera.m_Lens.OrthographicSize = defaultWidth/Camera.main.aspect;
        

    }
}
