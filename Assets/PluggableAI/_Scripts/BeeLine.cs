using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move in a straight line towards or away from a target
/// </summary>
[CreateAssetMenu (menuName = "PluggableAI/Actions/Beeline")]
public class BeeLine : AIAction
{
	public bool moveTowards = true;
	
	public override void Act(StateController c)
	{
		var m = c.Target.position - c.transform.position;
		c.MyAI.Move(moveTowards ? m : -m);
	}
}
