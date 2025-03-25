using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private float pickupRange = 1.5f;
    [SerializeField] private Transform holdPoint; // Where to position the sword when held
    [SerializeField] private KeyCode pickupKey = KeyCode.E;
    [SerializeField] private KeyCode dropKey = KeyCode.Q;
    [SerializeField] private LayerMask swordLayer;
    
    [Header("UI")]
    [SerializeField] private GameObject pickupPrompt; // Optional UI element to show pickup prompt
    
    private GameObject heldSword;
    private SwordController swordController;
    
    private void Start()
    {
        if (pickupPrompt != null)
        {
            pickupPrompt.SetActive(false);
        }
        
        // Create hold point if not assigned
        if (holdPoint == null)
        {
            holdPoint = new GameObject("SwordHoldPoint").transform;
            holdPoint.parent = transform;
            holdPoint.localPosition = new Vector3(0.5f, 0f, 0f); // Position to the right of player
        }
    }
    
    private void Update()
    {
        if (heldSword == null)
        {
            // Try to find nearby sword
            CheckForNearbyPickup();
        }
        else
        {
            // Handle dropping the sword
            if (Input.GetKeyDown(dropKey))
            {
                DropSword();
            }
            
            // Keep the sword at the hold point
            heldSword.transform.position = holdPoint.position;
        }
    }
    
    private void CheckForNearbyPickup()
    {
        // Cast a circle to find nearby swords
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, pickupRange, swordLayer);
        
        bool swordNearby = false;
        GameObject nearestSword = null;
        float closestDistance = float.MaxValue;
        
        // Find the closest sword
        foreach (Collider2D obj in nearbyObjects)
        {
            if (obj.GetComponent<SwordController>() != null)
            {
                float distance = Vector2.Distance(transform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestSword = obj.gameObject;
                    swordNearby = true;
                }
            }
        }
        
        // Show pickup prompt if a sword is nearby
        if (pickupPrompt != null)
        {
            pickupPrompt.SetActive(swordNearby);
        }
        
        // Pick up the sword when E is pressed
        if (swordNearby && Input.GetKeyDown(pickupKey))
        {
            PickupSword(nearestSword);
        }
    }
    
    private void PickupSword(GameObject sword)
    {
        heldSword = sword;
        
        // Completely disable physics on the sword
        Rigidbody2D rb = heldSword.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero; // Stop any existing velocity
            rb.angularVelocity = 0f;    // Stop any rotation
        }
        
        // Disable collisions with the sword while held
        Collider2D[] swordColliders = heldSword.GetComponents<Collider2D>();
        foreach (Collider2D col in swordColliders)
        {
            col.isTrigger = true; // Make it a trigger to prevent physics collisions
        }
        
        // Parent the sword to the player and position it precisely
        heldSword.transform.parent = transform;
        heldSword.transform.position = holdPoint.position;
        
        // You may also want to set a specific rotation
        // heldSword.transform.rotation = holdPoint.rotation;
        
        // Notify the sword controller that it's been picked up
        swordController = heldSword.GetComponent<SwordController>();
        if (swordController != null)
        {
            swordController.OnPickup(transform);
        }
        
        // Hide the pickup prompt
        if (pickupPrompt != null)
        {
            pickupPrompt.SetActive(false);
        }
    }
    
    private void DropSword()
    {
        if (heldSword != null)
        {
            // Re-enable physics
            Rigidbody2D rb = heldSword.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            
            // Re-enable collisions
            Collider2D[] swordColliders = heldSword.GetComponents<Collider2D>();
            foreach (Collider2D col in swordColliders)
            {
                col.isTrigger = false; // Make it solid again
            }
            
            // Add a small force when dropping to separate from player
            if (rb != null)
            {
                Vector2 dropDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                rb.AddForce(dropDirection * 2f, ForceMode2D.Impulse);
            }
            
            // Unparent the sword
            heldSword.transform.parent = null;
            
            // Notify the sword controller that it's been dropped
            if (swordController != null)
            {
                swordController.OnDrop();
            }
            
            heldSword = null;
            swordController = null;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw the pickup range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
        
        // Draw the hold point
        if (holdPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(holdPoint.position, 0.1f);
        }
    }
} 