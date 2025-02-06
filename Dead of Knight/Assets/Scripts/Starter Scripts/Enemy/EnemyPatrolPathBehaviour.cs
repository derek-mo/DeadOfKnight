using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolPathBehaviour : StateMachineBehaviour
{
    public float speed = 1;
    public bool isLoop;
    [Tooltip("To use Blend Tree it needs the following parameters: float \"distance\", float \"Horizontal\", float \"Vertical\", bool \"SpriteFacingRight\" ")]
    public bool useBlendTree = false;
    private bool inReverse;
    public List<Transform> PathNodes;
    public Transform nextPos;
    private GameObject thisObject;
    private int i;
    private Animator anim;
    private bool SpriteFacingRight = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim = animator;
        thisObject = animator.gameObject;
        if(thisObject == null)
        {
            Debug.Log("anim");
        }
        Transform paths = thisObject.transform.parent.Find("Paths"); // load in an empty named Paths that contains transforms
        if(paths == null)
        {
            Debug.Log("path is null");
        }
        foreach (Transform path in paths)
        {
            PathNodes.Add(path); // add every path to our list
        }
        nextPos = PathNodes[0]; // the first target is the first transform

        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navigate();
    }
    
    private void setNextNode()
    {
        nextPos = PathNodes[i]; // set the next position
        if (i < PathNodes.Count - 1) // if we are within bounds and going forwards
        {
            if (!inReverse)
            {
                i++; // increment the iterator for the next time this function is called
            }
            else 
            {
                i--; // decrement the iterator for the next time this function is called
                if (i == 0)
                {
                    inReverse = false;
                }
            }
        }
        else
        {
            if (isLoop)
            {
                i = 0;
            }
            else 
            {
                inReverse = true;
                i --;
            }
        }
    }

    private void navigate()
    {
        if(thisObject.transform.position == nextPos.position) // if we have reached this position
        {
            setNextNode(); // set the next position
        }
        
        float step = speed * Time.deltaTime;

        if(useBlendTree)
        {
            Vector3 blendTreePos = (nextPos.position - thisObject.transform.position).normalized;
            anim.SetFloat("Horizontal", blendTreePos.x);
            anim.SetFloat("Vertical", blendTreePos.y);

            FlipCheck(blendTreePos.x);
        }

        thisObject.transform.position = Vector3.MoveTowards(thisObject.transform.position, nextPos.position, step); // move towards the next position
    }

    // Flip Check & Flip code from Player Movement Script
    private void FlipCheck(float move)
	{
        
		//Flip the sprite so that they are facing the correct way when moving
		if (move > 0 && !anim.GetBool("SpriteFacingRight")) //if moving to the right and the sprite is not facing the right.
		{
			Flip();
		}
		else if (move < 0 && anim.GetBool("SpriteFacingRight")) //if moving to the left and the sprite is facing right
		{
			Flip();
		}
	}

	private void Flip()
	{
		anim.SetBool("SpriteFacingRight", !anim.GetBool("SpriteFacingRight")); //flip whether the sprite is facing right
		Vector3 currentScale = anim.transform.localScale;
		currentScale.x *= -1;
		anim.transform.localScale = currentScale;
	}

}
