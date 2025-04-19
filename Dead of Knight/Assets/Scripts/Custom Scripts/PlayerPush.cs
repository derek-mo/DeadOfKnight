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

            if (collider != null && collider.gameObject.CompareTag("Pushable") && Input.GetKey(KeyCode.Space) && obj == null)
            {
                //Debug.Log("Found Pushable Item: " + hit.collider.gameObject.name);
                obj = collider.gameObject;

                obj.transform.position = pushPoint.position;
                // obj.GetComponent<FixedJoint2D> ().enabled = true;
                // obj.GetComponent<FixedJoint2D> ().connectedBody = gameObject.GetComponent<Rigidbody2D>();
                //obj.GetComponent<objpull> ().beingPushed = true;
                obj.transform.SetParent(transform);

            }
            else if (Input.GetKeyUp(KeyCode.Space))
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

