using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessable : MonoBehaviour
{
    public float speed = 10;

    // Start is called before the first frame update
    public void Start()
    {
        //Object Names will be the names of objects we can possess
        if (gameObject.name == "Triangle")
        {
            GetComponent<Triangle>().enabled = true;
        }
        else if (gameObject.name == "Square")
        {
            GetComponent<Square>().enabled = true;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        move();

        exorcize();
    }

    //Not all Possessable objects will have move(). This is for demo purposes
    public void move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;

        transform.position += tempVect;
    }

    //All possable objects will have exorcize()
    public void exorcize()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            this.tag = "Possessable";
            GetComponent<Possessable>().enabled = false;
            GameObject ghost = GameObject.FindGameObjectWithTag("Player");
            ghost.GetComponent<SpriteRenderer>().enabled = true;
            if (gameObject.name == "Triangle")
            {
                gameObject.GetComponent<Triangle>().enabled = false;
            }
            if (gameObject.name == "Square")
            {
                gameObject.GetComponent<Square>().enabled = false;
            }
        }
        
    }
}


