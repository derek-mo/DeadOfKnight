using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolAreaBehaviourSerious : StateMachineBehaviour
{
	public float speed = 1;
	[Tooltip("To use Blend Tree it needs the following parameters: float \"distance\", float \"Horizontal\", float \"Vertical\", bool \"SpriteFacingRight\" ")]
	public bool useBlendTree = false;
	private Vector3 origin;
	private float waitTime;
	public float startWaitTime = 1;
	private Transform moveSpot;
	public float radius = 2;
	private Animator anim;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		origin = animator.transform.position; // store the origin where the patrol begins
		anim = animator;
		GameObject pos = new GameObject("Pos");
		pos.transform.position = animator.transform.position;
		moveSpot = pos.transform; // moveSpot is set to a transform

		waitTime = startWaitTime;
		moveSpot.position = randPos(); // set to a random position
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Transform enemyPos = animator.transform;
		float step = speed * Time.deltaTime;

		if(useBlendTree)
        {
			Vector3 blendTreePos = (moveSpot.position - enemyPos.transform.position).normalized;
			anim.SetFloat("Horizontal", blendTreePos.x);
			anim.SetFloat("Vertical", blendTreePos.y);

			FlipCheck(blendTreePos.x);
		}

		enemyPos.position = Vector2.MoveTowards(enemyPos.position, moveSpot.position, step); // move towards this position

		if (Vector2.Distance(enemyPos.position, moveSpot.position) < 0.2f) // if we have reached the position
		{
			// get a new position after idling for a bit

			if (waitTime <= 0)
			{
				moveSpot.position = randPos();
				waitTime = startWaitTime;
			}
			else // be idle
			{
				waitTime -= Time.deltaTime;
			}
		}
	}

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

	public Vector3 randPos()
	{
		return origin + new Vector3(Random.Range(-1 * radius, radius), Random.Range(-1 * radius, radius), 0);
	}
}
