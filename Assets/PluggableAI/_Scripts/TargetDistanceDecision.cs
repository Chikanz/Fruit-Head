
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDistanceDecision : Decision
{
	[HideInInspector]
	public float Distance;
	
	[HideInInspector]
	public bool Flip;
	
	public override bool Decide(StateController c)
	{
		return Flip ? Vector3.Distance(c.transform.position, c.Target.position) > Distance :
					  Vector3.Distance(c.transform.position, c.Target.position) < Distance;
	}
}
