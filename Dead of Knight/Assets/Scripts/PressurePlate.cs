using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    [SerializeField]
    GameObject door;
    GameObject leftHalf;
    GameObject rightHalf;
    void OnTriggerEnter2D(Collider2D col)
    {
        leftHalf = door.transform.Find("Left Half").gameObject;
        rightHalf = door.transform.Find("Right Half").gameObject;
        leftHalf.GetComponent<SpriteRenderer>().enabled = false;
        rightHalf.GetComponent<SpriteRenderer>().enabled = false;
        door.GetComponent<BoxCollider2D>().enabled = false;
    }
}