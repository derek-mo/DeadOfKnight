using UnityEngine;
using System.Collections;

public class PlayerPush : MonoBehaviour
{
    public float pushForce = 2f;
    public LayerMask objMask;     // This should be set to the Pushable layer only
    public float interactRadius = 1f;
    public float stopThreshold = 0.1f;
    public bool isGhost = false;  // Set this for ghost objects
    public bool isArmor = false;  // Set this for armor objects
    public float counterForce = 2f; // Force to apply against movement when not pushing

    private GameObject currentPushable;
    private Rigidbody2D playerRb;
    private Vector2 moveDirection;
    private bool isPushing = false;
    private float collisionForceBuffer = 0.5f; // Increased to overcome friction
    
    // Dictionary to store original constraints
    private System.Collections.Generic.Dictionary<Rigidbody2D, RigidbodyConstraints2D> originalConstraints = 
        new System.Collections.Generic.Dictionary<Rigidbody2D, RigidbodyConstraints2D>();

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        
        // Cache all pushable objects and their original constraints
        GameObject[] pushables = GameObject.FindGameObjectsWithTag("Pushable");
        foreach (GameObject pushable in pushables)
        {
            Rigidbody2D rb = pushable.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                originalConstraints[rb] = rb.constraints;
            }
        }
        
        // Handle the collision between ghost/armor/player and pushable boxes
        UpdateCollisionSettings();
        
        // Add a persistent check for collisions
        StartCoroutine(ContinuouslyCheckPushables());
    }
    
    // Add a continuous check for nearby pushables to stop them
    System.Collections.IEnumerator ContinuouslyCheckPushables()
    {
        while (true)
        {
            UpdatePushableConstraints();
            yield return new WaitForFixedUpdate();
        }
    }
    
    // Method to update constraints based on shift key
    void UpdatePushableConstraints()
    {
        // Find all pushable objects
        GameObject[] pushables = GameObject.FindGameObjectsWithTag("Pushable");
        
        foreach (GameObject pushable in pushables)
        {
            Rigidbody2D rb = pushable.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (Input.GetKey(KeyCode.LeftShift) && (isPushing || IsNearby(pushable)))
                {
                    // If shift is pressed and we're pushing or near the object, restore original constraints
                    if (originalConstraints.ContainsKey(rb))
                    {
                        rb.constraints = originalConstraints[rb];
                    }
                    else
                    {
                        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        originalConstraints[rb] = rb.constraints;
                    }
                }
                else
                {
                    // If shift is not pressed, freeze position
                    if (!originalConstraints.ContainsKey(rb))
                    {
                        originalConstraints[rb] = rb.constraints;
                    }
                    rb.velocity = Vector2.zero;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
        }
    }
    
    // Helper method to check if an object is nearby
    bool IsNearby(GameObject obj)
    {
        if (obj == null) return false;
        
        float distance = Vector2.Distance(transform.position, obj.transform.position);
        return distance < interactRadius * 1.5f;
    }
    
    // Method to be called from Start and whenever ghost/armor state changes
    public void UpdateCollisionSettings()
    {
        // Find all pushable objects
        GameObject[] pushables = GameObject.FindGameObjectsWithTag("Pushable");
        
        foreach (GameObject pushable in pushables)
        {
            Collider2D pushableCollider = pushable.GetComponent<Collider2D>();
            Collider2D playerCollider = GetComponent<Collider2D>();
            
            if (pushableCollider != null && playerCollider != null)
            {
                // If we're a ghost (not in armor), we should pass through boxes
                if (isGhost && !isArmor)
                {
                    Physics2D.IgnoreCollision(playerCollider, pushableCollider, true);
                }
                // If we're armor or regular player, we should collide with boxes
                else
                {
                    Physics2D.IgnoreCollision(playerCollider, pushableCollider, false);
                }
                
                // Update rigidbody settings for better physics interaction
                Rigidbody2D rb = pushable.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Keep existing material but ensure high drag
                    rb.drag = 5f; // High drag to prevent sliding
                    
                    // Store original constraints if not already stored
                    if (!originalConstraints.ContainsKey(rb))
                    {
                        originalConstraints[rb] = rb.constraints;
                    }
                    
                    // Freeze position when not pushing
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        rb.velocity = Vector2.zero;
                        rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                }
            }
        }
    }

    void Update()
    {
        // Ghost (not in armor) cannot push at all
        if (isGhost && !isArmor)
            return;
            
        // Get input for movement direction
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // Only check for pushable objects when shift is held
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (currentPushable != null && isPushing)
            {
                StopPushing();
            }
            return;
        }

        // Check for pushable objects
        Vector2 position = new Vector2(transform.position.x, transform.position.y - 0.25f);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, interactRadius, objMask);

        bool foundPushable = false;

        // Check for objects with Pushable tag
        foreach (var collider in colliders)
        {
            if (collider != null && collider.gameObject.CompareTag("Pushable") && collider.gameObject != gameObject)
            {
                currentPushable = collider.gameObject;
                foundPushable = true;
                
                // Tell the box it's being pushed
                PushableBox pushableBox = currentPushable.GetComponent<PushableBox>();
                if (pushableBox != null)
                {
                    pushableBox.SetPushing(true);
                }
                
                isPushing = true;
                PushObject();
                break;
            }
        }

        // If no pushable found, stop pushing
        if (!foundPushable)
        {
            if (currentPushable != null && isPushing)
            {
                StopPushing();
            }
        }
    }

    void StopPushing()
    {
        if (currentPushable != null)
        {
            // Tell the box it's no longer being pushed
            PushableBox pushableBox = currentPushable.GetComponent<PushableBox>();
            if (pushableBox != null)
            {
                pushableBox.SetPushing(false);
            }
            
            Rigidbody2D pushableRb = currentPushable.GetComponent<Rigidbody2D>();
            if (pushableRb != null)
            {
                pushableRb.velocity = Vector2.zero;
            }
        }
        currentPushable = null;
        isPushing = false;
    }

    void PushObject()
    {
        if (currentPushable != null && moveDirection != Vector2.zero)
        {
            Rigidbody2D pushableRb = currentPushable.GetComponent<Rigidbody2D>();
            if (pushableRb != null)
            {
                // Calculate the position difference between player and pushable
                Vector2 playerToPushable = (Vector2)currentPushable.transform.position - (Vector2)transform.position;
                float distance = playerToPushable.magnitude;

                // Check if we're close enough to the object
                if (distance < interactRadius * 1.5f)
                {
                    // Calculate dot product to ensure we're pushing in the general direction of the box
                    float dotProduct = Vector2.Dot(playerToPushable.normalized, moveDirection);

                    // Only push if we're facing toward the box (positive dot product)
                    if (dotProduct > 0.3f)
                    {
                        // Apply pushing force in the direction of movement
                        pushableRb.velocity = moveDirection * pushForce;
                    }
                    else
                    {
                        // Not facing the box enough, stop pushing
                        pushableRb.velocity = Vector2.zero;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 position = new Vector2(transform.position.x, transform.position.y - 0.25f);
        Gizmos.DrawWireSphere(position, interactRadius);
    }
    
    void FixedUpdate()
    {
        // This is now handled by the UpdatePushableConstraints method
    }
    
    // Prevent pushing without shift key
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PreventAutomaticPushing(collision);
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        PreventAutomaticPushing(collision);
    }
    
    private void PreventAutomaticPushing(Collision2D collision)
    {
        // If this is a pushable object and we're not pressing shift, completely stop it
        if (collision.gameObject.CompareTag("Pushable"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    // Store original constraints if not already stored
                    if (!originalConstraints.ContainsKey(rb))
                    {
                        originalConstraints[rb] = rb.constraints;
                    }
                    
                    // Freeze all movement
                    rb.velocity = Vector2.zero;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                else
                {
                    // When shift is pressed, restore original constraints
                    if (originalConstraints.ContainsKey(rb))
                    {
                        rb.constraints = originalConstraints[rb];
                    }
                }
            }
        }
    }
}

