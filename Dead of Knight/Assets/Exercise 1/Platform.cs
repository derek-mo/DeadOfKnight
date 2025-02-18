using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed;
    public float distance;
    public float timer;
    public Vector3 direction = new Vector3(1,0,0); //this will move the platform along the x-direction by default; can be changed in the editor
    Vector3 start;

    // Start is called before the first frame update
    void Start()
    {
        start = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = start + (distance * Mathf.Cos(timer*speed))* direction;
        timer += Time.deltaTime ;
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnCollisionExit(Collision collision)
    {

    }
}
