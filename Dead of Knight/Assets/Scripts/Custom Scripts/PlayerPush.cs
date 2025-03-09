// using UnityEngine;
// using System.Collections;
// using Unity.VisualScripting;

// public class PlayerPush : MonoBehaviour {

// 		public float distance=1f;
// 		public LayerMask boxMask;

// 		GameObject box;
// 	// Use this for initialization
// 	void Start () {
	
// 	}
	
// 	// Update is called once per frame
//     void Update()
//     {
//         Physics2D.queriesStartInColliders = false;

//         // Cast Raycasts in all 4 directions
//         RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, distance, boxMask);
//         RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, distance, boxMask);
//         RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, distance, boxMask);
//         RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, distance, boxMask);

//         // Variable to store the hit pushable object
//         GameObject pushableBox = null;

//         // Check each direction for a pushable object
//         if (hitUp.collider != null && hitUp.collider.gameObject.CompareTag("Pushable"))
//             pushableBox = hitUp.collider.gameObject;
//         else if (hitDown.collider != null && hitDown.collider.gameObject.CompareTag("Pushable"))
//             pushableBox = hitDown.collider.gameObject;
//         else if (hitLeft.collider != null && hitLeft.collider.gameObject.CompareTag("Pushable"))
//             pushableBox = hitLeft.collider.gameObject;
//         else if (hitRight.collider != null && hitRight.collider.gameObject.CompareTag("Pushable"))
//             pushableBox = hitRight.collider.gameObject;

//         // If a pushable object is detected and LeftShift is pressed
//         if (pushableBox != null && Input.GetKeyDown(KeyCode.LeftShift))
//         {
//             box = pushableBox;
//             FixedJoint2D joint = box.GetComponent<FixedJoint2D>();

//             if (joint != null)
//             {
//                 joint.enabled = true;
//                 joint.connectedBody = GetComponent<Rigidbody2D>();
//             }
//         }
//         // If LeftShift is released, detach the box
//         else if (Input.GetKeyUp(KeyCode.LeftShift) && box != null)
//         {
//             box.GetComponent<FixedJoint2D>().enabled = false;
//             box = null; // Clear the reference
//         }
//     }


// 		void OnDrawGizmos()
// 		{
// 				Gizmos.color = Color.yellow;

// 				Gizmos.DrawLine (transform.position, (Vector2)transform.position + Vector2.up * transform.localScale.x * distance);
// 				Gizmos.DrawLine (transform.position, (Vector2)transform.position + Vector2.down * transform.localScale.x * distance);
// 				Gizmos.DrawLine (transform.position, (Vector2)transform.position + Vector2.left * transform.localScale.x * distance);
// 				Gizmos.DrawLine (transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);

// 		}
// }

using UnityEngine;
using System.Collections;

public class PlayerPush : MonoBehaviour {
    public float distance=1f;
    public LayerMask boxMask;

    GameObject box;

	// Use this for initialization
	void Start () {
        Debug.Log("PlayerPush script started");
	}
	
	// Update is called once per frame
	void Update () {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.right * transform.localScale.x, distance, boxMask);

        //Debug.Log(hit.collider != null && hit.collider.gameObject.CompareTag("Pushable") && Input.GetKey(KeyCode.LeftShift));    

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Pushable") && Input.GetKey(KeyCode.LeftShift)) {
            Debug.Log("Hit");
            box = hit.collider.gameObject;
            Debug.Log ("Box: " + box.gameObject.name);

            box.GetComponent<FixedJoint2D> ().enabled = true;
            box.GetComponent<FixedJoint2D> ().connectedBody = gameObject.GetComponent<Rigidbody2D>();
            //box.GetComponent<boxpull> ().beingPushed = true;

        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            box.GetComponent<FixedJoint2D> ().enabled = false;
            //box.GetComponent<boxpull> ().beingPushed = false;
        }
		
	}


    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine (transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    }
}