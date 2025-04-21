using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Untagged")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Vector2 direction = -(collision.gameObject.transform.position - this.transform.position).normalized;
            rb.AddForce(direction * 5, ForceMode2D.Impulse);
            Invoke("Freeze", 1f);
        }
    }

    void Freeze()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePosition;
    }

}
