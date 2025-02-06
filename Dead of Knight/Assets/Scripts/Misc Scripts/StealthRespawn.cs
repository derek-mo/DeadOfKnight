using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthRespawn : MonoBehaviour
{

	public bool isHidden = false;
	private GameObject respawn;

	void Start()
	{
		//If your player starts in cover, change this to true. Shouldn't be too big of an issue though.
		isHidden = false;
		respawn = GameObject.FindGameObjectWithTag("Respawn");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Cover"))
		{
			isHidden = true;
		}
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		// I couldn't think of a better tag name than GuardView. Feel free to change it if you come up with a better name
		if (collision.CompareTag("GuardView"))
		{
			if (!isHidden)
			{
				transform.position = respawn.transform.position;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Cover"))
		{
			isHidden = false;
		}
	}
}
