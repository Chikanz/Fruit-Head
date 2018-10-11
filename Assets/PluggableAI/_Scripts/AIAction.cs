using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction : ScriptableObject 
{
	/// <summary>
	/// Meat and potatoes, action that's called on update goes here
	/// </summary>
	/// <param name="c"></param>
	public abstract void Act (StateController c);

	/// <summary>
	/// Called once when we enter this state
	/// </summary>
	/// <param name="c"></param>
	public virtual void OnEnter(StateController c)
	{
		
	}
	
	/// <summary>
	/// Called once when we exit this state
	/// </summary>
	/// <param name="c"></param>
	public virtual void OnExit(StateController c)
	{
		
	}
}