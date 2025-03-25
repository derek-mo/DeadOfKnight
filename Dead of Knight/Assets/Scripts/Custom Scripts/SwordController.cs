using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [Header("Sword Settings")]
    [SerializeField] private float swingCooldown = 0.5f;
    [SerializeField] private int swingDamage = 10;
    [SerializeField] private int joustDamage = 20; // More damage for jousting
    [SerializeField] private float hitRadius = 1.0f;
    [SerializeField] private float joustRange = 1.5f; // Longer range for jousting
    [SerializeField] private LayerMask enemyLayer;
    
    [Header("Animation")]
    [SerializeField] private Animator swordAnimator; // Reference to the sword's animator
    [SerializeField] private string swingTriggerName = "Swing"; // The name of the animation trigger
    [SerializeField] private string joustTriggerName = "Joust"; // The name of the joust animation trigger
    [SerializeField] private float swingDuration = 0.3f; // How long the swing animation takes
    [SerializeField] private float joustDuration = 0.4f; // How long the joust animation takes
    
    [Header("Controls")]
    [SerializeField] private KeyCode swingKey = KeyCode.K;
    [SerializeField] private KeyCode joustKey = KeyCode.L;
    
    [Header("Audio")]
    [SerializeField] private AudioClip swingSound;
    [SerializeField] private AudioClip joustSound;
    [SerializeField] private AudioSource audioSource;
    
    private bool isHeld = false;
    private Transform playerTransform;
    private bool canAttack = true;
    private float cooldownTimer = 0f;
    private PlayerMovement playerMovement;
    private PossessedMovement possessedMovement;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private bool isAnimating = false;
    
    void Start()
    {
        // Make sure sword has "Pushable" tag so it can be picked up with shift
        if (gameObject.tag != "Pushable")
        {
            Debug.LogWarning("Sword should have 'Pushable' tag to be picked up!");
        }
        
        if (!audioSource && GetComponent<AudioSource>() == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Get sword animator if not set
        if (swordAnimator == null)
        {
            swordAnimator = GetComponent<Animator>();
            if (swordAnimator == null)
            {
                // If there's no animator on this object, look in children
                swordAnimator = GetComponentInChildren<Animator>();
            }
        }
        
        // Store original rotation and scale
        originalRotation = transform.localRotation;
        originalScale = transform.localScale;
    }
    
    void Update()
    {
        if (!isHeld || isAnimating) return;
        
        // Handle cooldown for attacks
        if (!canAttack)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                canAttack = true;
            }
        }
        
        // If sword is held and player presses K, swing it
        if (canAttack && Input.GetKeyDown(swingKey))
        {
            SwingSword();
        }
        
        // If sword is held and player presses L, joust
        if (canAttack && Input.GetKeyDown(joustKey))
        {
            JoustWithSword();
        }
    }
    
    // Called when the player picks up the sword
    public void OnPickup(Transform player)
    {
        isHeld = true;
        playerTransform = player;
        
        // Get reference to the player movement script
        playerMovement = player.GetComponent<PlayerMovement>();
        possessedMovement = player.GetComponent<PossessedMovement>();
        
        // Reset rotation when picked up
        transform.localRotation = originalRotation;
    }
    
    // Called when the player drops the sword
    public void OnDrop()
    {
        isHeld = false;
        playerTransform = null;
        playerMovement = null;
        possessedMovement = null;
    }
    
    private void SwingSword()
    {
        // Play the sword swing animation directly
        if (swordAnimator != null)
        {
            swordAnimator.SetTrigger(swingTriggerName);
        }
        else
        {
            // If there's no animator, do a simple rotation animation
            StartCoroutine(SwingAnimation());
        }
        
        // Play swing sound
        if (audioSource != null && swingSound != null)
        {
            audioSource.PlayOneShot(swingSound);
        }
        
        // Start cooldown
        canAttack = false;
        cooldownTimer = swingCooldown;
        
        // Check for hits using the player's look direction
        Vector2 lookDirection = GetLookDirection();
        
        // Cast circle to detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            playerTransform.position + (Vector3)lookDirection, 
            hitRadius, 
            enemyLayer
        );
        
        // Apply damage to enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            // Example of how to deal damage if you have a health component
            // Health enemyHealth = enemy.GetComponent<Health>();
            // if (enemyHealth != null)
            // {
            //     enemyHealth.TakeDamage(swingDamage);
            // }
            
            Debug.Log("Hit enemy with swing: " + enemy.name + " for " + swingDamage + " damage");
        }
    }
    
    private void JoustWithSword()
    {
        // Play the jousting animation
        if (swordAnimator != null)
        {
            swordAnimator.SetTrigger(joustTriggerName);
        }
        else
        {
            // If there's no animator, do a simple thrusting animation
            StartCoroutine(JoustAnimation());
        }
        
        // Play joust sound
        if (audioSource != null && joustSound != null)
        {
            audioSource.PlayOneShot(joustSound);
        }
        else if (audioSource != null && swingSound != null)
        {
            // Fallback to swing sound if no joust sound
            audioSource.PlayOneShot(swingSound);
        }
        
        // Start cooldown
        canAttack = false;
        cooldownTimer = swingCooldown * 1.5f; // Longer cooldown for jousting
        
        // Get player look direction
        Vector2 lookDirection = GetLookDirection();
        
        // Cast a raycast in the look direction for jousting (more of a forward thrust)
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            playerTransform.position,
            lookDirection,
            joustRange,
            enemyLayer
        );
        
        // Apply more damage to enemies hit by joust
        foreach (RaycastHit2D hit in hits)
        {
            // Example of how to deal damage if you have a health component
            // Health enemyHealth = hit.collider.GetComponent<Health>();
            // if (enemyHealth != null)
            // {
            //     enemyHealth.TakeDamage(joustDamage);
            // }
            
            Debug.Log("Hit enemy with joust: " + hit.collider.name + " for " + joustDamage + " damage");
        }
    }
    
    // Helper method to get the player's look direction
    private Vector2 GetLookDirection()
    {
        Vector2 lookDirection = Vector2.right;
        if (playerMovement != null)
        {
            lookDirection = playerMovement.GetLastLookDirection();
        }
        else if (possessedMovement != null)
        {
            lookDirection = possessedMovement.GetLastLookDirection();
        }
        
        // Normalize the direction
        if (lookDirection != Vector2.zero)
        {
            lookDirection.Normalize();
        }
        
        return lookDirection;
    }
    
    // Simple hardcoded swing animation if no animator is present
    private IEnumerator SwingAnimation()
    {
        isAnimating = true;
        
        // Get the player's facing direction
        bool facingRight = true;
        if (playerMovement != null)
        {
            facingRight = playerMovement.SpriteFacingRight;
        }
        else if (possessedMovement != null)
        {
            facingRight = possessedMovement.SpriteFacingRight;
        }
        
        // Initial position
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;
        
        // Swing parameters - inverse angle based on facing direction
        float swingAngle = facingRight ? -120f : 120f;
        
        // Swing animation
        float elapsed = 0f;
        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / swingDuration;
            
            // First half of swing goes one way, second half returns
            float angle;
            if (normalizedTime < 0.5f)
            {
                angle = Mathf.Lerp(0, swingAngle, normalizedTime * 2);
            }
            else
            {
                angle = Mathf.Lerp(swingAngle, 0, (normalizedTime - 0.5f) * 2);
            }
            
            transform.localRotation = startRot * Quaternion.Euler(0, 0, angle);
            
            yield return null;
        }
        
        // Return to original rotation
        transform.localRotation = startRot;
        isAnimating = false;
    }
    
    // Simple hardcoded joust animation
    private IEnumerator JoustAnimation()
    {
        isAnimating = true;
        
        // Get the player's facing direction
        bool facingRight = true;
        if (playerMovement != null)
        {
            facingRight = playerMovement.SpriteFacingRight;
        }
        else if (possessedMovement != null)
        {
            facingRight = possessedMovement.SpriteFacingRight;
        }
        
        // Initial position and rotation
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;
        
        // Set to horizontal position for jousting
        transform.localRotation = startRot * Quaternion.Euler(0, 0, facingRight ? 0 : 180);
        
        // Thrust forward animation
        float elapsed = 0f;
        Vector3 thrustOffset = new Vector3(facingRight ? 0.5f : -0.5f, 0, 0);
        
        // Forward thrust
        while (elapsed < joustDuration * 0.4f)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / (joustDuration * 0.4f);
            
            transform.localPosition = Vector3.Lerp(startPos, startPos + thrustOffset, normalizedTime);
            
            yield return null;
        }
        
        // Hold at extended position briefly
        yield return new WaitForSeconds(joustDuration * 0.2f);
        
        // Pull back
        elapsed = 0f;
        while (elapsed < joustDuration * 0.4f)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / (joustDuration * 0.4f);
            
            transform.localPosition = Vector3.Lerp(startPos + thrustOffset, startPos, normalizedTime);
            
            yield return null;
        }
        
        // Return to original position and rotation
        transform.localPosition = startPos;
        transform.localRotation = startRot;
        isAnimating = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        // Visualize the hit radius in the editor
        Gizmos.color = Color.red;
        Vector3 position = transform.position;
        if (playerTransform != null)
        {
            Vector2 lookDirection = GetLookDirection();
            position = playerTransform.position + (Vector3)lookDirection;
            
            // Also draw the joust range
            Gizmos.DrawRay(playerTransform.position, lookDirection * joustRange);
        }
        Gizmos.DrawWireSphere(position, hitRadius);
    }
} 