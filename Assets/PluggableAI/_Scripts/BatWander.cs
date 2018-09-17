using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Wander")]
public class BatWander : Action 
{
	public override void Act(StateController c)
	{
		float leftSteer, rightSteer;

		leftSteer = Mathf.PerlinNoise(Time.time, 1);
		rightSteer = -Mathf.PerlinNoise(Time.time, 1);
		
		Vector3 leftVec = -c.transform.right* leftSteer;
		Vector3 rightVec = c.transform.right * rightSteer;
		
		//Debug
		Debug.DrawLine(c.transform.position, c.transform.position * leftSteer);
		Debug.DrawLine(c.transform.position, c.transform.position * rightSteer);
		
		c.MyAI.Move(c.transform.forward + leftVec + rightVec);
	}
}
