using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public float jumpStrength = 1.0f;

    public float rotationSpeed = 1.0f;
    public float verticalAngleLimit = 85.0f;

    private Vector3 currentRotation;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //Grab the rigidbody we want to manipulate for movement
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //=======Lateral movement==========
        MovePlayer();
        //======Jumping========
        Jump();
        //=======Rotation=========
        Rotate();
    }


    void MovePlayer()
    {
        //A "good" way for lateral player movement.
        //Note: there are many ways to do most things in Unity, other implementations may be more advantegeous depending on context.


        //Idea:
        //1) Get input from WASD keys to determine direction of movement.
        //2) Normalize, then scale based on movementSpeed.
        //3) Apply the velocity to the player's rigidbody.

        //Start with no lateral movement AND save y velocity
        Vector3 direction = new Vector3(0,0,0);

        //Q1) What happens if we dont save this value?
        float y = rb.velocity.y;

        //For each movement key, update direction of movement
        //Q2) What would happen if the last three if statements were else-if statements?
        //Q3) What would happen if "Camera.main" was replaced with "gameObject"?
        if (Input.GetKey(KeyCode.W))
        {
            direction += Camera.main.transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction -= Camera.main.transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction -= Camera.main.transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Camera.main.transform.right;
        }


        //TODO: Add a "sprint" feature.


        //Get the normalized vector, then scale based on the current speed
        //Q4) Why do we need to normalize here?
        Vector3 velocity = direction.normalized * movementSpeed;


        //Add back the y component
        velocity.y = y;
        //apply the velocity to the player
        rb.velocity = velocity;
    }

    void Jump()
    {
        //When the Space bar is pressed, apply a positive vertical force
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(gameObject.transform.up*jumpStrength, ForceMode.Impulse);
        }

    }

    void Rotate()
    {
        //Get "strength" of horizontal and verical mouse movements
        currentRotation.x += Input.GetAxis("Mouse X") * rotationSpeed;
        currentRotation.y -= Input.GetAxis("Mouse Y") * rotationSpeed;

        //X rotation is looped based on 360 degrees
        currentRotation.x = Mathf.Repeat(currentRotation.x, 360);

        //Y is clamped so the camera never flips
        currentRotation.y = Mathf.Clamp(currentRotation.y, -verticalAngleLimit, verticalAngleLimit);

        //rotate the player's view
        Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}

