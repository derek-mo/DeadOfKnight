// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;

// public class Possession : MonoBehaviour
// {
//     //All objects that can be possessed must have label Possessable
//     GameObject[] possessables; 
//     SpriteRenderer spriteRenderer;
//     // Start is called before the first frame update
//     void Start()
//     {
//         possessables = GameObject.FindGameObjectsWithTag("Possessable");
//         spriteRenderer = GetComponent<SpriteRenderer>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         possess();
//     }

//     void possess()
//     {
//         if (Input.GetKeyDown(KeyCode.P))
//         {
//             foreach (GameObject possessObject in possessables)
//             {
//                 Debug.Log(possessObject.name);
//                 if ((transform.position - possessObject.transform.position).magnitude <= 2)
//                 {
//                     spriteRenderer.enabled = false;
//                     Debug.Log("Possessing");
//                     possessObject.tag = "Player";
//                     possessObject.GetComponent<Possessable>().enabled = true;
//                     possessObject.GetComponent<Possessable>().canMove = true;
//                     return;
//                 }
//             }
//         }
//     }
// }

using UnityEngine;

public class Possession : MonoBehaviour
{
    public GameObject currentBody; // The possessed object
    public Camera mainCamera; // Assign the Main Camera in the Inspector
    public Sprite newSprite; // Assign the new sprite in the Inspector
    Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentBody = gameObject; // Initially, the ghost is the player
    }

    void Update()
    {
        if (currentBody != gameObject) // If possessing, move the ghost to follow
        {
            transform.position = currentBody.transform.position;
        }

        possess();
    }

    void possess()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject[] possessables = GameObject.FindGameObjectsWithTag("Possessable");

            foreach (GameObject possessObject in possessables)
            {
                if ((transform.position - possessObject.transform.position).magnitude <= 2)
                {
                    Debug.Log("Possessing: " + possessObject.name);

                    // Hide the ghost
                    spriteRenderer.enabled = false;

                    // Transfer control
                    currentBody = possessObject;
                    currentBody.tag = "Player";
                    if (currentBody.name == "dummy") {
                        animator = currentBody.GetComponent<Animator>();
                        animator.SetTrigger("Possessing");
                    }

                    if (newSprite != null) {
                        spriteRenderer = currentBody.GetComponent<SpriteRenderer>();
                        spriteRenderer.sprite = newSprite;
                    }   
                    currentBody.GetComponent<Possessable>().enabled = true;
                    currentBody.GetComponent<PlayerMovement>().enabled = true;

                    // **Update Camera Target**
                    CameraMovement camScript = Camera.main.GetComponent<CameraMovement>();
                    if (camScript != null)
                    {
                        camScript.PlayerCharacter = currentBody.transform; // Set the camera to follow the possessed object
                    }

                    return;
                }
            }
        }
    }

}
