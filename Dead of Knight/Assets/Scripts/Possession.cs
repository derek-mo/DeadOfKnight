using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Possession : MonoBehaviour
{
    //All objects that can be possessed must have label Possessable
    GameObject[] possessables; 
    SpriteRenderer spriteRenderer;
    public GameObject popup;

    // Start is called before the first frame update
    void Start()
    {
        possessables = GameObject.FindGameObjectsWithTag("Possessable");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        possess();
        show_popup();
    }

    void possess()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (GameObject possessObject in possessables)
            {
                if ((transform.position - possessObject.transform.position).magnitude <= 2)
                {
                    spriteRenderer.enabled = false;
                    Debug.Log("Possessing");
                    possessObject.tag = "Player";
                    possessObject.GetComponent<Possessable>().enabled = true;
                    return;
                }
            }
        }
    }

    //Shows the possess prompt when player is close enough to possessable object
    public void show_popup()
    {
        foreach (GameObject possessObject in possessables)
        {
            if (((transform.position - possessObject.transform.position).magnitude <= 2) && (possessObject.tag == "Possessable"))
            {
                popup.GetComponent<SpriteRenderer>().enabled = true;
                return;
            }
        }
        popup.GetComponent<SpriteRenderer>().enabled = false;
    }
}
