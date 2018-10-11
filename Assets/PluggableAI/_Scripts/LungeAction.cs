using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Lunge")]
public class LungeAction : AIAction
{
	public float force; //todo get this from stats?

	public override void OnEnter(StateController c)
	{
		base.OnEnter(c);
		
		var look = c.Target.position - c.transform.position;
		c.MyAI.ForceLookAt(null);
		c.MyAI.BurstMove(look.normalized * force);
		c.MyCC.UseMove(0);
	}

	public override void OnExit(StateController c)
	{
		c.MyAI.StopBurst();
		c.MyAI.GetComponent<CombatCharacter>().Moves[0].GetComponent<SpawnMove>().ForceStop(); //get first move and stop eet
	}

	public override void Act(StateController c)
	{
		
	}
}
