using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Damager
{
	[Header("Melee Properties")]
	public Animator MeleeWeaponAnimator;
	private Collider2D col; //Collider that deals the damage

	private void Start()
	{
		col = GetComponent<Collider2D>();
		MeleeWeaponAnimator = GetComponent<Animator>();
	}

	public override void WeaponStart(Transform wielderPosition, Vector2 lastLookDirection)
	{
		base.WeaponStart(wielderPosition, lastLookDirection);

		if (MeleeWeaponAnimator)
		{
			MeleeWeaponAnimator.SetFloat("Horizontal", lastLookDirection.x);
			MeleeWeaponAnimator.SetFloat("Vertical", lastLookDirection.y);

			if (TopDown)
				MeleeWeaponAnimator.SetTrigger("Attack_TopDown");
			else
				MeleeWeaponAnimator.SetTrigger("Attack_SideScroller");
		}

		col.enabled = true;
	}

	public override void WeaponFinished()
	{
		col.enabled = false;
	}
}
