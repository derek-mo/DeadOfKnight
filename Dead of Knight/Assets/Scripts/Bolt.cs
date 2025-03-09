using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("Collided");
        if (collision.gameObject.name == "armor_np")
        {
            rb.velocity = Vector3.zero;
            if (collision.gameObject.transform.localScale.x > 0)
                rb.AddForce(transform.right * 10, ForceMode2D.Impulse);
            else 
                rb.AddForce(transform.right * -10, ForceMode2D.Impulse);
        }
        if (collision.gameObject.name == "target")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
