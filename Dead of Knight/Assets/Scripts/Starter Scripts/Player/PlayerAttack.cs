using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    [Tooltip("If true, uses the player’s own attack animation (the same Animator as your player).")]
    public bool UsePlayerAttackAnimations = true;

    [Tooltip("This is the player's animator (usually the same one in your PlayerMovement script).")]
    public Animator anim;

    public Rigidbody2D rb;
    public PlayerMovement playerMoveScript;

    [Header("Player Weapons")]
    [Tooltip("List of all weapons the player can use.")]
    public List<Damager> weaponList;

    [Tooltip("The current weapon the player is using.")]
    public Damager weapon;

    [Tooltip("Cooldown time before next attack.")]
    public float coolDown = 0.4f;

    [Header("Audio")]
    public PlayerAudio playerAudio;

    private bool canAttack = true;

    private void Start()
    {
        // This is the player's own animator, presumably on the same object
        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        playerMoveScript = GetComponent<PlayerMovement>();
        playerAudio = GetComponent<PlayerAudio>();

        // If no weapon is assigned, grab the first from the list
        if (weapon == null && weaponList.Count > 0)
        {
            weapon = weaponList[0];
        }
    }

    private void Update()
{
    // Example weapon switching logic
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
        if (weaponList.Count > 0) switchWeaponAtIndex(0);
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2))
    {
        if (weaponList.Count > 1) switchWeaponAtIndex(1);
    }

    // Attack only on the frame the user presses Left Click
    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
        Attack();

        // Play an attack sound if assigned
        if (playerAudio && !playerAudio.AttackSource.isPlaying && playerAudio.AttackSource.clip != null)
        {
            playerAudio.AttackSource.Play();
        }
    }
    // Stop the attack when the user releases the mouse button
    else if (Input.GetKeyUp(KeyCode.Mouse0))
    {
        StopAttack();
    }
}

    public void Attack()
    {
        // Only attack if we have a weapon and are not on cooldown
        if (weapon && canAttack)
        {
            StartCoroutine(CoolDown());

            // Because we want the player's animator controlling the sword:
            // If UsePlayerAttackAnimations == true, call the player’s "isAttacking" trigger
            if (UsePlayerAttackAnimations && playerMoveScript != null)
            {
                playerMoveScript.TriggerPlayerAttackAnimation();
            }

            // Start the weapon logic (collider damage, etc.)
            if (weapon is ProjectileWeapon projectile)
            {
                projectile.WeaponStart(this.transform, playerMoveScript.GetLastLookDirection(), rb.velocity);
            }
            else
            {
                weapon.WeaponStart(this.transform, playerMoveScript.GetLastLookDirection());
            }
        }
    }

    public void StopAttack()
    {
        // Stop the weapon’s damage
        if (weapon)
        {
            weapon.WeaponFinished();
        }
    }

    public void switchWeaponAtIndex(int index)
    {
        if (index < weaponList.Count && weaponList[index] != null)
        {
            if (weapon) weapon.gameObject.SetActive(false);

            weapon = weaponList[index];
            weapon.gameObject.SetActive(true);
        }
    }

    private IEnumerator CoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(coolDown);
        canAttack = true;
    }
}
