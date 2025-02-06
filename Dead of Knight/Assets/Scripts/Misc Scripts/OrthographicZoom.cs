using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicZoom : MonoBehaviour
{
    public Camera cam;
    public float maxZoom = 3;
    public float minZoom = 6;
    public float sensitivity = 1;
    public float speed = 30;
    float targetZoom;

    void Start()
    {
        targetZoom = cam.orthographicSize;
    }
    void Update()
    {
        targetZoom -= Input.mouseScrollDelta.y * sensitivity;
        targetZoom = Mathf.Clamp(targetZoom, maxZoom, minZoom);
        float newSize = Mathf.MoveTowards(cam.orthographicSize, targetZoom, speed * Time.deltaTime);
        cam.orthographicSize = newSize;
    }
}