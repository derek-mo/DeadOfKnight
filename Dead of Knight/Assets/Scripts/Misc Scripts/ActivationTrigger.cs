using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationTrigger : MonoBehaviour {

    [SerializeField] private GameObject displayed;
    public bool permanentActivation = true;
    void Start()
    {
        displayed.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            displayed.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!permanentActivation)
        {
            if (collision.CompareTag("Player"))
            {
                displayed.SetActive(false);
            }
        }
        
    }
}
