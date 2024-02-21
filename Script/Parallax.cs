using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject camera;
    float startPosX;
    float startPosY;
    public float parallax;
    public float parY;

    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float distA = camera.transform.position.x * parallax;
        float distB = camera.transform.position.y * parY;
        transform.position = new Vector3(startPosX + distA, startPosY + distB, transform.position.z);
    }
}
