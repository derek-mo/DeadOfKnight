using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[Header("References")]

	[Tooltip("There are separate animators for the weapon. If you have a player attack animation from a sprite sheet, make this true. If your attack animation is the weapon itself moving (separate from the player) this can be false.")]
	public bool UsePlayerAttackAnimations = false;
	public Animator anim;
	public Rigidbody2D rb;
	public PlayerMovement playerMoveScript;

	[Header("Player Weapons")]
	[Tooltip("This is the list of all the weapons that your player uses")]
	public List<Damager> weaponList;
	[Tooltip("This is the current weapon that the player is using")]
	public Damager weapon;
	[Tooltip("The coolDown before you can attack again")]
	public float coolDown = 0.4f;

	[Header("Audio")]
	public PlayerAudio playerAudio;

	private bool canAttack = true;


	private void Start()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		playerMoveScript = GetComponent<PlayerMovement>();
		playerAudio = GetComponent<PlayerAudio>();
		if (weapon == null && weaponList.Count > 0)
		{
			weapon = weaponList[0];
		}
		// switchWeaponAtIndex(0);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))//Here is where you can hit the "1" key on your keyboard to activate this weapon
		{
			if (weaponList.Count > 0)
			{
				switchWeaponAtIndex(0);
			}
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))//Remove this if you don't have multiple weapons
		{
			if (weaponList.Count > 1)
			{
				switchWeaponAtIndex(1);
			}
		}

		if (Input.GetKey(KeyCode.Mouse0))
		{
			Attack();
			if (playerAudio && !playerAudio.AttackSource.isPlaying && playerAudio.AttackSource.clip != null)
			{
				playerAudio.AttackSource.Play();
			}
		}
		else
		{
			StopAttack();
		}
	}

	public void Attack()
	{
		//This is where the weapon is rotated in the right direction that you are facing
		if (weapon && canAttack)
		{
			if (UsePlayerAttackAnimations)
            {
				Debug.Log("using player attack animation");
				playerMoveScript.TriggerPlayerAttackAnimation();

			}

			if (weapon is ProjectileWeapon)
				weapon.WeaponStart(this.transform, playerMoveScript.GetLastLookDirection(), rb.velocity);
			else
				weapon.WeaponStart(this.transform, playerMoveScript.GetLastLookDirection());

			StartCoroutine(CoolDown());
		}
	}

	public void StopAttack()
	{
		if (weapon)
		{
			weapon.WeaponFinished();
		}
	}

	public void switchWeaponAtIndex(int index)
	{
		//Makes sure that if the index exists, then a switch will occur
		if (index < weaponList.Count && weaponList[index])
		{
			//Deactivate current weapon
			weapon.gameObject.SetActive(false);

			//Switch weapon to index then activate
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
