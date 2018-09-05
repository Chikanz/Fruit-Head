using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

/// <summary>
/// The state controller class is responsible for managing and changing the state of the AI 
/// </summary>
public class StateController : MonoBehaviour 
{
	public State currentState;
	public EnemyStats enemyStats;
	public Transform eyes;

	[HideInInspector] public BaseAI MyAI;
	[HideInInspector] public CombatCharacter MyCC;
	
	[HideInInspector] public Transform Target;
	[HideInInspector] public float stateTimeElapsed;

	public Animator myAnim { get; set; }

	public string StartAnimation;

	private bool active = false; 
	
	public Dictionary<string, object> VarStorage { get; private set; }

	void Awake ()
	{
		//Target = GameObject.FindWithTag("Player").transform;
		MyAI = GetComponent<BaseAI>();
		MyCC = GetComponent<CombatCharacter>();
		VarStorage = new Dictionary<string, object>();

		myAnim = GetComponentInChildren<Animator>();

		CombatManager.OnCombatStart += (sender, args) => OnActive();
		
		//todo targeting
		if (MyCC.Friendly)
		{
			
		}
	}

	//Called on started active
	void OnActive()
	{
		active = true;
		myAnim.SetTrigger(StartAnimation);
		myAnim.SetFloat("Offset", UnityEngine.Random.Range(0.0f,1.0f));
	}

	void Update()
	{
		if (!active) return;
		
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

	/// <summary>
	/// Since our pluggable AI system behaviour is stateless, per enemy variables can't be stored on the scriptabled objects
	/// THUSN'TLY we instead use a dict of objects to store and retrieve any variables we might want
	/// Generics are used to ensure we don't accidentally get the wrong variable type and fuck everything
	/// Also makes it way cleaner so we don't have to check types every time we call this function
	/// generics are fucking litty
	/// </summary>
	public T GetVar<T>(string name)
	{
		object returnVal = null;
		VarStorage.TryGetValue(name, out returnVal);
		Debug.Assert(returnVal != null, "Couldn't get var"); //This func shouldn't return nully boyes, use dict contains instead
		Debug.Assert(returnVal is T, "Var is wrong type"); //Janky type safety ftw
		
		return (T) returnVal; //Cast and return knowing we've got the right type
	}

	public void SetVar(string name, object o)
	{
		VarStorage.Add(name, o);
	}
}