using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class DodgeMove : Move
{
	private bool moving = false;
	
	// Use this for initialization
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();

		if (moving)
		{
			daddy.transform.Translate(Vector3.left * 5 * Time.deltaTime);
		}
		
	}

	public override void Execute()
	{
		base.Execute();

		toggleMoveSystems(false);
		//daddy.GetComponent<Rigidbody>().AddForce(Vector3.left * 3000);
		Invoke("Reset",0.8);
		
	}

	void Reset()
	{
		toggleMoveSystems(true);
	}

	void toggleMoveSystems(bool e)
	{
		moving = !e;
		daddy.GetComponent<ThirdPersonUserControl>().enabled = e;
		daddy.GetComponent<ThirdPersonCharacter>().enabled = e;
	}
}
