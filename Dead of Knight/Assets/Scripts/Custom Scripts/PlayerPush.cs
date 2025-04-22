/*
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerPush : MonoBehaviour
{
    public float rayDistance = 1f;
    public LayerMask objMask;
    public Transform pushPoint;
    public GameObject popup;
    //public Transform raycastPoint;

    private GameObject obj;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (Possession.isPossessing) {
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
            show_popup();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 position = new Vector2(transform.position.x, transform.position.y - 0.25f);

        Gizmos.DrawWireSphere(position, 1f);
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Pushable")){
    //         popup.GetComponent<Renderer>().enabled = true;
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     popup.GetComponent<Renderer>().enabled = false;
    // }

    public void show_popup()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (var collider in colliders)
        {
           if (collider.CompareTag("Pushable") && Possession.isPossessing)
             {
               popup.GetComponent<SpriteRenderer>().enabled = true;
               return;
             }
        }
        popup.GetComponent<SpriteRenderer>().enabled = false;
    }
}
*/

using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    [Header("Push Settings")]
    [SerializeField] private float rayDistance = 1f;          // radius for overlap check
    [SerializeField] private LayerMask objMask;               // mask to filter Pushable objects
    [SerializeField] private float pushOffset = 1f;           // distance from player center
    [SerializeField] private GameObject popup;                // the “Press Space to Push” UI

    private GameObject obj;                                   // the box currently being pushed
    private PossessedMovement pm;    
    private Rigidbody2D objRb;
    private Collider2D playerCol;
    private Collider2D boxCol;                         // to read LastMovement

    private void Awake()
    {
        pm = GetComponent<PossessedMovement>();
        playerCol = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Possession.isPossessing)
        {
            Physics2D.queriesStartInColliders = false;

            // 1) Check for nearby pushable objects at feet-level
            Vector2 feetPos = new Vector2(transform.position.x, transform.position.y - 0.25f);
            Collider2D[] hits = Physics2D.OverlapCircleAll(feetPos, rayDistance, objMask);

            // 2) Pick up the first one when Space is held
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Pushable") && Input.GetKey(KeyCode.Space) && obj == null)
                {
                    obj = hit.gameObject;
                    objRb  = obj.GetComponent<Rigidbody2D>();
                    boxCol = obj.GetComponent<Collider2D>();

                    // ignore player↔box collision
                    Physics2D.IgnoreCollision(playerCol, boxCol, true);
                    break;
                }
            }

            // 3) Release when Space is lifted
            if (Input.GetKeyUp(KeyCode.Space) && obj != null)
            {
                var sr = obj.GetComponent<SpriteRenderer>();
                sr.sortingOrder = 1;
                Physics2D.IgnoreCollision(playerCol, boxCol, false);
                obj   = null;
                objRb = null;
                boxCol= null;
            }

            // 4) If we’re holding one, force its position out in front of the player
            if (obj != null)
            {
                Vector2 dir = pm.LastMovement;
                if (dir == Vector2.zero) dir = Vector2.up;  // default if standing still
                if (dir.y < 0f && Mathf.Abs(dir.y) > Mathf.Abs(dir.x)) {
                    var sr = obj.GetComponent<SpriteRenderer>();
                    sr.sortingOrder = 3; // set sorting order to 1
                }
                else {
                    var sr = obj.GetComponent<SpriteRenderer>();
                    sr.sortingOrder = 1; // set sorting order to 0
                }
                obj.transform.position = (Vector2)transform.position + dir * pushOffset;;
            }
        }

        show_popup();
    }

    public void show_popup()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (var collider in colliders)
        {
           if (collider.CompareTag("Pushable") && Possession.isPossessing)
             {
               popup.GetComponent<SpriteRenderer>().enabled = true;
               return;
             }
        }
        popup.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnDrawGizmos()
    {
        // visualize the OverlapCircle in the Scene view
        Gizmos.color = Color.red;
        Vector2 feetPos = new Vector2(transform.position.x, transform.position.y - 0.25f);
        Gizmos.DrawWireSphere(feetPos, rayDistance);
    }
}

