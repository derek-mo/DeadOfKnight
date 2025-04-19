using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject targetObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile")) {
            Debug.Log("hit");
            if (targetObject != null)
            {
                targetObject.SetActive(false); // Make the object disappear/open
            }
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
