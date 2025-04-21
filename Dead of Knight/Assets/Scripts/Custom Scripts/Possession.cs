using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Possession : MonoBehaviour
{
    public Transform ghost;  // Assign in Inspector (for visibility control)
    public GameObject startingPossession;  // Assign this in the Inspector to your armor object
    public GameObject popup;

    private Sprite possessableSprite;
    private GameObject possessedObject;
    public static bool isPossessing = false;

    [Header("Audio")]
	public PlayerAudio playerAudio;

    private Sprite armorUp;
    private Sprite armorDown;
    private Sprite armorLeft;
    private Sprite armorRight;

    private void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        if (startingPossession != null)
        {
            Possess(startingPossession); // Start game possessing the armor
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (playerAudio) {
                playerAudio.AttackSource.Play();
            }
            if (isPossessing)
            {
                Depossess();
            }
            else
            {
                Possess();
            }
        }
        show_popup();
        //updatePosition();
    }

    // private void updatePosition()
    // {
    //     // Update the ghost's position to match the possessed object
    //     if (possessedObject)
    //         ghost.position = possessedObject.transform.position;
    // }

    private void Possess(GameObject target = null)
    {
        if (target == null) // Normal possession logic
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Possessable") || collider.CompareTag("Armor"))
                {
                    target = collider.gameObject;
                    break;
                }
            }
        }

        if (target != null)
        {
            possessedObject = target;
            transform.position = possessedObject.transform.position;

            // Hide ghost sprite and disable collider
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<Animator>().enabled = false;


            if (possessedObject.name == "Armor")
            {
                // Unfreeze movement
                possessedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                possessedObject.GetComponent<Rigidbody2D>().freezeRotation = true;

                // Enable movement & animations
                possessableSprite = possessedObject.GetComponent<SpriteRenderer>().sprite;
                possessedObject.GetComponent<PossessedMovement>().enabled = true;
                possessedObject.GetComponent<Animator>().enabled = true;
            }

            isPossessing = true;

            // Switch camera to possessed object
            CameraMovement cam = Camera.main.GetComponent<CameraMovement>();
            if (cam != null)
            {
                cam.PlayerCharacter = possessedObject.transform;
            }

            // Turn on Ballista
            if(possessedObject.name == "Ballista")
            {
                possessedObject.GetComponent<Ballista>().enabled = true;
            }
        }
    }

    private void Depossess()
    {
        if (possessedObject)
        {
            Vector2 exitPosition = (Vector2)possessedObject.transform.position;
            transform.position = exitPosition;

            // Bring back the ghost
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
            gameObject.GetComponent<Animator>().enabled = true;


            if (possessedObject.name == "Armor")
            {
                //Prevent object movement
                possessedObject.GetComponent<PossessedMovement>().enabled = false;
                possessedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                // Disable the possessed object's animation
                possessedObject.GetComponent<SpriteRenderer>().sprite = possessableSprite;
                possessedObject.GetComponent<Animator>().enabled = false;

                // var sr = possessedObject.GetComponent<SpriteRenderer>();
                // var pm = possessedObject.GetComponent<PossessedMovement>();
                // Vector2 dir = pm.LastMovement;
                // if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                // {
                //     sr.sprite = dir.x > 0 ? armorRight : armorLeft;
                // }
                // else
                // {
                //     sr.sprite = dir.y > 0 ? armorUp : armorDown;
                // }

            }

            // Turn on Ballista
            if (possessedObject.name == "Ballista")
            {
                possessedObject.GetComponent<Ballista>().enabled = false;
            }

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
        Gizmos.DrawWireSphere(transform.position, 1f);  // Change 1f to match your OverlapCircle radius
    }

    //Shows the possess prompt when player is close enough to possessable object
    public void show_popup()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (var collider in colliders)
        {
           if ((collider.CompareTag("Possessable") || collider.CompareTag("Armor")) && (!isPossessing))
             {
               popup.GetComponent<SpriteRenderer>().enabled = true;
                return;
             }
           }
        popup.GetComponent<SpriteRenderer>().enabled = false;
    }

}