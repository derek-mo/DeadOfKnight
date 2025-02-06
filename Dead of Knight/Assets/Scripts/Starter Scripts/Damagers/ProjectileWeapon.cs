using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Damager
{
	[Header("Projectile Properties")]
	public GameObject Projectile;
	public Transform ProjectileSpawn;
	public bool WeaponOnLeft = false;
	public bool ShootTowardsMouse = false;
	public float Force = 100f;
	public float Duration = 10f;
	[Tooltip("The starting velocity that a projectile might have")]
	public Vector2 InitialProjectileVelocity;
	public bool Gravity = true;

	private Vector2 mousePosition;
	private float angle;

	private void FixedUpdate()
	{
		if (ShootTowardsMouse)
			mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	public override void WeaponStart(Transform wielderPosition, Vector2 lastLookDirection, Vector2 currentVelocity)
	{
		base.WeaponStart(wielderPosition, lastLookDirection);

		GameObject bullet = Instantiate(Projectile, ProjectileSpawn.position, Quaternion.identity);
		bullet.GetComponent<Projectile>().SetValues(Duration, alignmnent, damageValue, this.TopDown);
		Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>(); ;

		if (!Gravity)
			rb.gravityScale = 0;

		if (!TopDown)
		{
			Vector2 direction = wielderPosition.right;
			if (WeaponOnLeft)
				direction *= -1;
			Vector2 InitialVelocity = new Vector2((InitialProjectileVelocity.x + currentVelocity.x)*wielderPosition.localScale.x * direction.x, InitialProjectileVelocity.y + currentVelocity.y);
			rb.AddForce(InitialVelocity + Force * direction * wielderPosition.localScale.x, ForceMode2D.Impulse);
		}
		else
		{
			if (ShootTowardsMouse)
				rb.AddForce((new Vector3(currentVelocity.x, currentVelocity.y, 0f)) + (Vector3.Normalize(new Vector3(mousePosition.x, mousePosition.y, 0f) - transform.position)) * Force, ForceMode2D.Impulse);
			else
				rb.AddForce((InitialProjectileVelocity * lastLookDirection) + currentVelocity + lastLookDirection * Force, ForceMode2D.Impulse);
		}
	}
}
