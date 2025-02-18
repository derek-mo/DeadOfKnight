using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //ContactPoint contact = collision.contacts[0];
        //Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        //Vector3 position = contact.point;
        //Instantiate(explosionPrefab, position, rotation);
        GameObject obj = collision.gameObject;
        if (obj.tag == "Player")
            obj.transform.position = new Vector3(0, 2, 0);
    }
}
