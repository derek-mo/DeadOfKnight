using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CollectibleItem : MonoBehaviour
{
	[Header("Collectible Manager: Collectible Options")]
	public bool isCollectible = false;
	public int collectibleValue = 1;
	private CollectibleManager cManager;

	//This component is placed on any object that is a keyItem pick up and to be placed in your "inventory"
	[Header("Inventory System: Item Details")]
	public bool isKeyItem = false;
	public string itemName = "Item";
	public int itemID = 0;
	[Tooltip("The sprite that will represent the item when displayed in the inventory. Defaults to spriteRender at time of pickup if empty")]
	public Sprite inventoryDisplaySprite;
	[Tooltip("Deactivates the pickup item from the scene, preventing further/duplicate pickups of the same item")]
	public bool singePickupInstance = false;
	[Tooltip("The item will be removed from inventory when used on a locked object")]
	public bool destroyOnUse = false;
	SpriteRenderer spriteRenderer;

	private void Start()
	{
		if (isCollectible)
		{
			cManager = FindObjectOfType<CollectibleManager>();
		}
		if (isKeyItem)
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			if (isCollectible)
			{
				cManager.Collected(collectibleValue);

			}
			if (isKeyItem)
			{
				Sprite sprite = spriteRenderer.sprite;
				if (inventoryDisplaySprite != null)
				{
					sprite = inventoryDisplaySprite;
				}
				collision.TryGetComponent<PlayerInventory>(out PlayerInventory inv);
				inv.AddItemToInventory(new PlayerInventory.Item(itemName, itemID, sprite, spriteRenderer.color, destroyOnUse));
			}

			if (singePickupInstance)
			{
				this.gameObject.SetActive(false);
			}

		}
	}
}
