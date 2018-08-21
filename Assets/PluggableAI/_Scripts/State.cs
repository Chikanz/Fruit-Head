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
			if(!a) continue;
			a.Act (controller);
		}
	}

	private void CheckTransitions(StateController c)
	{
		foreach (var t in transitions)
		{
			if (t.Decision.Decide (c)) //Find out if we should change to this state 
			{
				Debug.Assert(t.NextState != null, "Next state is null, dum dum");
				
				Debug.Log("Transitioning to state " + t.NextState.name);
				c.TransitionToState (t.NextState); //do it
				
				if(!t.AnimationToPlay.Equals("")) //Optional animation to trigger
					c.GetComponentInChildren<Animator>().SetTrigger(t.AnimationToPlay);
			}
		}
	}
}