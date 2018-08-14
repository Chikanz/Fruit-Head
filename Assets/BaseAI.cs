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
	private EnemyStats stats;
    private Animator m_Animator;

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
		if (velocity == Vector3.zero) return;
		
		velocity.y = 0; //Only operate on X and Z (Fixes spazzing out)
		
		//Move
		var finalVel = velocity.normalized * stats.moveSpeed;
		RB.MovePosition(transform.position + finalVel);
		Debug.DrawLine(transform.position, transform.position + finalVel * 100, Color.red);

		//Face direction we're moving in
		var look = Quaternion.LookRotation(velocity.normalized);
		RB.MoveRotation(Quaternion.Lerp(transform.rotation, look, 0.1f));

        //test
        UpdateAnimator(transform.position + finalVel);

		velocity = Vector3.zero;
	}

	/// <summary>
	/// Add velocity to this agent
	/// </summary>
	/// <param name="v"></param>
	public void Move(Vector3 v)
	{
		velocity += v;
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
