using UnityEngine;

public class Conduit : MonoBehaviour
{
    public Animator conduitAnimator;  // Reference to the conduit animator
    public GameObject targetObject;  // The object this conduit will trigger
    //public float interactionDistance = 3f; // Distance to detect the player
    public Transform player;  // Reference to the player
    private bool isPlayerNear = false; // Flag to check if the player is near
    private bool isObjectActive = true; // Track whether the object is active or not

    void Update()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y + 0.1f);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.7f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                isPlayerNear = true;
                break;
            }
            else
            {
                isPlayerNear = false;
            }
        }
        
        // Check if the player is within interaction distance and presses "Space"
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Space))
        {
            ActivateConduit();
        }
    }

    // Method to activate the conduit and trigger the target object's action
    private void ActivateConduit()
    {
        if (conduitAnimator != null)
        {
            if (isObjectActive)
            {
                conduitAnimator.SetTrigger("Activate"); // Trigger an animation (ensure "Activate" is a trigger in your Animator)
            }
            else
            {
                conduitAnimator.SetTrigger("Deactivate"); // Trigger an animation (ensure "Deactivate" is a trigger in your Animator)
            }
        }

        if (targetObject != null)
        {
            ToggleTargetObject();
        }
    }

    // Toggles the visibility of the target object (or any other action you'd like)
    private void ToggleTargetObject()
    {
        // Toggle the active state of the target object
        isObjectActive = !isObjectActive;
        targetObject.SetActive(isObjectActive);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Gizmos.DrawWireSphere(position, 0.7f);  // Change 1f to match your OverlapCircle radius
    }
}
