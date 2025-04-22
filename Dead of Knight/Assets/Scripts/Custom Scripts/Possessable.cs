using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessable : MonoBehaviour
{

    [SerializeField] private Sprite up;
    [SerializeField] private Sprite down;
    [SerializeField] private Sprite left;
    [SerializeField] private Sprite right;

    // Start is called before the first frame update
    public void Start() {
    }

    // Update is called once per frame
    public void Update() {
        if (!Possession.isPossessing) {
            if (up && down && left && right){
                var sr = gameObject.GetComponent<SpriteRenderer>();
                var pm = gameObject.GetComponent<PossessedMovement>();
                Vector2 dir = pm.LastMovement;
                if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                {
                    sr.sprite = dir.x > 0 ? right : left;
                }
                else
                {
                    sr.sprite = dir.y > 0 ? up : down;
                }
            }
        }
    }
}

