using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour{
    private float length, startposX, startposY;
    public GameObject cam;
    public float parallaxEffect;

    void Start(){
        startposX = transform.position.x;
        startposY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate(){
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float distX = (cam.transform.position.x * parallaxEffect);
        float distY = (cam.transform.position.y * parallaxEffect);

        transform.position = new Vector3(startposX + distX, startposY + distY, transform.position.z);

        if(temp > (startposX + length) - 5)
            startposX += length;
        else if (temp < (startposX - length) + 5)
            startposX -= length;

    }
}
