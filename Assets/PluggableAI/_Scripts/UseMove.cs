using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/UseMove")]
public class UseMove : AIAction
{
	public int moveIndex = 0;
	
	public override void OnEnter(StateController c)
	{
		c.MyCC.UseMove(moveIndex);
	}

	public override void Act(StateController c)
	{
		
	}
}
