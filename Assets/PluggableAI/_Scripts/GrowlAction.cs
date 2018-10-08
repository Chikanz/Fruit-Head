using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Growl")]
public class GrowlAction : AIAction 
{
	public override void Act(StateController c)
	{
		c.MyAI.ForceLookAt(c.Target);
	}
}
