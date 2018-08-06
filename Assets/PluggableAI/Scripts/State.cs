using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/State")]
public class State : ScriptableObject 
{

	public Action[] actions;
	public Transition[] transitions;	

	public void UpdateState(StateController controller)
	{
		DoActions (controller);
		CheckTransitions (controller);
	}

	private void DoActions(StateController controller)
	{
		foreach (var a in actions)
		{
			a.Act (controller);
		}
	}

	private void CheckTransitions(StateController controller)
	{
		foreach (var t in transitions)
		{
			if (t.Decision.Decide (controller)) //Find out if we should change to this state 
			{
				Debug.Log("Transitioning to state " + t.NextState.name);
				controller.TransitionToState (t.NextState); //do it
			}
		}
	}
}