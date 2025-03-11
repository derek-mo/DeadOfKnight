using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    //public Animator plateAnimator;  // Reference to the pressure plate animator
    public GameObject targetObject; // The object to activate/deactivate
    public Sprite pressedSprite; // The sprite to display when the plate is pressed
    private Sprite unpressedSprite; // The sprite to display when the plate is not pressed
    public float requiredMass = 10f; // Minimum mass required to activate the plate
    private float currentMass = 0f; // Tracks the total mass on the plate

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null) // Only count objects with a Rigidbody2D
        {
            Debug.Log("Object entered: " + rb.name);
            Debug.Log("Object mass: " + rb.mass);
            currentMass += rb.mass;
            CheckPlateState();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null) // Only subtract if the object has a Rigidbody2D
        {
            currentMass -= rb.mass;
            CheckPlateState();
        }
    }

    private void CheckPlateState()
    {
        if (currentMass >= requiredMass)
        {
            ActivatePlate();
        }
        else
        {
            DeactivatePlate();
        }
    }

    private void ActivatePlate()
    {
        // if (plateAnimator != null)
        // {
        //     plateAnimator.SetBool("Pressed", true);
        // }
        unpressedSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        gameObject.GetComponent<SpriteRenderer>().sprite = pressedSprite;

        if (targetObject != null)
        {
            targetObject.SetActive(false); // Make the object disappear/open
        }
    }

    private void DeactivatePlate()
    {
        // if (plateAnimator != null)
        // {
        //     plateAnimator.SetBool("Pressed", false);
        // }
        gameObject.GetComponent<SpriteRenderer>().sprite = unpressedSprite;

        if (targetObject != null)
        {
            targetObject.SetActive(true); // Restore the object when weight is removed
        }
    }
}
