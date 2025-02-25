using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessable : MonoBehaviour
{
    public Sprite newSprite; // Assign the new sprite in the Inspector
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    public void Start() {
        //Object Names will be the names of objects we can possess
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (newSprite != null) {
            spriteRenderer.sprite = newSprite;
        }
        // else if (gameObject.name == "Square")
        // {
        //     GetComponent<Square>().enabled = true;
        // }
    }

    // Update is called once per frame
    public void Update() {
        exorcize();
    }

    //Not all Possessable objects will have move(). This is for demo purpose

    //All possable objects will have exorcize()
    // public void exorcize()
    // {
    //     if (Input.GetKeyDown(KeyCode.O))
    //     {
    //         this.tag = "Possessable";
    //         GetComponent<Possessable>().enabled = false;
    //         GameObject ghost = GameObject.FindGameObjectWithTag("Player");
    //         ghost.GetComponent<SpriteRenderer>().enabled = true;
    //         if (gameObject.name == "dummy") {
    //             canMove = false;
    //             gameObject.GetComponent<PlayerMovement>().enabled = false;
    //         }
    //         // if (gameObject.name == "Triangle")
    //         // {
    //         //     gameObject.GetComponent<Triangle>().enabled = false;
    //         // }
    //         // if (gameObject.name == "Square")
    //         // {
    //         //     gameObject.GetComponent<Square>().enabled = false;
    //         // }
    //     }
        
    // }
    public void exorcize() {
        if (Input.GetKeyDown(KeyCode.O)) {
            GameObject ghost = GameObject.Find("Ghost"); // Make sure the ghost is named "Ghost" in Unity

            if (ghost != null) {
                ghost.GetComponent<SpriteRenderer>().enabled = true;
                ghost.GetComponent<Possession>().currentBody = ghost; // Return control to the ghost

                // **Update Camera Target to Follow Ghost Again**
                CameraMovement camScript = Camera.main.GetComponent<CameraMovement>();
                if (camScript != null) {
                    camScript.PlayerCharacter = ghost.transform;
                }
            }

            // Reset the possessed object
            this.tag = "Possessable";
            GetComponent<Possessable>().enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
        }
    }


}

