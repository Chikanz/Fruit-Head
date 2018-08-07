using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Circle")]
public class IdleAction : Action
{
	public float TimeInState = 3;
	
	public override void Act(StateController c)
	{
		
	}
}
