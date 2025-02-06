using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
	public GameManager gameManager;
	public GameSceneManager gameSceneManager;

	[Header("Health")]
	[Tooltip("The max health the player can have")]
	public int maxHealth = 100;

	[Tooltip("The current health the player has")]
	public int currentHealth;

	[Tooltip("If you're using segameManagerented health, this is gameObject that holds your health icons as its children")]
	public GameObject HealthIcons;

	[Tooltip("This is the Bar of health that you use if you're doing non-segameManagerented health")]
	public GameObject HealthBar;

	private List<GameObject> Hearts = new List<GameObject>();//List of the GameObject hearts that you are using. These need to be in order

	private List<GameObject> TempHearts = new List<GameObject>();

	[Tooltip("If you actually want to use a healthbar or not")]
	public bool useHealthBar = false;

	[Header("Audio")]
	public PlayerAudio playerAudio;

	private Animator anim;


	[HideInInspector] public int index = 0; //for editor uses

	void Start()
	{
		SetUpHealth();
		anim = GetComponent<Animator>();
		playerAudio = GetComponent<PlayerAudio>();
	}

	public void SetUpHealth()
	{
		if (!useHealthBar)
		{
			Hearts.Clear();
			TempHearts.Clear();
			foreach (Transform child in HealthIcons.transform)
			{
				child.gameObject.GetComponent<Image>().color = Color.white;//This makes the color to white, you can make this a public variable if you want to change it
				Hearts.Add(child.gameObject);
				TempHearts.Add(child.gameObject);
			}
			currentHealth = TempHearts.Count;
		}
		else
		{
			if (HealthBar)
			{
				HealthBar.GetComponent<Image>().type = Image.Type.Filled;
				// HealthBar.GetComponent<Image>().fillMethod = (int)Image.FillMethod.Horizontal;
				// HealthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
				currentHealth = maxHealth;
				UpdateHealthBar();
			}
		}
	}

	public void DecreaseHealth(int value)//This is the function to use if you want to decrease the player's health somewhere
	{
		if (!useHealthBar)
		{
			SegameManagerentedHealthDecrease(value);
			return;
		}
		currentHealth -= value;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
		}
		UpdateHealthBar();
	}

	public void IncreaseHealth(int value)//This is the function to use if you want to increase the player's heath somewhere
	{
		if (!useHealthBar)
		{
			SegameManagerentedHealthIncrease(value);
			return;
		}
		currentHealth += value;
		if (currentHealth > maxHealth)
		{
			currentHealth = maxHealth;
		}
		UpdateHealthBar();
	}

	private void SegameManagerentedHealthDecrease(int value)//Helper function
	{
		if (value > TempHearts.Count)
		{
			value = TempHearts.Count;
		}
		for (int i = 0; i < value; i++)
		{
			TempHearts[currentHealth - 1].GetComponent<Image>().color = Color.black;
			TempHearts.RemoveAt(TempHearts.Count - 1);
			currentHealth--;
		}

		if (TempHearts.Count == 0)
		{
			currentHealth = 0;
		}
	}

	private void SegameManagerentedHealthIncrease(int value)//Helper function
	{
		if (value + TempHearts.Count > Hearts.Count)
		{
			value = Hearts.Count - TempHearts.Count;
		}

		for (int i = 0; i < value; i++)
		{
			var temp = Hearts[currentHealth];
			temp.GetComponent<Image>().color = Color.white;
			TempHearts.Add(temp);
			currentHealth++;
		}
	}

	public void ResetHealth()//Resets health back to normal
	{
		if (!useHealthBar)
		{
			for (int i = 0; i < Hearts.Count; i++)
			{
				Hearts[i].GetComponent<Image>().color = Color.white;
			}

			TempHearts.Clear();

			foreach (var VARIABLE in Hearts)
			{
				TempHearts.Add(VARIABLE);
			}
			currentHealth = TempHearts.Count;
		}
		else
		{
			currentHealth = maxHealth;
			UpdateHealthBar();
		}
	}

	void UpdateHealthBar()//Updates the health bar according to the new health amounts
	{
		if (useHealthBar)
		{
			float fillAmount = (float)currentHealth / maxHealth;
			if (fillAmount > 1)
			{
				fillAmount = 1.0f;
			}

			HealthBar.GetComponent<Image>().fillAmount = fillAmount;
		}
	}

	//This is where we handle the place where the health is dealth with
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Collider2D thisCollision = GetComponent<Collider2D>();
		if (collision.otherCollider == thisCollision)
		{
			if (collision.gameObject.TryGetComponent(out Damager weapon))
			{
				if (weapon.alignmnent == Damager.Alignment.Enemy ||
					weapon.alignmnent == Damager.Alignment.Environment)
				{
					DecreaseHealth(weapon.damageValue);
					if (currentHealth == 0)
					{
						TimeToDie();
					}
				}
			}
			if (collision.gameObject.TryGetComponent(out HealingItem healingValue))
			{
				IncreaseHealth(healingValue.HealAmount);
				if (healingValue.DestroyOnContact)
				{
					Destroy(collision.gameObject);
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Collider2D thisCollider = GetComponent<Collider2D>();
		if (collision.IsTouching(thisCollider))
		{
			if (collision.gameObject.TryGetComponent(out Damager weapon))
			{
				if (weapon.alignmnent == Damager.Alignment.Enemy ||
					weapon.alignmnent == Damager.Alignment.Environment)
				{
					DecreaseHealth(weapon.damageValue);
					if (currentHealth == 0)
					{
						TimeToDie();
					}
				}
			}
			if (collision.gameObject.TryGetComponent(out HealingItem healingValue))
			{
				IncreaseHealth(healingValue.HealAmount);
				if (healingValue.DestroyOnContact)
				{
					Destroy(collision.gameObject);
				}
			}
		}


	}

	private void TimeToDie()
	{
		StartCoroutine(PlayerDies());
	}

	IEnumerator PlayerDies()
	{
		if (gameManager != null && gameSceneManager != null)
		{
			anim.SetBool("isDead", true);
			gameManager.DisablePlayerMovement(true);
			if (playerAudio && !playerAudio.DeathSource.isPlaying && playerAudio.DeathSource.clip != null)
			{
				playerAudio.DeathSource.Play();
			}
			yield return new WaitForSeconds(1f);
			StartCoroutine(gameSceneManager.FadeOut());

			yield return new WaitForSeconds(1f);
			gameManager.Respawn(gameObject);
			StartCoroutine(gameSceneManager.FadeIn());
			yield return new WaitForSeconds(1f);
			ResetHealth();
			gameManager.DisablePlayerMovement(false);
			anim.SetBool("isDead", false);
		}
		else
		{
			Debug.Log("Game Manager or Game Scene Manager not assigned on player!");
		}
	}
}
