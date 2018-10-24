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
    private Animator myAnim;

	private Transform lookatTarget;
	private EnemyManager _EM;

	// Use this for initialization
	private void Start ()
	{
		RB = GetComponent<Rigidbody>();
		RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

		stats = GetComponent<StateController>().enemyStats;

        myAnim = GetComponent<Animator>();

		_EM = GetComponentInParent<EnemyManager>();

		//Disable on ded
		var MyCC = GetComponent<CombatCharacter>();
		if (MyCC) MyCC.OnDefeat += (sender, args) => this.enabled = false;
	}

	private void FixedUpdate ()
	{
		velocity.y = 0; //Only operate on X and Z (Fixes spazzing out)
		
		if(velocity.magnitude > 1) velocity.Normalize();

		//Move
		var finalVel = (velocity * stats.moveSpeed) +
		               (burstVel * stats.moveSpeed);		               

		RB.MovePosition(transform.position + finalVel);
		Debug.DrawLine(transform.position, transform.position + finalVel * 100, Color.red);
		

		//Face direction we're moving in, override if we have to		
		var look = velocity == Vector3.zero ?
			transform.rotation : Quaternion.LookRotation(velocity.normalized); //Don't face velocity when not moving

		if (lookatTarget)
		{
			var l = (lookatTarget.position - transform.position).normalized;
			l.y = 0;
			look = Quaternion.LookRotation(l);
		}	
		
		RB.MoveRotation(Quaternion.Lerp(transform.rotation, look, 0.1f));

		if(burstVel.sqrMagnitude > 0)
			burstVel *= 0.98f;
		
		//walking for avery
		if(myAnim) myAnim.SetFloat("Walking", velocity.sqrMagnitude);
		
		velocity = Vector3.zero;		
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

	public void StopBurst()
	{
		burstVel = Vector3.zero;
	}

	public void ForceLookAt(Transform cTarget)
	{
		lookatTarget = cTarget;
	}
}
