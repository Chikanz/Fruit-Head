using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Damage = heal amount lol
/// </summary>
public class HealMove : SpawnMove
{
	private bool friendlyHeal = false; //Check which team the heal is originating from

	//hide vars and force assumptions about specialized heal move 
	//never mind we can't do this without writing a custom inspector hmmm
	/*
	[HideInInspector]
	private bool isProjectile = false;
	[HideInInspector]	
	private float Knockback = 0;
	[HideInInspector]
	private int Damage = 0;
	*/

	protected override void Start()
	{
		base.Start();
		friendlyHeal = daddy.GetComponent<CombatCharacter>().Friendly;
	}

	public override void Execute()
	{
		base.Execute();
	}

	//Override parent function
	public override void Trigger(GameObject enemy)
	{
		var targetCC = enemy.GetComponentInParent<CombatCharacter>();
		
		Debug.Assert(targetCC != null, "CC for " +  enemy.transform.parent.name + " was empty!");
		
		if(targetCC && targetCC.Friendly == friendlyHeal) //null check + see if we're on the same team
			targetCC.Heal(Damage);
	}
	
}
