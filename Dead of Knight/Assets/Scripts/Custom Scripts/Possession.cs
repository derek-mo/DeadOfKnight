/*using UnityEngine;

public class Possession : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
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

                    // // Hide the ghost
                    // spriteRenderer.enabled = false;

                    // // Transfer control
                    // currentBody = possessObject;
                    // currentBody.tag = "Player";
                    // if (currentBody.name == "dummy") {
                    //     animator = currentBody.GetComponent<Animator>();
                    //     animator.SetTrigger("Possessing");
                    // }

                    // if (newSprite != null) {
                    //     spriteRenderer = currentBody.GetComponent<SpriteRenderer>();
                    //     spriteRenderer.sprite = newSprite;
                    // }   
                    // currentBody.GetComponent<Possessable>().enabled = true;
                    // currentBody.GetComponent<PlayerMovement>().enabled = true;

                    // // **Update Camera Target**
                    // CameraMovement camScript = Camera.main.GetComponent<CameraMovement>();
                    // if (camScript != null)
                    // {
                    //     camScript.PlayerCharacter = currentBody.transform; // Set the camera to follow the possessed object
                    // }

                }
            }
        }
    }

}
*/

using Unity.VisualScripting;
using UnityEngine;

public class Possession : MonoBehaviour
{
    public Transform ghost;  // Assign in Inspector (for visibility control)

    private Sprite possessableSprite;
    private GameObject possessedObject;
    private bool isPossessing = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isPossessing)
            {
                Depossess();
            }
            else
            {
                Possess();
            }
        }

        updatePosition();
    }

    private void updatePosition()
    {
        transform.position = possessedObject.transform.position;
    }

    private void Possess()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.75f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Possessable"))
            {
                //Possessed Object is the object w/collider the radius insects
                possessedObject = collider.gameObject;
                transform.position = possessedObject.transform.position;
                
                //Hide ghost sprite and disable collider
                gameObject.GetComponent<SpriteRenderer>().sprite = null;
                gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                gameObject.GetComponent<Animator>().enabled = false;

                //Unfreezing x and y positions but still freezing z
                possessedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                possessedObject.GetComponent<Rigidbody2D>().freezeRotation = true;

                //Enable the possessed object's movement and animation
                possessableSprite = possessedObject.GetComponent<SpriteRenderer>().sprite;
                possessedObject.GetComponent<PossessedMovement>().disabled = false;
                possessedObject.GetComponent<Animator>().enabled = true;

                // flip boolean
                isPossessing = true;

                //Switch camera target to possessed object
                CameraMovement cam = Camera.main.GetComponent<CameraMovement>();
                if (cam != null)
                {
                    cam.PlayerCharacter = possessedObject.transform;
                }
                break;
            }
        }
    }

    private void Depossess()
    {
        if (possessedObject)
        {
            Vector2 exitPosition = (Vector2)possessedObject.transform.position + Vector2.left;
            transform.position = exitPosition;

            // Bring back the ghost
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
            gameObject.GetComponent<Animator>().enabled = true;

            //Prevent object movement
            possessedObject.GetComponent<PossessedMovement>().disabled = true;
            possessedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            // Disable the possessed object's animation
            possessedObject.GetComponent<SpriteRenderer>().sprite = possessableSprite;
            possessedObject.GetComponent<Animator>().enabled = false;

            //Reset possessed object
            possessedObject = null;

            // flip boolean
            isPossessing = false;
            CameraMovement cam = Camera.main.GetComponent<CameraMovement>();
            if (cam != null)
            {
                cam.PlayerCharacter = transform; // Set the camera to follow the possessed object
            }
        }
    }

    // just to see the radius for possession
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.75f);  // Change 1f to match your OverlapCircle radius
    }

}