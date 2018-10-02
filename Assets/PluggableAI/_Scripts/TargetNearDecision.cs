﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNearDecision : Decision
{
	public float Distance;
	
	public override bool Decide(StateController c)
	{
		return Vector3.Distance(c.transform.position, c.Target.position) < Distance;
	}
}
