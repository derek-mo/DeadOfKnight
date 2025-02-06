using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Damager
{
	float duration;
	Damager.Alignment alignment;

	private void Start()
	{
		StartCoroutine(Duration());
	}

	public Projectile(float duration, Damager.Alignment alignment, int damageValue, bool TopDown)
	{
		this.duration = duration;
		this.alignment = alignment;
		this.damageValue = damageValue;
		this.TopDown = TopDown;
	}

	public void SetValues(float duration, Damager.Alignment alignment, int damageValue, bool TopDown)
	{
		this.duration = duration;
		this.alignment = alignment;
		this.damageValue = damageValue;
		this.TopDown = TopDown;

		if (TopDown)
		{
			GetComponent<Rigidbody2D>().gravityScale = 0;
		}
	}

	IEnumerator Duration()
	{
		yield return new WaitForSeconds(duration);
		Destroy(gameObject);
	}

}
