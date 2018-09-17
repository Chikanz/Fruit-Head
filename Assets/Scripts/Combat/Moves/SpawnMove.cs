using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A move that spawns a hitbox or projectile
/// </summary>
public class SpawnMove : Move {
	[SerializeField]
	[Tooltip("projectile to spawn")]
	private GameObject Projectile;
	
	[SerializeField]
	private bool isProjectile;

	[SerializeField]
	[Tooltip("How long the attack object survives for")]
	private float lingerTime;

	[SerializeField]
	[Tooltip("Should we destroy this hitbox after it hits an enemy?")]
	private bool DestroyOnHit;

	private GameObject ActiveObject; //The hitbox/projectile spawned
	
	private bool _isSubscribed;

	[Range(0,500)]
	public int Knockback;
	
	protected override void Start()
	{
		base.Start();
		ToggleChild(false);
	}

	protected override void Update () 
	{
		base.Update();	
	}
	
	/// <summary>
	/// This move's functionality
	/// </summary>
	public override void Execute()
	{
		base.Execute();
		
		//Spawn new or enable child
		if (isProjectile)
		{
			ActiveObject = Instantiate(Projectile, Vector3.zero, Quaternion.identity, transform);
			Destroy(ActiveObject, lingerTime); //Destroy hitbox after linger timer
		}
		else
		{
			ToggleChild(true); //enable child
			StartCoroutine(DelayToggleChild(lingerTime, false)); //disable child on timer
			ActiveObject = transform.GetChild(0).gameObject; //set active object to child
		}

		//Subscribe to hitbox event if it's lonely
		if (!_isSubscribed)
		{
			ActiveObject.GetComponent<Hitbox>().OnHit += Trigger;
			_isSubscribed = true;
		}
	}

	public override void Trigger(GameObject enemy)
	{
		base.Trigger(enemy);
		
		enemy.GetComponentInParent<CombatCharacter>().Attack(new AttackMessage(Damage, daddy.gameObject, Knockback));

		if (DestroyOnHit)
		{
			if (isProjectile)
			{
				Destroy(ActiveObject);
				_isSubscribed = false;
			}
			else
			{
				ToggleChild(false);
			}
		}    
	}

	private void ToggleChild(bool enabled)
	{
		transform.GetChild(0).gameObject.SetActive(enabled);
	}
	
	private IEnumerator DelayToggleChild(float timer, bool enabled)
	{
		yield return new WaitForSeconds(timer);
		ToggleChild(enabled);
	}
}
