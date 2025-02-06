using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedObject : MonoBehaviour
{
    //"Locked" object that can be opened with an Item in the players inventory that matches certain conditions

    [Header("Type")]
    [Tooltip("the Game Object to be changed.")]
    public GameObject unlockGameObject;
    [Tooltip("Set True for affected Game Object to be activated or False to be deactivated when a key is used.")]
    public bool setActive = false;
    public bool oneTimeActivation = true;
    private bool used = false;

    [Header("Key Info")]
    [Tooltip("Name of the key that is required to unlock the gameObject")]
    public string keyName = "";
    [Tooltip("ID of the key that is required to unlock the gameObject")]
    public int keyID = 0;

    [Header("Key Behaviour")]
    public bool useKeyName = true;
    public bool useKeyID = false;

    private PlayerInventory inv;

    private void Start()
    {
        inv = FindObjectOfType<PlayerInventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!used)
        {
            foreach (PlayerInventory.Item i in inv.inventory)
            {
                if (useKeyName && i.itemName.ToLower() == keyName.ToLower())
                {
                    unlockGameObject.SetActive(setActive);

                    behaviours(i);
                    break;
                }
                if (useKeyID && i.itemID == keyID)
                {
                    unlockGameObject.SetActive(setActive);

                    behaviours(i);
                    break;
                }
            }


        }
    }


    private void behaviours(PlayerInventory.Item item)
    {
        //Remove Item
        if (item.destroyOnUse)
        {
            inv.RemoveItemFromInventory(item);
        }
        //Set activation
        if (oneTimeActivation)
        {
            used = true;
        }
    }
}
