using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEActivationTrigger : MonoBehaviour {

    [SerializeField] private GameObject displayed;
    public bool permanentDeactivation = true;

    void Start()
    {
        displayed.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            displayed.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!permanentDeactivation)
        {
            if (collision.CompareTag("Player"))
            {
                displayed.SetActive(true);
            }
        }
        
    }
}
