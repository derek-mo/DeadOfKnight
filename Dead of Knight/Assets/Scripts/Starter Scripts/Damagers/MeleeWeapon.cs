using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Damager
{
    [Header("Melee Properties")]
    private Collider2D col; //Collider that deals the damage

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    public override void WeaponStart(Transform wielderPosition, Vector2 lastLookDirection)
    {
        // Base Damager logic (if any)
        base.WeaponStart(wielderPosition, lastLookDirection);

        // Enable the collider so we can deal damage
        col.enabled = true;
    }

    public override void WeaponFinished()
    {
        // Disable the collider when not attacking
        col.enabled = false;
    }
}
