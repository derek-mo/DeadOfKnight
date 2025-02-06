using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPlatform : MonoBehaviour
{
    Animator anim;
    Collider2D _collider;
    SpriteRenderer spriteRenderer;

    public float respawnTime = 3.0f;
    public bool refreshOnExit = false;

    //Get the necessary components
    void Start()
    {
        anim = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetBool("Crumble", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (refreshOnExit)
        {
            if (collision.gameObject.tag == "Player")
            {
                anim.SetBool("Crumble", false);
            }
        }
        
    }

    //Use for an Animation Event
    public void Disappear()
    {
        Deactivation();
        StartCoroutine(RespawnTime());
    }

    //Respawn the platform after X amount of time
    private IEnumerator RespawnTime()
    {
        yield return new WaitForSeconds(respawnTime);
        anim.SetBool("Crumble", false);
        Activation();
    }

    private void Activation()
    {
        _collider.enabled = true;
        spriteRenderer.enabled = true;
    }

    private void Deactivation()
    {
        _collider.enabled = false;
        spriteRenderer.enabled = false;
    }

}
