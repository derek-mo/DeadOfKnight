/*
// using UnityEngine;
// using System.Collections;
// using Unity.VisualScripting;

// public class PlayerPush : MonoBehaviour {

// 		public float distance=1f;
// 		public LayerMask objMask;

// 		gameObject obj;
// 	// Use this for initialization
// 	void Start () {
	
// 	}
	
// 	// Update is called once per frame
//     void Update()
//     {
//         Physics2D.queriesStartInColliders = false;

//         // Cast Raycasts in all 4 directions
//         RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, distance, objMask);
//         RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, distance, objMask);
//         RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, distance, objMask);
//         RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, distance, objMask);

//         // Variable to store the hit pushable obj
//         gameObject pushableobj = null;

//         // Check each direction for a pushable obj
//         if (hitUp.collider != null && hitUp.collider.gameObject.CompareTag("Pushable"))
//             pushableobj = hitUp.collider.gameObject;
//         else if (hitDown.collider != null && hitDown.collider.gameObject.CompareTag("Pushable"))
//             pushableobj = hitDown.collider.gameObject;
//         else if (hitLeft.collider != null && hitLeft.collider.gameObject.CompareTag("Pushable"))
//             pushableobj = hitLeft.collider.gameObject;
//         else if (hitRight.collider != null && hitRight.collider.gameObject.CompareTag("Pushable"))
//             pushableobj = hitRight.collider.gameObject;

//         // If a pushable obj is detected and LeftShift is pressed
//         if (pushableobj != null && Input.GetKeyDown(KeyCode.LeftShift))
//         {
//             obj = pushableobj;
//             FixedJoint2D joint = obj.GetComponent<FixedJoint2D>();

//             if (joint != null)
//             {
//                 joint.enabled = true;
//                 joint.connectedBody = GetComponent<Rigidbody2D>();
//             }
//         }
//         // If LeftShift is released, detach the obj
//         else if (Input.GetKeyUp(KeyCode.LeftShift) && obj != null)
//         {
//             obj.GetComponent<FixedJoint2D>().enabled = false;
//             obj = null; // Clear the reference
//         }
//     }
// }

// ONLY HORIZONTAL SCRIPT

using UnityEngine;
using System.Collections;

public class PlayerPush : MonoBehaviour {
    public float distance=1f;
    public LayerMask objMask;

    gameObject obj;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.right * transform.localScale.x, distance, objMask);

        Debug.DrawRay(transform.position, Vector2.right * transform.localScale.x * distance, Color.red);
        Debug.Log("Raycast hit: " + (hit.collider != null ? hit.collider.gameObject.name : "Nothing"));


        //Debug.Log(hit.collider != null && hit.collider.gameObject.CompareTag("Pushable") && Input.GetKey(KeyCode.LeftShift));    

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Pushable") && Input.GetKey(KeyCode.LeftShift)) {
            //Debug.Log("Found Pushable Item: " + hit.collider.gameObject.name);
            obj = hit.collider.gameObject;

            obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            obj.GetComponent<Rigidbody2D>().freezeRotation = true;
            obj.GetComponent<FixedJoint2D> ().enabled = true;
            obj.GetComponent<FixedJoint2D> ().connectedBody = gameObject.GetComponent<Rigidbody2D>();
            //obj.GetComponent<objpull> ().beingPushed = true;

        } else if (Input.GetKeyUp(KeyCode.LeftShift) && obj != null) {
            obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            obj.GetComponent<FixedJoint2D> ().enabled = false;
            //obj.GetComponent<objpull> ().beingPushed = false;
        }
		
	}
}
*/

using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerPush : MonoBehaviour
{
    public float rayDistance = 1f;
    public LayerMask objMask;
    public Transform pushPoint;
    //public Transform raycastPoint;

    private GameObject obj;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        Physics2D.queriesStartInColliders = false;

        //RaycastHit2D hit = Physics2D.Raycast (raycastPoint.position, Vector2.right * transform.localScale.x, rayDistance, objMask);
        Vector2 position = new Vector2(transform.position.x, transform.position.y - 0.25f);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 1f);
        //Debug.Log(hit.collider != null && hit.collider.gameObject.CompareTag("Pushable") && Input.GetKey(KeyCode.E));    
        foreach (var collider in colliders)
        {

            if (collider != null && collider.gameObject.CompareTag("Pushable") && Input.GetKey(KeyCode.LeftShift) && obj == null)
           }
                //Debug.Log("Found Pushable Item: " + hit.collider.gameObject.name);
                obj = collider.gameObject;

                obj.transform.position = pushPoint.position;
                // obj.GetComponent<FixedJoint2D> ().enabled = true;
                // obj.GetComponent<FixedJoint2D> ().connectedBody = gameObject.GetComponent<Rigidbody2D>();
                //obj.GetComponent<objpull> ().beingPushed = true;
                obj.transform.SetParent(transform);

            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                obj.transform.SetParent(null);
                obj = null;
                //obj.GetComponent<FixedJoint2D> ().enabled = false;
                //obj.GetComponent<objpull> ().beingPushed = false;
            }

            if (obj != null)
            {
                obj.transform.position = pushPoint.position;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 position = new Vector2(transform.position.x, transform.position.y - 0.25f);

        Gizmos.DrawWireSphere(position, 1f);
    }
}

