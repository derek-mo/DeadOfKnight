using UnityEngine;

public class PushableBox : MonoBehaviour
{
    private Rigidbody2D rb;
    private RigidbodyConstraints2D originalConstraints;
    private bool isBeingPushed = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Store original constraints (likely just rotation)
            originalConstraints = rb.constraints;
            
            // Start frozen
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    
    void Update()
    {
        // Check for shift key press
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
        
        // Update constraints based on shift key
        if (rb != null)
        {
            if (shiftPressed && isBeingPushed)
            {
                // Allow movement
                rb.constraints = originalConstraints;
            }
            else
            {
                // Freeze position
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                rb.velocity = Vector2.zero;
            }
        }
    }
    
    // Called by PlayerPush when the player is pushing this box
    public void SetPushing(bool isPushing)
    {
        isBeingPushed = isPushing;
    }
} 