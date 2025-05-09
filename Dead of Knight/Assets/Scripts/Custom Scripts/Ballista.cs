using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista : MonoBehaviour
{
    public GameObject bullet;
    public int BallistaType;
    private Animator animator;
    //1 for up, 2 for right, 3 for down, 4 for left
    // Start is called before the first frame update

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

    void shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Fire");
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            Debug.Log("Pew");
            if (BallistaType == 1)
            {
                newBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
            }
            else if (BallistaType == 2)
            {
                newBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
            }
            else if (BallistaType == 3)
            {
                newBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 10f, ForceMode2D.Impulse);
            }
            else
            {
                newBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 10f, ForceMode2D.Impulse);
            }
        }
    }
}