using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
	public bool TopDown = false;

	public enum Alignment
	{
		Player,
		Enemy,
		Environment
	}

	[Tooltip("This determines whose side the damager is on. If the player is wielding the weapon, then its alignment is Player")]
	public Alignment alignmnent = Alignment.Player;
	// public WeaponType weaponType = WeaponType.Melee;

	[Tooltip("How much damage is dealt by this damager")]
	public int damageValue;

	public virtual void WeaponStart(Transform wielderPosition, Vector2 lastLookDirection) { }
	public virtual void WeaponStart(Transform wielderPosition, Vector2 lastLookDirection, Vector2 currentVelocity) { }

	public virtual void WeaponFinished() { }

}
