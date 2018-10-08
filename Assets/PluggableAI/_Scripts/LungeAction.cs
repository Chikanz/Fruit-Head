using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Lunge")]
public class LungeAction : AIAction
{
	public float force = 5;
	
	public override void Act(StateController c)
	{
		var look = c.Target.position - c.transform.position;
		c.MyAI.ForceLookAt(null);
		c.MyAI.Move(look.normalized * force);
		c.MyCC.UseMove(0);
	}
}
