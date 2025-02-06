using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
	//This class should be placed on anything enemy related! Or anything that the player can damage

	[Header("Settings")]
	public bool EnemyHealthBar = false;
	public int maxHealth = 100;
	public int currentHealth;

	[Header("Health Bar")]
	[Tooltip("Padding between healthbar and enemy")]
	public float padding = 2f;

	[Tooltip("Use this to control how wide or tall the health bar is")]
	public Vector2 Dimensions;

	[Tooltip("Prefab for healthbar")]
	public GameObject HealthBar;
	public RectTransform canvasRectTransform;

	private Image healthBarImage;
	private RectTransform healthRectTransform;

	private Animator anim;
	

	void Start()
	{
		currentHealth = maxHealth;
		anim = GetComponent<Animator>();
		if (EnemyHealthBar)
		{
			if (canvasRectTransform == null)
				canvasRectTransform = GameObject.FindGameObjectWithTag("EnemyHealthCanvas").GetComponent<RectTransform>();

			GameObject newHealthBar = Instantiate(HealthBar, transform.position, Quaternion.identity);
			healthRectTransform = newHealthBar.GetComponent<RectTransform>();

			newHealthBar.transform.SetParent(canvasRectTransform);

			healthBarImage = newHealthBar.GetComponent<Image>();
			healthBarImage.type = Image.Type.Filled;

			UpdateHealthBar();
			healthRectTransform.sizeDelta += Dimensions;
		}
	}

	void Update()
	{
		if (EnemyHealthBar)
		{
			Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
			healthRectTransform.anchoredPosition = screenPoint - canvasRectTransform.sizeDelta / 2f;
			healthRectTransform.anchoredPosition += new Vector2(0f, padding);
			//healthRectTransform.sizeDelta += Dimensions;

		}
	}

	public void DecreaseHealth(int value)
	{
		currentHealth -= value;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			anim.SetBool("isDead", true); // Jane Apostol Fall '23

			StartCoroutine(DestroyAfterAnimation()); // Jane Apostol Fall '23  Delay destruction of sprite until after animation
		}

		if (EnemyHealthBar)
			UpdateHealthBar();
	}

	private IEnumerator DestroyAfterAnimation() // Jane Apostol Fall '23
	{
		yield return new WaitForSeconds(2.0f);  // Adjust the delay based on your death animation duration
		Destroy(this.gameObject);
		Destroy(healthBarImage.gameObject);
	}

	void UpdateHealthBar()//Updates the health bar according to the new health amounts
	{
		float fillAmount = (float)currentHealth / maxHealth;
		if (fillAmount > 1)
		{
			fillAmount = 1.0f;
		}

		healthBarImage.fillAmount = fillAmount;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out Damager weapon))
		{
			if (weapon.alignmnent == Damager.Alignment.Player || weapon.alignmnent == Damager.Alignment.Environment)
			{
				DecreaseHealth(weapon.damageValue);

				if (EnemyHealthBar)
					UpdateHealthBar();
			}
		}
	}
}
