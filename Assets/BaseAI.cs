using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
///  Handles movement of an enemy unit and probably some other stuff
/// </summary>
[RequireComponent(typeof(StateController))]
[RequireComponent(typeof(Rigidbody))]
public class BaseAI : MonoBehaviour 
{
	private Rigidbody RB;
	private Vector3 velocity;
	private Vector3 burstVel;
	private EnemyStats stats;
    private Animator m_Animator;

	private Transform lookatTarget;

	[HideInInspector] public Vector3 forceLook;

	// Use this for initialization
	private void Start ()
	{
		RB = GetComponent<Rigidbody>();
		RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

		stats = GetComponent<StateController>().enemyStats;

        m_Animator = GetComponent<Animator>();
    }

	private void FixedUpdate ()
	{
		if (velocity == Vector3.zero)
		{
			velocity.y = 0; //Only operate on X and Z (Fixes spazzing out)

			//Move
			var finalVel = (velocity.normalized * stats.moveSpeed) +
			               ((burstVel.normalized * stats.moveSpeed) * 2);

			RB.MovePosition(transform.position + finalVel);
			Debug.DrawLine(transform.position, transform.position + finalVel * 100, Color.red);
		}

		//Face direction we're moving in, override if we have to
		var look = Quaternion.LookRotation(velocity.normalized);

		if (lookatTarget)
		{
			var l = (lookatTarget.position - transform.position).normalized;
			look = Quaternion.LookRotation(l);
		}

		Debug.DrawLine(transform.position,transform.position + (look.eulerAngles * 5), Color.magenta);
	
		
		RB.MoveRotation(Quaternion.Lerp(transform.rotation, look, 0.1f));

		if(burstVel.sqrMagnitude > 0)
			burstVel *= 0.9f;
		
		velocity = Vector3.zero;

		//test
        UpdateAnimator(transform.position + finalVel);

	}

	/// <summary>
	/// Add velocity to this agent
	/// </summary>
	/// <param name="v"></param>
	public void Move(Vector3 v)
	/// <summary>
	{
		velocity += v;
	}

	/// Move this agent but don't normalize the velocity
	/// </summary>
	/// <param name="vUnclamped"></param>
	public void BurstMove(Vector3 vUnclamped)
	{
		burstVel += vUnclamped;
	}

	public void ForceLookAt(Transform cTarget)
	{
		lookatTarget = cTarget;
	}

	//from ThirdPersonCharacter, so the animations play when it moves
    void UpdateAnimator(Vector3 move)
    {
        float m_TurnAmount = Mathf.Atan2(move.x, move.z);
        float m_ForwardAmount = move.z;
        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);

    }
}
