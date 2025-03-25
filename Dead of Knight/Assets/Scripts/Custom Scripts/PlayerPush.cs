using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerPush : MonoBehaviour {
    public float rayDistance = 1f;
    public LayerMask objMask;
    public Transform pushPoint;
    public Transform raycastPoint;

    private GameObject obj;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
        Physics2D.queriesStartInColliders = false;
        
        RaycastHit2D hit = Physics2D.Raycast (raycastPoint.position, Vector2.right * transform.localScale.x, rayDistance, objMask);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Pushable") && Input.GetKey(KeyCode.LeftShift) && obj == null) {
            obj = hit.collider.gameObject;

            obj.transform.position = pushPoint.position;
            obj.transform.SetParent(transform);
            
            // If the object has a SwordController component, notify it that it's been picked up
            SwordController sword = obj.GetComponent<SwordController>();
            if (sword != null) {
                sword.OnPickup(transform);
            }

        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            if (obj != null) {
                // If the object has a SwordController component, notify it that it's been dropped
                SwordController sword = obj.GetComponent<SwordController>();
                if (sword != null) {
                    sword.OnDrop();
                }
                
                obj.transform.SetParent(null);
                obj = null;
            }
        }
        
        if (obj != null) {
            obj.transform.position = pushPoint.position;
        }
	}

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine (raycastPoint.position, (Vector2)raycastPoint.position + Vector2.right * transform.localScale.x * rayDistance);
    // }
}

