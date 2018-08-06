using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The state controller class is responsible for managing and changing the state of the AI 
/// </summary>
public class StateController : MonoBehaviour 
{
	public State currentState;
	public EnemyStats enemyStats;
	public Transform eyes;

	[HideInInspector]
	public BaseAI MyAI;
	
	[HideInInspector] public Transform Target;
	[HideInInspector] public float stateTimeElapsed;	


	void Awake ()
	{
		Target = GameObject.FindWithTag("Player").transform;
		MyAI = GetComponent<BaseAI>();
	}

	public void SetupAI()
	{
		
	}

	void Update()
	{		
		Debug.Assert(currentState,"AI state not set");
		currentState.UpdateState (this);								
	}

	public void TransitionToState(State nextState)
	{
		currentState = nextState;
		OnExitState();
	}

	/// <summary>
	/// This controls the attack timer on an attack action since it's stateless
	/// </summary>
	/// <param name="duration">duration of the timer</param>
	/// <returns>true if timer is done</returns>
	public bool CheckIfCountDownElapsed(float duration)
	{
		stateTimeElapsed += Time.deltaTime;
		return (stateTimeElapsed >= duration);
	}

	private void OnExitState()
	{
		stateTimeElapsed = 0;
	}
}