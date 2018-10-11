using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// chef kiss emoji
/// </summary>

[CreateAssetMenu (menuName = "PluggableAI/Actions/Wander")]
public class BatWander : AIAction
{
	private const string startKey = "_start";
	private const string randKey = "_rand";
	
	[Tooltip("How strong the urge to return to this AI's starting point is")]
	public float homingFactor = 2;
	
	public override void OnEnter(StateController c)
	{
		c.SetVar(c.ID() + startKey, c.transform.position); //Stash start point
		c.SetVar(randKey, (float) Random.Range(0,100));
	}

	public override void Act(StateController c)
	{
		//sample 2D perlin noise to get a random multiplier that changes smoothly
		//offset by a random amount so that different bats don't all move the same way
		var rand = c.GetVar<float>(randKey);
		var leftSteer = Mathf.PerlinNoise(Time.time + rand, Time.time);
		var rightSteer = Mathf.PerlinNoise((Time.time + rand) * 2 , Time.time);
		
		//Add multiplier to left and right local vectors 
		Vector3 leftVec = -c.transform.right * leftSteer ;
		Vector3 rightVec = c.transform.right * rightSteer;			
		
		//Get starting pos from var storage
		Vector3 startPos = c.GetVar<Vector3>(c.ID() + startKey);
		var homing = startPos - c.transform.position;		
		
		//Send home boy on its way
		c.MyAI.Move(c.transform.forward  + leftVec + rightVec + (homing * homingFactor));
		
		//Vectors are actually just spicy arrows change my mind
		Debug.DrawLine(c.transform.position, c.transform.position + c.transform.forward * 2 , Color.cyan);
		Debug.DrawLine(c.transform.position + c.transform.forward * 2 , c.transform.position + (c.transform.forward * 2) + leftVec , Color.cyan);
		Debug.DrawLine(c.transform.position + c.transform.forward * 2 , c.transform.position + (c.transform.forward * 2) + rightVec , Color.magenta);		
		Debug.DrawLine(c.transform.position, c.transform.position + c.transform.forward * 2 + leftVec + rightVec, Color.green);
				
		Debug.DrawLine(c.transform.position, c.transform.position + homing, Color.yellow);
	}
}
